using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSI.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class PrimaryKeyAttribute : Attribute
    {
        private string _name;

        public PrimaryKeyAttribute()
        {
        }

        public PrimaryKeyAttribute(string name)
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
