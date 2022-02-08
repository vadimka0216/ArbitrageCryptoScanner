using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnalyzStrategy.Exchanges
{
    class CryptoCom: BaseExchange
    {
        public CryptoCom():base("CryptoCom") { }
        public override int getInfoPair(ref InfoPair info)
        {
            int result = 0;
            string response=RequestUrl("https://api.crypto.com/v2/public/get-ticker?instrument_name=" + 
            info.pair.name1 + "_" + info.pair.name2);
            Regex reg = new Regex("\"b\":([^\"]+),\"k\":([^\"]+),\"");
            if (reg.IsMatch(response))
            {
                base.SearchValues(reg, response, ref info);
            }
            else
            {
                result=response.IndexOf("{\"code\":0,\"method\":") >= 0 ? 2 : 1;
            }
            return result;
        }
        public override AbstractExchange.Pair[] getPairs()
        {
            Pair[] result = null;
            string response=RequestUrl("https://api.crypto.com/v2/public/get-instruments");
            Regex reg = new Regex("\"quote_currency\":\"([^\"]+)\",\"base_currency\":\"([^\"]+)\"");
            InitialPairs(ref result, reg, response);
            if (result != null)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    string buf = result[i].name1;
                    result[i].name1 = result[i].name2;
                    result[i].name2 = buf;
                }
            }
            return result;
        }
    }
}
