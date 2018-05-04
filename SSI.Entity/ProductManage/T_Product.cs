using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.ProductManage
{
    [Description("操作")]
    [PrimaryKey("F_Id")]
    public class T_Product : BaseEntity<T_Product>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public string F_Id { get; set; }

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

        [PropertyCN("有效标志")]
        //有效标志(1有效,0无效)
        public int F_Enable_Mark { get; set; }

        [PropertyCN("删除标志")]
        //删除标志(1已删,0未删)
        public int F_Delete_Mark { get; set; }

        [PropertyCN("创建人")]
        //创建人
        public string F_Create_By { get; set; }

        [PropertyCN("创建时间")]
        //创建时间
        public DateTime? F_Create_Time { get; set; }
    }
}
