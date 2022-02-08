using System.Globalization;
using System.Runtime.InteropServices;

namespace AnalyzStrategy
{
    class doubleEn
    {
        public static bool TryParse(string s, out double result)
        {
            return double.TryParse(s, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result);
        }
        public static CultureInfo getCulture()
        {
            return CultureInfo.GetCultureInfo("en-US");
        }
    }
    class MiscFuncs
    {
        [DllImport("kernel32.dll")]//, CharSet = CharSet.Unicode)]
        public static extern uint GetTickCount();

    }
}
