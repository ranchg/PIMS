using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.PartManage
{
    [Description("零件采购")]
    [PrimaryKey("F_Id")]
    public class T_Part_Buy : BaseEntity<T_Part_Buy>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public string F_Id { get; set; }

        [PropertyCN("零件编码")]
        //零件编码
        public string F_Part_Code { get; set; }

        [PropertyCN("数量")]
        //数量
        public int F_Quantity { get; set; }

        [PropertyCN("预计到货日期")]
        //预计到货日期
        public DateTime? F_Eta { get; set; }

        [PropertyCN("实际到货日期")]
        //实际到货日期
        public DateTime? F_Ata { get; set; }

        [PropertyCN("备注")]
        //备注
        public string F_Remark { get; set; }

        [PropertyCN("有效标志")]
        //有效标志(1有效,0无效)
        public int F_Enable_Mark { get; set; } = 1;

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
