using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.SystemManage
{
    [Description("角色操作关系表")]
    [PrimaryKey("F_Id")]
    public class T_Role_Action : BaseEntity<T_Role_Action>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("角色ID")]
        //账号
        public int F_Role_Id { get; set; }

        [PropertyCN("操作ID")]
        //密码
        public int F_Action_Id { get; set; }

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

        public override T_Role_Action Create()
        {
            base.Create();
            F_Id = DataFactory.Database().FindCountBySql("SELECT ISNULL(MAX(F_ID), 0) + 1 FROM T_ROLE_ACTION");
            return this;
        }
    }
}
