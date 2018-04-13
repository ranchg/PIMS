using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    /// <summary>
    ///  查询条件
    /// </summary>
    public class Condition
    {
        public string ParamType { get; set; }
        public string ParamName { get; set; }

        public object ParamValue { get; set; }

        public ConditionOperate Operation { get; set; }
    }

    public enum ConditionOperate : byte
    {
        AfterDay = 0x16,
        BeforeDay = 0x15,
        Equal = 0,
        Greater = 2,
        GreaterThan = 3,
        LastMonth = 0x12,
        LastWeek = 15,
        LeftLike = 10,
        Less = 4,
        LessThan = 5,
        Like = 8,
        NextMonth = 20,
        NextWeek = 0x11,
        NotEqual = 1,
        NotLike = 9,
        NotNull = 7,
        Null = 6,
        RightLike = 11,
        ThisMonth = 0x13,
        ThisWeek = 0x10,
        Today = 13,
        Tomorrow = 14,
        Yesterday = 12
    }

    public class Column
    {
        public string ParamTitle { get; set; }

        public string ParamField { get; set; }

        public override string ToString()
        {
            return string.Format("{0} as \"{1}\"", this.ParamField, this.ParamTitle);
        }
    }
}
