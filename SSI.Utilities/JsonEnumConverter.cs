using Newtonsoft.Json;
using System;

namespace SSI.Utilities
{
    /// <summary>
    /// 报表中枚举类型转换对应描述
    /// </summary>
    /// <typeparam name="T">对应的枚举</typeparam>
    public class JsonEnumConverter<T> : JsonConverter where T : struct, IConvertible
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteValue("");
                return;
            }
            string str = "";
            int intVal;
            if (int.TryParse(value.ToString(), out intVal))
            {
                str = GetEnumName(typeof(T), intVal);
            }

            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }

            writer.WriteValue(str);
        }

        /// <summary>
        /// 获取对应值的描述
        /// </summary>
        /// <param name="type">枚举类</param>
        /// <param name="value">枚举值</param>
        /// <returns>枚举名称</returns>
        private string GetEnumName(Type type, int value)
        {
            try
            {
                string name = Enum.GetName(type, value);
                if (name == null)
                {
                    name = Enum.GetName(type, 10000);//如果未找到寻找默认值
                }
                return name;
            }
            catch
            {
                return "";
            }
        }
    }

    #region 枚举：描述=int值
    /// <summary>
    /// SIM卡状态
    /// </summary>
    public enum SimStatus
    {
        未销号 = 0,
        已销号 = 1
    }
    public enum JudgeStatus
    {
        是 = 1,
        否 = 0
    }
    #endregion
}
