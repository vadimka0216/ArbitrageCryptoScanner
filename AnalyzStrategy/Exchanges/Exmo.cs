using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnalyzStrategy.Exchanges
{
    class Exmo: BaseExchange
    {
        public Exmo() : base("Exmo") { }
        public override int getInfoPair(ref InfoPair info) 
        {
            int res = 0;
            info.sprice = info.bprice = -1;
            string response = RequestUrl("https://api.exmo.com/v1.1/ticker");
            Regex pattern = new Regex("\"" + info.pair.name1 + "_" + info.pair.name2
                + "\":\\{\"buy_price\":\"([\\d\\.]+)\",\"sell_price\":\"([\\d\\.]+)\",");

            if (pattern.IsMatch(response))
            {
                SearchValues(pattern, response, ref info);
            }
            else
            {
                res = response.IndexOf("buy_price") >= 0 ? 2 : 1;
            }

            return res;
        }
        public override Pair[] getPairs() 
        {
            Pair[] result = null;
            Regex regPairs=new Regex("\"([^_]+)_([^\"]+)\":{\"buy_price\":");
            string response = RequestUrl("https://api.exmo.com/v1.1/ticker");
            InitialPairs(ref result, regPairs, response);
            return result;
        }
    }
}
