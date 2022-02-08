using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnalyzStrategy.Exchanges
{
    class Huobi: BaseExchange
    {
        public Huobi() : base("Huobi") { }
        public override int getInfoPair(ref InfoPair info) 
        {
            int res = 0;
            string pair1=info.pair.name1.ToLower();
            string pair2=info.pair.name2.ToLower();
            Regex pattern = new Regex("\"symbol\":\"" + pair1 + pair2 + "\",\"open\":[^\\}]+\"bid\":([^\"]+),\"bidSize\":[^\"]+,\"ask\":([^\"]+),\"");
            string response = RequestUrl("https://api.huobi.pro/market/tickers");
            if (pattern.IsMatch(response))
            {
                SearchValues(pattern, response, ref info);
            }
            else
            {
                res=response.IndexOf("symbol") >= 0 ? 2 : 1;
            }
            return res;
        }
        public override Pair[] getPairs()
        {
            Pair[] result=null;
            Regex regPairs = new Regex("\"base-currency\":\"([^\"]+)\",\"quote-currency\":\"([^\"]+)\"[^\\}]+\"state\":\"online\"");
            string response = RequestUrl("https://api.huobi.pro/v1/common/symbols");
            InitialPairs(ref result, regPairs, response);
            return result;
        }
    }
}
