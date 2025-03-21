using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MNKSolve
{
    public class GeoLogUtils
    {
        public static String CorrectDecimalSeparator(String strNumber)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                String separator = ci.NumberFormat.NumberDecimalSeparator;
                Int32 indexPoint, lengthStr;
                lengthStr = strNumber.Length;
                if (separator == ",")
                {
                    indexPoint = strNumber.IndexOf('.');
                    if (indexPoint > -1)
                        strNumber = strNumber.Substring(0, indexPoint) + ',' + strNumber.Substring(indexPoint + 1, lengthStr - indexPoint - 1);
                    //      else strNumber = "-1";
                }
                if (separator == ".")
                {
                    indexPoint = strNumber.IndexOf(',');
                    if (indexPoint > -1)
                    {
                        strNumber = strNumber.Substring(0, indexPoint) + '.' + strNumber.Substring(indexPoint + 1, lengthStr - indexPoint - 1);
                    }
                    //    else strNumber = "-1";
                }
                return strNumber;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " --- " + ex.StackTrace);
                return "";
            }
        }

        public static double mTryParse(string text)
        {
            try
            {
                string tempString = Regex.Replace(text.Trim(), @"[^0-9.,-]", "");
                tempString = GeoLogUtils.CorrectDecimalSeparator(tempString);
                double x0;
                if (Double.TryParse(tempString, out x0))
                {
                    return x0;
                }
                else
                    return double.NaN;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " --- " + ex.StackTrace);
                return double.NaN;
            }

        }
    }
}
