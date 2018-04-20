using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.SystemManage
{
    [Description("操作")]
    [PrimaryKey("F_Id")]
    public class T_Action : BaseEntity<T_Action>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("菜单ID")]
        //菜单ID
        public int F_Menu_Id { get; set; }

        [PropertyCN("编码")]
        //编码
        public string F_Code { get; set; }

        [PropertyCN("名称")]
        //名称
        public string F_Name { get; set; }

        [PropertyCN("目标")]
        //目标
        public string F_Target { get; set; }

        [PropertyCN("类型标志")]
        //类型标志(1菜单,2列表,3按钮)
        public int F_Type_Mark { get; set; }

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

        public override T_Action Create()
        {
            base.Create();
            F_Id = DataFactory.Database().FindCountBySql("SELECT ISNULL(MAX(F_ID), 0) + 1 FROM T_ACTION");
            return this;
        }
    }
}
