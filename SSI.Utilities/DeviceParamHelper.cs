using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    public class DeviceParamHelper
    {
        public const byte PT_STR = 1;
        public const byte PT_UINT8 = 2;
        public const byte PT_UINT16 = 3;
        public const byte PT_UINT32 = 4;
        public const byte PT_BYTES = 5; //字节数组，用16进制文本表示
        public static string GetHexString(byte paramType,uint cmdId,string cmdParam)
        {
            string hexStr = "";
            byte[] byteArr;
            byte[] lengthArr;
            byte[] cmdIdArr = DataConversion.long2bytes(Convert.ToInt64(cmdId));//参数id
            hexStr += DataConversion.ByteToHex(cmdIdArr, 4, 4);//添加参数id

            switch (paramType)
            {
                case DeviceParamHelper.PT_UINT8:
                    lengthArr = DataConversion.short2bytes(1);
                    hexStr += DataConversion.ByteToHex(lengthArr, 1, 1);

                    byteArr = DataConversion.short2bytes(Convert.ToInt16(cmdParam));
                    hexStr += DataConversion.ByteToHex(byteArr, 1, 1);
                    break;
                case DeviceParamHelper.PT_UINT16:
                    lengthArr = DataConversion.short2bytes(2);
                    hexStr += DataConversion.ByteToHex(lengthArr, 1, 1);

                     byteArr = DataConversion.IntToBytes(Convert.ToInt32(cmdParam));//参数id
                     hexStr += DataConversion.ByteToHex(byteArr, 2, 2);
                    break;
                case DeviceParamHelper.PT_UINT32:
                    lengthArr = DataConversion.short2bytes(4);
                    hexStr += DataConversion.ByteToHex(lengthArr, 1, 1);

                    byteArr = DataConversion.long2bytes(Convert.ToInt64(cmdParam));//参数id
                    hexStr += DataConversion.ByteToHex(byteArr, 4, 4);
                    break;
                case DeviceParamHelper.PT_STR:
                   
                    byteArr = DataConversion.GBK2Byte(cmdParam.Trim());
                    lengthArr = DataConversion.short2bytes((short)byteArr.Length);
                    hexStr += DataConversion.ByteToHex(lengthArr, 1, 1);
                    hexStr += DataConversion.ByteToHex(byteArr, 0, byteArr.Length);
                    break;
                //case DeviceParamHelper.PT_BYTES://字节数组不处理
                //    hexStr += DataConversion.ByteToHex(byteArr, 0, byteArr.Length);
                //    break;
            }
            return hexStr;
        }
    }
}
