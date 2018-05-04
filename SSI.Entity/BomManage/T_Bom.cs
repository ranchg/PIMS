using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SSI.Entity.BomManage
{
    [Description("BOM")]
    [PrimaryKey("F_Id")]
    public class T_Bom : BaseEntity<T_Bom>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public string F_Id { get; set; }

        [PropertyCN("产品ID")]
        //菜单ID
        public string F_Product_Id { get; set; }

        [PropertyCN("BOM编码")]
        //编码
        public string F_Code { get; set; }

        [PropertyCN("BOM名称")]
        //编码
        public string F_Name { get; set; }

        [PropertyCN("产品版本")]
        //名称
        public string F_Version { get; set; }

        [PropertyCN("目标")]
        //目标
        public DateTime? F_Date { get; set; }

        [PropertyCN("有效标志")]
        //有效标志(1有效,0无效)
        public int F_Enable_Mark { get; set; }=1;

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
