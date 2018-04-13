using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    public class GridParam
    {
        public string query { get; set; } //查询条件
        public string search { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
    }
}
