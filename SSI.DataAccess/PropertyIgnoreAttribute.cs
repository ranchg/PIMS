using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSI.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyIgnoreAttribute : Attribute
    {
        /// <summary>
        ///  校验的类型
        /// </summary>
        private ProertyIgnore checkType;

        public PropertyIgnoreAttribute()
        {

        }

        public PropertyIgnoreAttribute(ProertyIgnore type)
        {
            this.checkType = type;
        }
        public virtual ProertyIgnore CheckType
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
    }

    public enum ProertyIgnore
    {
        Ignore
    }
}
