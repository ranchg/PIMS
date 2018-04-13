using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.SystemManage
{
    [Description("菜单")]
    [PrimaryKey("F_Id")]
    public class T_Menu : BaseEntity<T_Menu>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("父级ID")]
        //父级ID
        public int F_Parent_Id { get; set; }

        [PropertyCN("名称")]
        //名称
        public string F_Name { get; set; }

        [PropertyCN("目标")]
        //目标
        public string F_Target { get; set; }

        [PropertyCN("图标")]
        //图标
        public string F_Icon { get; set; }

        [PropertyCN("排序")]
        //排序
        public int F_Sort { get; set; }

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

        public override T_Menu Create()
        {
            base.Create();
            F_Id = DataFactory.Database().FindCountBySql("SELECT S_MENU.NEXTVAL FROM DUAL");
            return this;
        }
    }
}
