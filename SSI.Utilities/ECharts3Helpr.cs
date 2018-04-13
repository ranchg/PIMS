using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    /// <summary>
    /// EChartt3.0 数据构造器
    /// </summary>
    public class ECharts3Helpr
    {
        public ECharts3Helpr() { }

        /// <summary>
        /// 图形标题
        /// </summary>
        public string TitleText { get; set; }
        /// <summary>
        /// 图形附属标题
        /// </summary>
        public string TitleSubText { get; set; }
        /// <summary>
        /// 图形附属提示的数据内容
        /// </summary>
        public string LegendData { get; set; }
        /// <summary>
        /// 地图属性配置
        /// </summary>
        public string VisualMapText { get; set; }
        public List<Series> LstSeries { get; set; }       
    }
    /// <summary>
    /// 显示区域类
    /// </summary>
    public class Series
    {
        /// <summary>
        /// 地图上显示区域系列数据名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 地图上显示区域系列数据类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 地图上显示区域系列数据地图区域类型
        /// </summary>
        public string mapType { get; set; }

        public object label { get; set; }


        /// <summary>
        /// 地图上显示区域系列数据数据内容
        /// </summary>
        public List<Data> data { get; set; }
    }

    public class Data {
        public string name { get; set; }
        public int value { get; set; }
    }
}
