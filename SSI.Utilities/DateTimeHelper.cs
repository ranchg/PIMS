using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    public class DateTimeHelper
    {
        public static int DiffDays(DateTime dtfrm, DateTime dtto)
        {
            TimeSpan span = (TimeSpan)(dtto.Date - dtfrm.Date);
            return span.Days;
        }

        public string GetChineseWeekDay(int y, int m, int d)
        {
            string[] strArray = new string[] { "日", "一", "二", "三", "四", "五", "六" };
            if (m < 3)
            {
                m += 12;
                y--;
            }
            if (((y % 400) == 0) || (((y % 100) != 0) && ((y % 4) == 0)))
            {
                d--;
            }
            else
            {
                d++;
            }
            return ("星期" + strArray[((((((d + (2 * m)) + ((3 * (m + 1)) / 5)) + y) + (y / 4)) - (y / 100)) + (y / 400)) % 7]);
        }

        public static string GetDate(int i)
        {
            return DateTime.Now.AddDays((double)i).ToString("yyyy-MM-dd");
        }

        public static int GetDaysOfMonth(DateTime dt)
        {
            int year = dt.Year;
            switch (dt.Month)
            {
                case 1:
                    return 0x1f;

                case 2:
                    return (IsRuYear(year) ? 0x1d : 0x1c);

                case 3:
                    return 0x1f;

                case 4:
                    return 30;

                case 5:
                    return 0x1f;

                case 6:
                    return 30;

                case 7:
                    return 0x1f;

                case 8:
                    return 0x1f;

                case 9:
                    return 30;

                case 10:
                    return 0x1f;

                case 11:
                    return 30;

                case 12:
                    return 0x1f;
            }
            return 0;
        }

        public static int GetDaysOfMonth(int iYear, int Month)
        {
            switch (Month)
            {
                case 1:
                    return 0x1f;

                case 2:
                    return (IsRuYear(iYear) ? 0x1d : 0x1c);

                case 3:
                    return 0x1f;

                case 4:
                    return 30;

                case 5:
                    return 0x1f;

                case 6:
                    return 30;

                case 7:
                    return 0x1f;

                case 8:
                    return 0x1f;

                case 9:
                    return 30;

                case 10:
                    return 0x1f;

                case 11:
                    return 30;

                case 12:
                    return 0x1f;
            }
            return 0;
        }

        public static int GetDaysOfYear(DateTime dt)
        {
            return (IsRuYear(dt.Year) ? 0x16e : 0x16d);
        }

        public static int GetDaysOfYear(int iYear)
        {
            return (IsRuYear(iYear) ? 0x16e : 0x16d);
        }

        public static string GetNumberWeekDay(DateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            if (month < 3)
            {
                month += 12;
                year--;
            }
            if (((year % 400) == 0) || (((year % 100) != 0) && ((year % 4) == 0)))
            {
                day--;
            }
            else
            {
                day++;
            }
            int num4 = ((((((day + (2 * month)) + ((3 * (month + 1)) / 5)) + year) + (year / 4)) - (year / 100)) + (year / 400)) % 7;
            return num4.ToString();
        }

        public static string GetToday()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string GetToday(string format)
        {
            return DateTime.Now.ToString(format);
        }

        public static int GetWeekAmount(int year)
        {
            DateTime time = new DateTime(year, 12, 0x1f);
            GregorianCalendar calendar = new GregorianCalendar();
            return calendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static string GetWeekNameOfDay(DateTime dt)
        {
            string str = string.Empty;
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "星期日";

                case DayOfWeek.Monday:
                    return "星期一";

                case DayOfWeek.Tuesday:
                    return "星期二";

                case DayOfWeek.Wednesday:
                    return "星期三";

                case DayOfWeek.Thursday:
                    return "星期四";

                case DayOfWeek.Friday:
                    return "星期五";

                case DayOfWeek.Saturday:
                    return "星期六";
            }
            return str;
        }

        public static int GetWeekNumberOfDay(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 7;

                case DayOfWeek.Monday:
                    return 1;

                case DayOfWeek.Tuesday:
                    return 2;

                case DayOfWeek.Wednesday:
                    return 3;

                case DayOfWeek.Thursday:
                    return 4;

                case DayOfWeek.Friday:
                    return 5;

                case DayOfWeek.Saturday:
                    return 6;
            }
            return 0;
        }

        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar calendar = new GregorianCalendar();
            return calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        private static bool IsRuYear(int iYear)
        {
            int num = iYear;
            return (((num % 400) == 0) || (((num % 4) == 0) && ((num % 100) != 0)));
        }

        public static DateTime ToDate(string strInput)
        {
            try
            {
                return DateTime.Parse(strInput);
            }
            catch (Exception)
            {
                return DateTime.Today;
            }
        }

        public static string ToString(DateTime oDateTime, string strFormat)
        {
            try
            {
                switch (strFormat.ToUpper())
                {
                    case "SHORTDATE":
                        return oDateTime.ToShortDateString();

                    case "LONGDATE":
                        return oDateTime.ToLongDateString();
                }
                return oDateTime.ToString(strFormat);
            }
            catch (Exception)
            {
                return oDateTime.ToShortDateString();
            }
        }

        public static void WeekRange(int year, int weekOrder, ref DateTime firstDate, ref DateTime lastDate)
        {
            DateTime time = new DateTime(year, 1, 1);
            int num = Convert.ToInt32(time.DayOfWeek);
            int num2 = (-1 * num) + 1;
            int num3 = 7 - num;
            firstDate = time.AddDays((double)num2).Date;
            lastDate = time.AddDays((double)num3).Date;
            if (weekOrder != 1)
            {
                int num4 = (weekOrder - 1) * 7;
                firstDate = firstDate.AddDays((double)num4);
                lastDate = lastDate.AddDays((double)num4);
            }
        }
    }
}
