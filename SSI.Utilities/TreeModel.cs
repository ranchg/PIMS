using System.Collections.Generic;

/**  来自 tree.js 中的描述 by fangjq
    * @description {Config} data  
    * {Object} Tree theme. Three themes provided. 'bbit-tree-lines' ,'bbit-tree-no-lines' and 'bbit-tree-arrows'.
    * @sample 
    * data:[{
    * id:"node1", //node id
    * text:"node 1", //node text for display.
    * value:"1", //node value
    * showcheck:false, //whether to show checkbox
    * checkstate:0, //Checkbox checking state. 0 for unchecked, 1 for partial checked, 2 for checked.
    * hasChildren:true, //If hasChildren and complete set to true, and ChildNodes is empty, tree will request server to get sub node.
    * isexpand:false, //Expand or collapse.
    * complete:false, //See hasChildren.
    * ChildNodes:[] // child nodes
    * }]                  
    *  */

namespace SSI.Utilities
{
    public class TreeModel
    {

        public TreeModel()
        {
            this.hasChildren = false;
            this.complete = false;
            this.ChildNodes = new List<TreeModel>();
            this.checkstate = 0;
            this.checkIgnore = false;
            this.clickIgnore = false;
        }

        /// <summary>
        ///  结点ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        ///  结点显示的文本
        /// </summary>
        public string text { get; set; }
        /// <summary>
        ///  结点的值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        ///  结点图片
        /// </summary>
        public string img { get; set; }
        /// <summary>
        ///  是否显示checkbox（默认不显示）
        /// </summary>
        public bool showcheck 
        {
            get;
            set;
        }

        public int checkstate
        {
            get;
            set;
        }
        /// <summary>
        ///  折叠或者打开（默认打开)
        /// </summary>
        public bool isexpand
        {
            get;
            set;
        }

        /// <summary>
        ///  查看是否有子结点
        /// </summary>
        public bool complete { get; set; }
        /// <summary>
        ///  是否有子结点
        /// </summary>
        public bool hasChildren { get; set; }
        /// <summary>
        ///  子结点
        /// </summary>
        public List<TreeModel> ChildNodes { get; set; }
        /// <summary>
        ///  点击结点时是否有效
        /// </summary>
        public bool clickIgnore { get; set; }
        /// <summary>
        ///  点击checkbox时是否有效
        /// </summary>
        public bool checkIgnore { get; set; }

    }

    public class BootstrapTree
    {
        public string nodeId { get; set; }
        public string text { get; set; }

        public List<BootstrapTree> nodes { get; set; }
    }
}
