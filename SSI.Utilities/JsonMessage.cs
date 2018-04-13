using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    public class JsonMessage
    {
        public override string ToString()
        {
            return this.ToJson();
        }
        public string state { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
