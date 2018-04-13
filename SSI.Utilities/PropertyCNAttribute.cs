using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    /// <summary>
    ///  属性中文描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyCNAttribute : Attribute
    {
        private string _name;

        public PropertyCNAttribute()
        {
        }

        public PropertyCNAttribute(string name)
        {
            this._name = name;
        }

        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }
    }
}
