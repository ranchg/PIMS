using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SSI.Utilities
{
    /// <summary>
    /// 设置报表显示的时间样式
    /// </summary>
    public class JsonDateFormat : JsonConverter
    {
        public  IsoDateTimeConverter dtConverter = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }
    public class JsonFullTimeFormat : JsonDateFormat
    {
        public JsonFullTimeFormat()
        {
            dtConverter = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
        }
        
    }
    public class JsonFullDateFormat : JsonDateFormat
    {
        public JsonFullDateFormat()
        {
            dtConverter = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" };
        }
    }
    public class JsonChinaFullTimeFormat : JsonDateFormat
    {
        public JsonChinaFullTimeFormat()
        {
            dtConverter = new IsoDateTimeConverter() { DateTimeFormat = "yyyy年MM月dd日 HH时mm分ss秒" };
        }        
    }
    public class JsonChinaFullDateFormat : JsonDateFormat
    {
        public JsonChinaFullDateFormat()
        {
            dtConverter = new IsoDateTimeConverter() { DateTimeFormat = "yyyy年MM月dd日" };
        }    
         
    }
}
