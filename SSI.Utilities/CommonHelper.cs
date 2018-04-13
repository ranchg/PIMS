using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SSI.Utilities
{
    public class CommonHelper
    {
        public static string CostTime(long t)
        {
            long num = t / 0x15f900L;
            long num2 = (t - (num * 0x15f900L)) / 0xea60L;
            long num3 = ((t - (num * 0x15f900L)) - (num2 * 0xea60L)) / 0x3e8L;
            long num4 = ((t - (num * 0x15f900L)) - (num2 * 0xea60L)) - (num3 * 0x3e8L);
            string str2 = GetDateTime(num.ToString() + ":" + num2.ToString() + ":" + num3.ToString() + "." + num4.ToString()).ToString("HH:mm:ss");
            if (str2 == "00:00:00")
            {
                str2 = "00:00:01";
            }
            return str2;
        }

        public static string CreateNo()
        {
            Random random = new Random();
            string str = random.Next(0x3e8, 0x2710).ToString();
            return (DateTime.Now.ToString("yyyyMMddHHmmss") + str);
        }

        public static bool GetBool(object obj)
        {
            if (obj != null)
            {
                bool flag;
                bool.TryParse(obj.ToString(), out flag);
                return flag;
            }
            return false;
        }

        public static byte Getbyte(object obj)
        {
            if ((obj != null) && (obj.ToString() != ""))
            {
                return byte.Parse(obj.ToString());
            }
            return 0;
        }

        public static byte[] GetByte(object obj)
        {
            if ((obj.ToString() != null) && (obj.ToString() != ""))
            {
                return (byte[])obj;
            }
            return null;
        }

        public static DateTime GetDateTime(object obj)
        {
            if ((obj != null) && (obj.ToString() != ""))
            {
                return DateTime.Parse(obj.ToString());
            }
            return DateTime.Now;
        }

        public static decimal GetDecimal(object obj)
        {
            if ((obj != null) && (obj.ToString() != ""))
            {
                return decimal.Parse(obj.ToString());
            }
            return 0M;
        }

        public static float GetFloat(object obj)
        {
            float num;
            float.TryParse(obj.ToString(), out num);
            return num;
        }

        public static string GetFormatDateTime(object obj, string Format)
        {
            if (((obj != null) && (obj.ToString() != null)) && (obj.ToString() != ""))
            {
                return DateTime.Parse(obj.ToString()).ToString(Format);
            }
            return "";
        }

        public static int GetInt(object obj)
        {
            if (obj != null)
            {
                int num;
                int.TryParse(obj.ToString(), out num);
                return num;
            }
            return 0;
        }

        public static int GetInt(object obj, int exceptionvalue)
        {
            if (obj == null)
            {
                return exceptionvalue;
            }
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return exceptionvalue;
            }
            int num = exceptionvalue;
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return exceptionvalue;
            }
        }

        public static long GetLong(object obj)
        {
            if ((obj != null) && (obj.ToString() != ""))
            {
                return long.Parse(obj.ToString());
            }
            return 0L;
        }

        public static long GetLong(object obj, long exceptionvalue)
        {
            if (obj == null)
            {
                return exceptionvalue;
            }
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return exceptionvalue;
            }
            long num = exceptionvalue;
            try
            {
                return Convert.ToInt64(obj);
            }
            catch
            {
                return exceptionvalue;
            }
        }

        public static string GetString(object obj)
        {
            if ((obj != null) && (obj != DBNull.Value))
            {
                return obj.ToString();
            }
            return "";
        }

        public static bool IsDateTime(object obj)
        {
            try
            {
                DateTime time = DateTime.Parse(ToObjectString(obj));
                return ((time > DateTime.MinValue) && (DateTime.MaxValue > time));
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTime(string strValue)
        {
            if ((strValue != null) && (strValue != ""))
            {
                string pattern = @"[1-2]{1}[0-9]{3}((-|[.]){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|[.]){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";
                if (Regex.IsMatch(strValue, pattern))
                {
                    int length = -1;
                    int index = -1;
                    int num3 = -1;
                    if (-1 != (length = strValue.IndexOf("-")))
                    {
                        index = strValue.IndexOf("-", (int)(length + 1));
                        num3 = strValue.IndexOf(":");
                    }
                    else
                    {
                        length = strValue.IndexOf(".");
                        index = strValue.IndexOf(".", (int)(length + 1));
                        num3 = strValue.IndexOf(":");
                    }
                    if (-1 == index)
                    {
                        return true;
                    }
                    if (-1 == num3)
                    {
                        num3 = strValue.Length + 3;
                    }
                    int num4 = Convert.ToInt32(strValue.Substring(0, length));
                    int num5 = Convert.ToInt32(strValue.Substring(length + 1, (index - length) - 1));
                    int num6 = Convert.ToInt32(strValue.Substring(index + 1, (num3 - index) - 4));
                    if (((num5 < 8) && (1 == (num5 % 2))) || ((num5 > 8) && (0 == (num5 % 2))))
                    {
                        if (num6 < 0x20)
                        {
                            return true;
                        }
                    }
                    else if (num5 != 2)
                    {
                        if (num6 < 0x1f)
                        {
                            return true;
                        }
                    }
                    else if (((num4 % 400) == 0) || (((num4 % 4) == 0) && (0 < (num4 % 100))))
                    {
                        if (num6 < 30)
                        {
                            return true;
                        }
                    }
                    else if (num6 < 0x1d)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsDecimal(object obj)
        {
            try
            {
                decimal.Parse(ToObjectString(obj));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDouble(object obj)
        {
            try
            {
                double.Parse(ToObjectString(obj));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsEmpty(string obj)
        {
            return (ToObjectString(obj).Trim() == string.Empty);
        }

        public static bool IsFloat(object obj)
        {
            try
            {
                float.Parse(ToObjectString(obj));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInt(object obj)
        {
            try
            {
                int.Parse(ToObjectString(obj));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsLong(object obj)
        {
            try
            {
                long.Parse(ToObjectString(obj));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime JsonToDateTime(string jsonDate)
        {
            string s = jsonDate.Substring(5, jsonDate.Length - 6) + "+0800";
            DateTimeKind utc = DateTimeKind.Utc;
            int index = s.IndexOf('+', 1);
            if (index == -1)
            {
                index = s.IndexOf('-', 1);
            }
            if (index != -1)
            {
                utc = DateTimeKind.Local;
                s = s.Substring(0, index);
            }
            long num2 = long.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
            DateTime time4 = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long ticks = time4.Ticks;
            DateTime time = new DateTime((num2 * 0x2710L) + ticks, DateTimeKind.Utc);
            switch (utc)
            {
                case DateTimeKind.Unspecified:
                    return DateTime.SpecifyKind(time.ToLocalTime(), DateTimeKind.Unspecified);

                case DateTimeKind.Utc:
                    return time;

                case DateTimeKind.Local:
                    return time.ToLocalTime();
            }
            return time;
        }

        public static string RndNum(int codeNum)
        {
            StringBuilder builder = new StringBuilder(codeNum);
            Random random = new Random();
            for (int i = 1; i < (codeNum + 1); i++)
            {
                int num2 = random.Next(9);
                builder.AppendFormat("{0}", num2);
            }
            return builder.ToString();
        }

        public static string TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            double elapsedMilliseconds = watch.ElapsedMilliseconds;
            return elapsedMilliseconds.ToString();
        }

        public static Stopwatch TimerStart()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            return stopwatch;
        }

        public static DateTime? ToDateTime(object obj)
        {
            if ((obj != null) && (obj.ToString() != ""))
            {
                return new DateTime?(DateTime.Parse(obj.ToString()));
            }
            return null;
        }

        public static string ToObjectString(object obj)
        {
            return ((obj == null) ? string.Empty : obj.ToString());
        }

        public static string WebPathTran(string path)
        {
            try
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            catch
            {
                return path;
            }
        }

        public static string GetGuid
        {
            get
            {
                return Guid.NewGuid().ToString().ToLower();
            }
        }

        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="fromBase">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="toBase">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        /// <returns></returns>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from);  //先转成10进制
                string result = Convert.ToString(intValue, to);  //再转成目标进制
                if (to == 2)
                {
                    int resultLength = result.Length;  //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
