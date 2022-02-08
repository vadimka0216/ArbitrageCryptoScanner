using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using RestSharp;

namespace nsOpenUrl
{

    class OpenUrl
    {
        static private string RequestUrl(HttpWebRequest request)
        {
            string resultPage = "";

            try
            {
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8, true))
                    {
                        resultPage = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch { };
            return resultPage;
        }

        static public string RequestUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            return RequestUrl(request);
        }

        static public string RequestUrl(string url, string proxy, int timeout=60000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = method;
            request.Proxy = new WebProxy(proxy);
            request.Timeout = timeout;
            return RequestUrl(request);
        }

        static public string RequestUrlPostExEx(string url, Dictionary<string, string> data, string proxy = null, RestSharp.RestRequest request = null)
        {
            var client = new RestClient(url);
            //client.CookieContainer = new CookieContainer();
            return RequestUrlExEx(client, proxy, data, request);
        }


        static public string RequestUrlExEx(RestClient client, string proxy = null, Dictionary<string, string> data = null, RestRequest request = null)
        {
            if (client == null) return "";

            if (proxy != null)
                client.Proxy = new WebProxy(proxy);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            if (request == null)
                request = new RestRequest();//"resource/{id}");
            if (data != null)
            {
                foreach (string key in data.Keys)
                    request.AddParameter(key, data[key]);
            }

            request.AddHeader("accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            //request.AddHeader("referer", url);
            request.AddHeader("accept-language", "ru,en;q=0.9");
            request.AddHeader("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.216 YaBrowser/21.5.4.607 Yowser/2.5 Safari/537.36");
            //request.AddHeader("accept-encoding", "gzip, deflate, br");//иногда выдает сжатие br
            request.AddHeader("content-type", "application/x-www-form-urlencoded");//"text /html; charset=UTF-8");
            IRestResponse response;
            if (data == null) response = client.Get(request);
            else response = client.Post(request);
            //System.Windows.Forms.MessageBox.Show(response.ContentEncoding);

            var content = response.Content; // Raw content as string

            return content;
        }

        static public string OpenUrlEmulator(string url)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                    | SecurityProtocolType.Tls11
                    | SecurityProtocolType.Tls12
                    | SecurityProtocolType.Ssl3;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //Http - set headers:
            request.CookieContainer = new CookieContainer();
            request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.Referer = url;
            request.Headers.Add("Accept-Language", "ru,en;q=0.9");
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.111 YaBrowser/21.2.1.107 Yowser/2.5 Safari/537.36";
            return RequestUrl(request);
        }

    }

    class OpenUrlModern
    {
        static public string RequestUrl(string url, string proxy, int timeout=60000)
        {
            var client=new RestClient(url);
            var request = new RestRequest();
            request.ReadWriteTimeout = request.Timeout =
                client.ReadWriteTimeout = client.Timeout = timeout;
            return OpenUrl.RequestUrlExEx(client, proxy, null, request);
        }
    }
}
