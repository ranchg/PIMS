using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SSI.Entity.EquipmentManage
{
    [Description("零件")]
    [PrimaryKey("F_Id")]
    public class T_Equipment : BaseEntity<T_Equipment>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //编码
        public string F_Id { get; set; }

        [PropertyCN("设备编码")]
        //编码
        public string F_Code { get; set; }

        [PropertyCN("名称")]
        //名称
        public string F_Name { get; set; }

        [PropertyCN("资产编码")]
        //规格
        public string F_Asset_Code { get; set; }

        [PropertyCN("规格型号")]
        //单位
        public string F_Spec { get; set; }

        [PropertyCN("部门")]
        //单位
        public string F_Org { get; set; }

        [PropertyCN("厂家")]
        //单位
        public string F_Vender { get; set; }

        [PropertyCN("位置")]
        //单位
        public string F_Place { get; set; }

        [PropertyCN("周期")]
        //单位
        public int? F_Period { get; set; }

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
