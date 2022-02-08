using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnalyzStrategy.Exchanges
{
    class Hitbtc:BaseExchange
    {
        public Hitbtc() : base("Hitbtc") { }
        public override int getInfoPair(ref InfoPair info)
        {
            int res = 0;
            string response=RequestUrl("https://api.hitbtc.com/api/3/public/ticker/"+info.pair.name1+info.pair.name2);
            Regex regValue=new Regex("\"ask\":\"([^\"]+)\",\"bid\":\"([^\"]+)\"");
            if (regValue.IsMatch(response))
            {
                base.SearchValues(regValue, response, ref info);
                double buf = info.bprice;
                info.bprice = info.sprice;
                info.sprice = buf;
            }
            else
            {
                res=response.IndexOf("\"error\":")>=0 ? 2:1;
            }
            return res;
        }
        public override Pair[] getPairs() 
        {
            Pair[] pairs=null;
            string response=RequestUrl("https://api.hitbtc.com/api/3/public/symbol");

            Regex regPairs=new Regex("\"base_currency\":\"([^\"]+)\",\"quote_currency\":\"([^\"]+)\"");
            base.InitialPairs(ref pairs, regPairs, response);
            return pairs;
        }
    }
}
