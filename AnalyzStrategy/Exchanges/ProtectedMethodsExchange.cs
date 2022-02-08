using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnalyzStrategy.Exchanges
{
    partial class BaseExchange
    {
        protected void InitialPairs(ref Pair[] result, Regex regPairs, string response)
        {
            MatchCollection matches = regPairs.Matches(response);
            if (matches.Count > 0)
            {
                result = new Pair[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                {
                    string s1 = matches[i].Groups[1].Value;
                    string s2 = matches[i].Groups[2].Value;
                    result[i] = new Pair(s1.ToUpper(), s2.ToUpper());
                }
            }
        }
        protected void SearchValues(Regex pattern, string response, ref InfoPair info)
        {
            Match result = pattern.Match(response);
            if (result.Groups.Count > 2)
            {
                doubleEn.TryParse(result.Groups[1].Value.ToString(), out info.bprice);
                doubleEn.TryParse(result.Groups[2].Value.ToString(), out info.sprice);
                //Console.WriteLine("Цена покупки: {0}; Цена продажи: {1}", bprice, aprice);
            }
        }
    }
}
