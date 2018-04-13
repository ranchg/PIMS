using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    public class DigitalConverHelper
    {
        public static string Object2String(object obj)
        {
            if (null == obj)
            {
                return null;
            }

            return obj.ToString();
        }

        public static int Object2Int(object obj)
        {
            if (null == obj)
            {
                return 0;
            }
            int result;
            if (int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return 0;
        }

        public static double Object2Double(object obj)
        {
            if (null == obj)
            {
                return 0;
            }
            double result;
            if (double.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return 0;
        }

        public static double String2Double(string obj)
        {
            if (obj == "")
            {
                return 0;
            }
            double result;
            if (double.TryParse(obj, out result))
            {
                return result;
            }
            return 0;
        }
    }
}
