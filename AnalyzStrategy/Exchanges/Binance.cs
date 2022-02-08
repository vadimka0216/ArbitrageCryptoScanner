using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnalyzStrategy.Exchanges
{
    class Binance : BaseExchange
    {
        public Binance():base("Binance") {}

        public override int getInfoPair(ref InfoPair info)
        {
            int res = 0;
            Pair pair = info.pair;
            info.sprice = info.bprice = -1;
            string response = RequestUrl("https://api.binance.com/api/v1/ticker/24hr?symbol=" + pair.name1 + pair.name2);
            Regex bidPrice = new Regex("\"bidPrice\":\"(\\d+\\.\\d+)\"");
            Regex askPrice = new Regex("\"askPrice\":\"(\\d+\\.\\d+)\"");
            if (bidPrice.IsMatch(response) && askPrice.IsMatch(response))
            {
                string result = bidPrice.Match(response).Groups[1].Value.ToString();
                doubleEn.TryParse(result, out info.bprice);
                result = askPrice.Match(response).Groups[1].Value.ToString();
                doubleEn.TryParse(result, out info.sprice);
                //Console.WriteLine("Цена покупки: {0}; Цена продажи: {1}", bprice, aprice);
            }
            else
            {
                res = (response.IndexOf("\"code\":-1100") >= 0) ? 2 : 1;
            }

            return res;
        }
        public override Pair[] getPairs()
        {
            Pair[] result = null;
            Regex regPair1 = new Regex("\"baseAsset\":\"([^\"]+)\"");
            Regex regPair2=new Regex("\"quoteAsset\":\"([^\"]+)\"");
            string response = RequestUrl("https://api.binance.com/api/v3/exchangeInfo");
            MatchCollection matches1 = regPair1.Matches(response);
            MatchCollection matches2 = regPair2.Matches(response);
            if (matches1.Count>0&&matches1.Count == matches2.Count)
            {
                result = new Pair[matches1.Count];
                for (int i=0; i<matches1.Count; i++)
                {
                    result[i] = new Pair(matches1[i].Groups[1].Value, matches2[i].Groups[1].Value);
                }
            }
            return result;
        }
    }
}
