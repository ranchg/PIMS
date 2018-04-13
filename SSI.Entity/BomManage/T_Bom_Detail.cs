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
    [Description("BOMDetail")]
    [PrimaryKey("F_Id")]
    public class T_Bom_Detail : BaseEntity<T_Bom_Detail>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("BOMID")]
        //菜单ID
        public int F_Bom_Id { get; set; }

        [PropertyCN("零件ID")]
        //编码
        public int F_Part_Id { get; set; }

        [PropertyCN("零件数量")]
        //名称
        public int F_Num { get; set; }

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

        public override T_Bom_Detail Create()
        {
            base.Create();
            F_Id = DataFactory.Database().FindCountBySql("SELECT S_BOM_DETAIL.NEXTVAL FROM DUAL");
            return this;
        }
    }
}
