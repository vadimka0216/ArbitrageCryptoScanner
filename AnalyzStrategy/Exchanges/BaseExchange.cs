using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsOpenUrl;

namespace AnalyzStrategy.Exchanges
{
    abstract class AbstractExchange
    {
        public struct InfoPair
        {
            public InfoPair(Pair _pair)
            {
                pair = _pair;
                sprice = bprice = volume = -1;
            }
            public Pair pair;
            public double sprice;
            public double bprice;
            public double volume;
        }
        public struct Pair
        {
            public Pair(string pair1, string pair2)
            {
                name1 = pair1; name2 = pair2;
            }
            public string name1;
            public string name2;
            public override string ToString() { return name1 + name2; }
        }
        abstract public void setProxy(string proxy);
        abstract public void setKeys(string key);
        abstract public int getInfoPair(ref InfoPair info); //int - 0 успех
                                  //1 - ошибка соединения
                                  //2 - ошибка, не найдена пара
        abstract public Pair[] getPairs();//null - error
        abstract public string getName();
    }

    partial class BaseExchange:AbstractExchange
    {
        string name;
        protected string proxy;
        public BaseExchange(string nameExchange) { name = nameExchange; }
        public override int getInfoPair(ref InfoPair info) { return 0; }
        public override string getName() { return name; }
        public override Pair[] getPairs() { return null; }
        public override void setKeys(string key) {}
        public override void setProxy(string _proxy) { proxy = _proxy; }
        public virtual string RequestUrl(string url)
        {
            string result = "";
            if (proxy==null)
            {
                result = OpenUrl.RequestUrl(url);
            }
            else
            {
                result = OpenUrlModern.RequestUrl(url, proxy, 4500);
            }
            return result;
        }
    }

    /* Example class:
    class Binance : BaseExchange
    {
        public Binance():base("Example") {}
        public override int getInfoPair(ref InfoPair info) {}
        public override Pair[] getPairs() {}
    }
    */

    class TestExchange
    {//deps: WinForm
        BaseExchange exchange;
        public TestExchange(BaseExchange bot)
        {
            exchange = bot;
        }
        public void Log(string msg)
        {
            System.Windows.Forms.MessageBox.Show(msg,"Information",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information);
        }
        public void RunTests()
        {
            BaseExchange.Pair[] pairs = exchange.getPairs();
            if (pairs!=null)
            {
                BaseExchange.InfoPair info=new BaseExchange.InfoPair(pairs[0]);
                int result = exchange.getInfoPair(ref info);
                string pair = pairs[0].name1 + pairs[0].name2;
                if (result==0)
                {
                    Log("Биржа " + exchange.getName() + " готова к эксплуатации, информация о паре " + pair +
                        ": "+info.bprice+" < "+info.sprice+" | "+info.volume);
                }
                else
                {
                    Log("На бирже: " + exchange.getName() + " не удалось получить информацию о паре: " + pair);
                }
            }
            else
            {
                Log("Не удалось получить список торговых пар на бирже: "+exchange.getName());
            }
        }
    }
}
