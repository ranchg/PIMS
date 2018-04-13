using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.PartManage
{
    [Description("零件库存")]
    [PrimaryKey("F_Id")]
    public class T_Part_Stock : BaseEntity<T_Part_Stock>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("零件ID")]
        //零件ID
        public int F_Part_Id { get; set; }

        [PropertyCN("统计周期ID")]
        //统计周期ID
        public int F_Stat_Period_Id { get; set; }

        [PropertyCN("ERP库存数量")]
        //ERP库存数量
        public int F_Erp_Stock_Quantity { get; set; }

        [PropertyCN("生产消耗数量")]
        //生产消耗数量
        public int F_Make_Consume_Quantity { get; set; }

        [PropertyCN("采购在途数量")]
        //采购在途数量
        public int F_Buy_Onway_Quantity { get; set; }

        [PropertyCN("采购到货数量")]
        //采购到货数量
        public int F_Buy_Reach_Quantity { get; set; }

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

        public override T_Part_Stock Create()
        {
            base.Create();
            F_Id = DataFactory.Database().FindCountBySql("SELECT S_PART_STOCK.NEXTVAL FROM DUAL");
            return this;
        }
    }
}
