using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using nsOpenUrl;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json;
using System.IO;

using System.Net.NetworkInformation;
using System.Net;

namespace AnalyzStrategy
{
    abstract class BaseManageProxy
    {
        //abstract public BaseManageProxy();
        public delegate void function(int countProxy);
        abstract public void reloadList(function callbackState);

        abstract public void setInvalid(string proxy);//rating  turn down, save temp rating

        abstract public string getBestNext();//next uniq proxy

        abstract public void resetBestUniqs();//reload rating,

        //call reloadRating();

        abstract public void reloadRating();//update rating, tempList.Clear

    }

    class ManageProxy : BaseManageProxy
    {
        protected const long WaitUseProxy = 10000;//10 seconds
        public struct ProxyRate
        {
            public ProxyRate(string _proxy, int _rating = 0)
            {
                proxy = _proxy;
                rating = _rating;
                lastUse = 0;
            }
            public string proxy;
            public int rating;
            public long lastUse;
        }
        object locker = new object();

        const string apiCheckProxy = "https://api.openproxy.space/list?skip=0&ts={0}";
        Regex regCode = new Regex("\"protocols\":\\[1,2\\],\"title\":\"[^\"]+\",\"code\":\"([^\"]+)\"");
        protected List<ProxyRate> proxys;
        int counter = 0;
        protected Regex patternProxys = new Regex("(\\d{1,3}\\.){3}(\\d{1,3}):\\d{1,5}");

        public ManageProxy()
        {
            proxys = new List<ProxyRate>();
            counter = 0;
        }
        private long getTimeMilliseconds()
        {
            return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        long lastReloadList = 0; const long waitReloadList = 300000;//in ms

        protected virtual void loadNewProxy()
        {
            string link = "https://openproxy.space/list/";
            string result = OpenUrl.OpenUrlEmulator(string.Format(apiCheckProxy, getTimeMilliseconds()));
            if (regCode.IsMatch(result))
            {
                string code = regCode.Match(result).Groups[1].Value;
                link += code;
                result = OpenUrl.OpenUrlEmulator(link);
                MatchCollection matches = patternProxys.Matches(result);
                if (matches.Count > 0)
                {
                    proxys.Clear();
                    foreach (Match m in matches)
                    {
                        proxys.Add(new ProxyRate(m.Groups[0].Value));
                    }
                }
            }
        }

        protected function _callbackState;
        public override void reloadList(function callbackState)//call in main thread
        {
            lock (locker)
            {
                if (lastReloadList + waitReloadList < getTimeMilliseconds())
                {
                    _callbackState = callbackState;
                    lastReloadList = getTimeMilliseconds();
                    loadNewProxy();
                }
            }
        }

        public int getCount()
        {
            int res = 0;
            lock (locker) { res = proxys.Count; }
            return res;
        }
        public override string getBestNext()
        {
            string result = null;
            lock (locker)
            {
                if (proxys.Count <= counter)
                {
                    counter = 0;
                }

                for (; counter < proxys.Count; counter++)
                {
                    var proxy = proxys[counter];
                    if (WaitUseProxy + proxy.lastUse < MiscFuncs.GetTickCount())
                    {
                        result = proxy.proxy;
                        proxy.lastUse = MiscFuncs.GetTickCount();
                        proxys[counter++] = proxy;
                        break;
                    }
                }
                //counter++;
            }
            return result;
        }
        public override void reloadRating()
        {
            lock (locker)
            {
                counter = 0;
                proxys.Sort(delegate(ProxyRate x, ProxyRate y)
                {
                    /*if (x.rating == null && y.rating == null) return 0;
                    else if (x.rating == null) return -1;
                    else if (y.rating == null) return 1;
                    else*/
                    return y.rating.CompareTo(x.rating);
                });
            }
        }
        public override void resetBestUniqs()
        {
            lock (locker)
            {
                for (int i = 0; i < proxys.Count; i++)
                {
                    var obj = proxys[i];
                    obj.rating = 0;
                    proxys[i] = obj;
                }
            }
        }
        public override void setInvalid(string proxy)
        {
            lock (locker)
            {
                for (int i = 0; i < proxys.Count; i++)
                {
                    if (proxys[i].proxy == proxy)
                    {
                        var obj = proxys[i];
                        obj.rating--;
                        proxys[i] = obj;
                        break;
                    }
                }
            }
        }

    }

    class ManageProxy_v2 : ManageProxy
    {
        public ManageProxy_v2() : base() { }
        protected override void loadNewProxy()
        {
            string[] urls = new string[] { "https://openproxy.space/list/http", 
                "https://free-proxy-list.net/",
                "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all"
            };

            List<MatchCollection> listMatches = new List<MatchCollection>();
            int count = 0;
            foreach (var url in urls)
            {
                MatchCollection matches = patternProxys.Matches(OpenUrl.OpenUrlEmulator(url));
                count += matches.Count;
                listMatches.Add(matches);
            }
            if (count > 0)
            {
                proxys.Clear();
                FormatScanSave prevScan;
                if (!tryGetResultLastScan(out prevScan))
                {
                    if (prevScan.lastScan > 0)
                    {
                        Directory.CreateDirectory(folderOlderCheck);
                        SaveScanResult(folderOlderCheck + "\\" + (new DateTime(prevScan.lastScan)).ToString("dd_MM_yyyy-HH_mm") + ".json", prevScan);
                    }
                    prevScan = new FormatScanSave();
                    prevScan.proxys = new List<FormatScanSave.LogProxy>();
                }
                foreach (var matches in listMatches)
                    StartScanner(prevScan.proxys, matches, count);
                SaveScanResult(pathFileSave, prevScan);
                prevScan.proxys.Clear();
            }
            listMatches.Clear();
        }

        const string pathFileSave = "scan_proxy.json";
        const string folderOlderCheck = "ProxyScanner";

        public struct FormatScanSave
        {
            public struct LogProxy
            {
                public LogProxy(string _proxy, bool _isValid)
                {
                    proxy = _proxy;
                    isValid = _isValid;
                }
                public string proxy;
                public bool isValid;
            }
            public List<LogProxy> proxys;
            public long lastScan;//in ticks
        }
        protected virtual void StartScanner(List<FormatScanSave.LogProxy> prevScan, MatchCollection matches, int maxCountProxy)
        {
            int count = matches.Count;
            var threads = new ThreadBigList();
            object lock_saveScan = new object();
            //List<FormatScanSave.LogProxy> currScan=new List<FormatScanSave.LogProxy>();

            foreach (Match m in matches)
            {
                string proxy = m.Groups[0].Value;

                bool isNeedCheck = true; int index = -1;
                for (int i = 0; i < prevScan.Count; i++)
                {
                    var pr = prevScan[i];
                    if (pr.proxy == proxy)
                    {
                        isNeedCheck = pr.isValid;
                        index = i;
                        break;
                    }
                }

                if (isNeedCheck)
                {
                    threads.New(() =>
                    {
                        bool isValid = TestProxy(proxy);

                        //string response = OpenUrlModern.RequestUrl("https://www.google.com/", proxy, 5000);
                        //bool isValid = response.IndexOf("Google") >= 0;

                        lock (lock_saveScan)
                        {
                            if (isValid)
                            {
                                proxys.Add(new ProxyRate(proxy));
                            }

                            if (index == -1)
                            {
                                prevScan.Add(new FormatScanSave.LogProxy(proxy, isValid));
                            }
                            else
                            {
                                var pr = prevScan[index]; pr.isValid = isValid;
                                prevScan[index] = pr;
                            }
                        }
                        //currScan.Add(new FormatScanSave.LogProxy(proxy, isValid));
                        if (_callbackState != null) _callbackState(maxCountProxy);
                    });
                }
                else
                {
                    if (_callbackState != null) _callbackState(maxCountProxy);
                }
            }
            threads.Wait();

            //foreach (var curr in currScan)
            //{
            //    if (curr.isValid)
            //    {
            //        proxys.Add(new ProxyRate(curr.proxy));
            //    }

            //    for (int i = 0; i < prevScan.Count; i++)
            //    {
            //        var prev=prevScan[i];
            //        if (curr.proxy == prev.proxy)
            //        {
            //            prev.isValid = curr.isValid;
            //            prevScan[i] = prev;
            //            break;
            //        }
            //    }
            //}
            
            //currScan.Clear();
            
        }
        protected void SaveScanResult(string fileName, FormatScanSave obj)
        {
            obj.lastScan = DateTime.Now.Ticks;
            File.WriteAllText(fileName, JsonConvert.SerializeObject(obj));
        }
        protected bool tryGetResultLastScan(out FormatScanSave resultScan)
        {
            bool result = false;
            resultScan = new FormatScanSave();
            if (File.Exists(pathFileSave))
            {
                try
                {
                    resultScan = JsonConvert.DeserializeObject<FormatScanSave>(File.ReadAllText(pathFileSave));
                    result = true;
                }
                catch { };
                result = result && DateTime.Now.Subtract(new DateTime(resultScan.lastScan)).Hours <= 4;
            }
            return result;
        }


        public static bool TestProxy(Ping ping, string proxy)
        {
            var ip = proxy.Split(':')[0];
            //var port = proxy.Split(':')[1];
            // var ping = new System.Net.NetworkInformation.Ping();
            var reply = ping.Send(ip, 1500);
            return reply.Status == IPStatus.Success;
        }
        public static bool TestProxy(string proxy)
        {
            bool result = true;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://google.com");
            request.Proxy = new WebProxy(proxy);//(proxy.IPEndPoint.Address.ToString(), proxy.IPEndPoint.Port);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
            request.Timeout = 2000;
            try
            {
                var response = request.GetResponse();
            }
            catch (Exception)
            {
                result = false;
            }
            //bool result = false;
            //HttpRequest request = new HttpRequest();
            ////request.IgnoreProtocolErrors = true;
            //request.Proxy = HttpProxyClient.Parse(proxy);
            //request.ConnectTimeout = 2000;
            //try
            //{
            //    request.Get("http://google.com");
            //    result = true;
            //}
            //catch (HttpException) { };
            return result;
        }
    }
}
