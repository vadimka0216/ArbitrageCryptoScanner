using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnalyzStrategy.Exchanges
{
    class Yobit: BaseExchange
    {
        public Yobit() : base("Yobit") { }
        public override int getInfoPair(ref InfoPair info) 
        {
            int res = 0;
            string pair1 = info.pair.name1;
            string pair2 = info.pair.name2;

            pair1 = pair1.ToLower(); pair2 = pair2.ToLower();
            Dictionary<string, string> exceptions = new Dictionary<string, string>(); exceptions.Add("rub", "rur");
            if (exceptions.ContainsKey(pair2)) pair2 = exceptions[pair2]; exceptions.Clear();

            info.sprice = info.bprice = -1;
            string response = RequestUrl("https://yobit.net/api/3/ticker/" + pair1 + "_" + pair2);
            Regex pattern = new Regex("\"buy\":([\\d\\.]+),\"sell\":([\\d\\.]+),\"");

            if (pattern.IsMatch(response))
            {
                SearchValues(pattern, response, ref info);
            }
            else
            {
                res=response.IndexOf("\"Invalid pair name:") >= 0 ? 2 : 1;
            }

            return res;
        }
        public override Pair[] getPairs() 
        {
            Pair[] result = null;
            string response=RequestUrl("https://yobit.net/api/3/info");
            Regex regPairs = new Regex("\\},\"([^_]+)_([^\"]+)\":");
            InitialPairs(ref result, regPairs, response);
            return result;
        }
    }
}
