using System.Globalization;

namespace GraphVisual
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
}
