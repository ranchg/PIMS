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
    public class T_Product_Make : BaseEntity<T_Product_Make>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("产品ID")]
        //产品ID
        public int F_Product_Id { get; set; }

        [PropertyCN("数量")]
        //数量
        public int F_Quantity { get; set; }

        [PropertyCN("生产日期")]
        //生产日期
        public DateTime? F_Make_Date { get; set; }

        [PropertyCN("是否统计")]
        //是否统计
        public int F_Is_Read { get; set; }

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

        public override T_Product_Make Create()
        {
            base.Create();
            F_Id = DataFactory.Database().FindCountBySql("SELECT S_PRODUCT_MAKE.NEXTVAL FROM DUAL");
            F_Is_Read = 0;
            F_Enable_Mark = 1;
            return this;
        }
    }
}
