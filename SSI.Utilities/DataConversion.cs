using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    public class DataConversion
    {
        /// <summary>
        /// 高位在后 小端模式
        /// </summary>
        /// <param name="beginAddr">开始下标</param>
        /// <param name="length">长度</param>
        /// <param name="Source">数组</param>
        /// <returns></returns>
        public static long multiByteConvertLongLow(int beginAddr, int length, byte[] Source)
        {
            string tempStr = "";
            for (int i = beginAddr; i > (beginAddr - length); i--)
            {
                tempStr += Source[i].ToString("X2");//Convert.ToString(Source[i], 16).PadLeft(2, '0');
            }
            return Convert.ToInt64(tempStr, 16);
        }

        /// <summary>
        /// 高位在前 大端模式
        /// </summary>
        /// <param name="beginAddr">开始下标</param>
        /// <param name="length">长度</param>
        /// <param name="Source">数组</param>
        /// <returns></returns>
        public static long multiByteConvertLongHigh(int beginAddr, int length, byte[] Source)
        {
            string tempStr = "";
            for (int i = beginAddr; i < (beginAddr + length); i++)
            {
                tempStr += Source[i].ToString("X2");// Convert.ToString(Source[i], 16).PadLeft(2, '0');
            }
            return Convert.ToInt64(tempStr, 16);
        }

        /// <summary>
        /// 求权
        /// </summary>
        /// <param name="bs"> 进制基数</param>
        /// <param name="times">权级数</param>
        /// <returns></returns>
        private static int power(int bs, int times)
        {

            int i;

            int rslt = 1;

            for (i = 0; i < times; i++)

                rslt *= bs;

            return rslt;

        }
        public static int BCDtoDec(byte[] bt, int begin, int length)
        {
            int i, tmp, z = 0;
            int dec = 0;
            for (i = begin; i < begin + length; i++)
            {
                z++;
                tmp = ((bt[i] >> 4) & 0x0F) * 10 + (bt[i] & 0x0F);

                dec += tmp * power(100, length - z);

            }
            return dec;
        }

        public static byte[] DectoBCD(int Dec, int length)
        {
            byte[] Bcd = new byte[length];
            int i;
            int temp;
            for (i = length - 1; i >= 0; i--)
            {
                temp = Dec % 100;
                Bcd[i] = Convert.ToByte(((temp / 10) << 4) + ((temp % 10) & 0x0F));
                Dec /= 100;
            }
            return Bcd;
        }

        public static string BCD2String(byte[] bt, int begin, int length)
        {
            string temp = "";
            for (int i = begin; i < begin + length; i++)
            {
                temp += bt[i].ToString("X2");
            }
            return temp;
        }

        public static string ByteToASCII(byte[] bt, int begin, int length)
        {
            return Encoding.ASCII.GetString(bt, begin, length);
        }
        public static string ByteToGB2312(byte[] bt, int begin, int length)
        {
            return Encoding.GetEncoding("gb2312").GetString(bt, begin, length);
        }

        public static string ByteToUTF8(byte[] bt, int begin, int length)
        {
            return Encoding.UTF8.GetString(bt, begin, length);
        }
        public static string ByteToGBK(byte[] bt, int begin, int length)
        {
            return Encoding.GetEncoding("GBK").GetString(bt, begin, length).TrimEnd('\0');
        }
        public static byte[] GBK2Byte(string str)
        {
            return Encoding.GetEncoding("GBK").GetBytes(str);
        }

        public static string GBKStringToHexadecimalString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            byte[] bs = GBK2Byte(input);
            return ByteArrayToHexadecimalString(bs);
        }

        public static string ByteArrayToHexadecimalString(byte[] inputs)
        {
            if (null == inputs)
            {
                return null;
            }

            if (inputs.Length == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var b in inputs)
            {
                sb.Append(ByteToHexadecimalString(b));
            }
            return sb.ToString();
        }

        public static string ByteToHexadecimalString(byte input)
        {
            return input.ToString("X2");
        }
        public static byte[] int2bytes(int i)
        {
            byte[] b = new byte[4];

            b[0] = (byte)(0xff & i);
            b[1] = (byte)((0xff00 & i) >> 8);
            b[2] = (byte)((0xff0000 & i) >> 16);
            b[3] = (byte)((0xff000000 & i) >> 24);
            return b;
        }
        //public static int bytes2int(byte[] bytes)
        //{
        //    int num = bytes[0] & 0xFF;
        //    num |= ((bytes[1] << 8) & 0xFF00);
        //    num |= ((bytes[2] << 16) & 0xFF0000);
        //    num |= ((bytes[3] << 24) & 0xFF000000);
        //    return num;
        //}  
        public static long bytes2long(byte[] b)
        {
            long temp = 0;
            long res = 0;
            for (int i = 0; i < 8; i++)
            {
                res <<= 8;
                temp = b[i] & 0xff;
                res |= temp;
            }
            return res;
        }
        public static byte[] long2bytes(long num)
        {
            byte[] b = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                b[i] = (byte)(num >> (56 - (i * 8)));
            }
            return b;
        }

        public static byte[] short2bytes(short num)
        {
            byte[] temp = System.BitConverter.GetBytes(num);
            Array.Reverse(temp);// 反转 成大端模式
            return temp;
        }

        public static int bytes2short(byte[] bt)
        {
            Array.Reverse(bt);
            return BitConverter.ToInt16(bt, 0);

        }
        public static string getTime(byte[] bt, int begin, bool longTime = true)
        {

            string year = 20 + BCDtoDec(bt, begin, 1).ToString("00");
            string mouth = BCDtoDec(bt, begin + 1, 1).ToString("00");
            string day = BCDtoDec(bt, begin + 2, 1).ToString("00");
            if (longTime)
            {
                string hour = BCDtoDec(bt, begin + 3, 1).ToString("00");
                string minute = BCDtoDec(bt, begin + 4, 1).ToString("00");
                string second = BCDtoDec(bt, begin + 5, 1).ToString("00");
                return string.Format("{0}/{1}/{2} {3}:{4}:{5}", year, mouth, day, hour, minute, second);

            }
            return string.Format("{0}-{1}-{2}", year, mouth, day);
        }

        public static byte[] MergeBytes(byte[] bt0, byte[] bt1)
        {
            byte[] Destination = new byte[bt0.Length + bt1.Length];
            Array.Copy(bt0, 0, Destination, 0, bt0.Length);
            Array.Copy(bt1, 0, Destination, bt0.Length, bt1.Length);
            return Destination;
        }

        public static byte[] XOR(byte[] bt)
        {
            byte checksum = bt[1];
            for (int i = 2; i < bt.Length - 2; i++)//bt.count-1表示校验之前的位置
            {
                checksum ^= bt[i];
            }
            bt[bt.Length - 2] = checksum;
            return bt;
        }

        /// <summary>
        /// 转义
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static byte[] Escape(byte[] bt)
        {
            bt = XOR(bt);
            List<byte> list = new List<byte>();
            list.Add(0x7e);
            for (int i = 1; i < bt.Length - 1; i++)
            {
                switch (bt[i])
                {
                    case 0x7e:
                        list.Add(0x7d);
                        list.Add(0x02);
                        break;
                    case 0x7d:
                        list.Add(0x7d);
                        list.Add(0x01);
                        break;
                    default:
                        list.Add(bt[i]);
                        break;
                }
            }
            list.Add(0x7e);
            byte[] btEscape = new byte[list.Count];
            btEscape = list.ToArray();
            return btEscape;
        }

        /// <summary>
        /// 反转义
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static byte[] unEscape(byte[] bt)
        {
            List<byte> list = new List<byte>();
            for (int i = 1; i < bt.Length - 1; i++)
            {
                if (bt[i] == 0x7d)
                {
                    switch (bt[i + 1])
                    {
                        case 0x02:
                            list.Add(0x7e);
                            i++;
                            break;
                        case 0x01:
                            list.Add(0x7d);
                            i++;
                            break;
                        default:
                            list.Add(bt[i]);
                            break;
                    }
                }
                else
                    list.Add(bt[i]);
            }
            byte[] btEscape = new byte[list.Count];
            btEscape = list.ToArray();
            return btEscape;

        }

        public static string Byte2String(byte[] bt, int bengin, int length)
        {
            string temp = "";
            for (int i = bengin; i < bengin + length; i++)
            {
                temp += Convert.ToString(bt[i], 2).PadLeft(8, '0');
            }
            return temp;

        }

        public static int ByteToDec(byte[] bt, int begin, int length)
        {
            string str = "";
            for (int i = begin; i < begin + length; i++)
            {
                str += bt[i].ToString("X2");
            }
            return Convert.ToInt32(str, 16);
        }

        /// <summary>
        /// int转byte数组 高位在前 大端模式
        /// </summary>
        /// <param name="i"></param>
        /// <returns>byte[]</returns>
        public static byte[] IntToBytes(int i)
        {
            byte[] b = new byte[4];

            b[3] = (byte)(0xff & i);
            b[2] = (byte)((0xff00 & i) >> 8);
            b[1] = (byte)((0xff0000 & i) >> 16);
            b[0] = (byte)((0xff000000 & i) >> 24);
            return b;
        }


        #region 字节数组转16进制字符串
        /// <summary>  
        /// Byte[] to HEX  字节数组转16进制字符串
        /// </summary>  
        /// <param name="hex"></param>  
        /// <returns></returns>
        public static string ByteToHex(byte[] bt, int begin, int length)
        {
            string str = "";
            for (int i = begin; i < begin + length; i++)
            {
                str += bt[i].ToString("X2");
            }
            return str;
        }
        #endregion

        /// <summary>
        /// int转一个字节的十六进制字符串（只取最低位）
        /// </summary>
        /// <param name="i"></param>
        /// <returns>hexstring</returns>
        public static string IntToOneHexString(int i)
        {
            string hexString = "";
            hexString = ((byte)(0xff & i)).ToString("X2");
            return hexString;
        }


        #region 16进制字符串转换成字节数组
        /// <summary>  
        /// 16进制字符串转换成字节数组   eg:AB 00 CE 0A ...
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }
        #endregion

        #region 各进制数间转换
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
        #endregion

    }
}
