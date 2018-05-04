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
    [Description("维护")]
    [PrimaryKey("F_Id")]
    public class T_Maintenance : BaseEntity<T_Maintenance>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //编码
        public string F_Id { get; set; }

        [PropertyCN("设备ID")]
        //编码
        public string F_Equipment_Id { get; set; }

        [PropertyCN("项目")]
        //名称
        public string F_Name { get; set; }

        [PropertyCN("费用")]
        //规格
        public decimal? F_Cost { get; set; }

        [PropertyCN("操作人")]
        //创建人
        public string F_Oper_By { get; set; }

        [PropertyCN("操作时间")]
        //创建时间
        public DateTime? F_Oper_Time { get; set; }

        [PropertyCN("备注")]
        //创建人
        public string F_Remark { get; set; }

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
