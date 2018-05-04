using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.PartManage
{
    [Description("零件")]
    public class T_Part : BaseEntity<T_Part>
    {
        [PropertyCN("编码")]
        //编码
        public string F_Code { get; set; }

        [PropertyCN("名称")]
        //名称
        public string F_Name { get; set; }

        [PropertyCN("规格")]
        //规格
        public string F_Spec { get; set; }

        [PropertyCN("单位")]
        //单位
        public string F_Unit { get; set; }

        [PropertyCN("创建时间")]
        //创建时间
        public DateTime? F_Create_Time { get; set; }
    }
}
