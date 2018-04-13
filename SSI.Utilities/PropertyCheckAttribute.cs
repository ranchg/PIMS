using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyCheckAttribute : Attribute
    {
        /// <summary>
        ///  校验的类型
        /// </summary>
        private ProertyCheckType checkType;

        private string UnderField;

        public PropertyCheckAttribute()
        {
        }
        public PropertyCheckAttribute(ProertyCheckType type)
        {
            this.checkType = type;
        }
        public PropertyCheckAttribute(ProertyCheckType type, string field)
        {
            this.checkType = type;
            this.UnderField = field;
        }
        public virtual ProertyCheckType CheckType
        {
            get
            {
                return this.checkType;
            }
            set
            {
                this.checkType = value;
            }
        }

        public virtual string UField
        {
            get
            {
                return this.UnderField;
            }
            set
            {
                this.UnderField = value;
            }
        }
    }

    public enum ProertyCheckType
    {
        /// <summary>
        ///  不为空
        /// </summary>
        NotNull,
        /// <summary>
        ///  不为空且唯一
        /// </summary>
        NotNullAndUnique,
        /// <summary>
        ///  是否在表中已经存在
        /// </summary>
        IsExsist,
        /// <summary>
        ///  必须是数字
        /// </summary>
        IsNumber,

        /// <summary>
        ///  为空或者是数字
        /// </summary>
        IsNullOrNumber
    }

    public class CheckType
    {
        public CheckType(ProertyCheckType enumType, string underField)
        {
            this.enumType = enumType;
            this.underField = underField;
        }
        public ProertyCheckType enumType { get; set; }

        public string underField { get; set; }
    }
}
