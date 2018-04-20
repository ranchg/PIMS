using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.SystemManage
{
    [Description("用户角色关系表")]
    [PrimaryKey("F_Id")]
    public class T_User_Role : BaseEntity<T_User_Role>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("用户ID")]
        //账号
        public int F_User_Id { get; set; }

        [PropertyCN("角色ID")]
        //密码
        public int F_Role_Id { get; set; }

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

        public override T_User_Role Create()
        {
            base.Create();
            F_Id = DataFactory.Database().FindCountBySql("SELECT ISNULL(MAX(F_ID), 0) + 1 FROM T_USER_ROLE");
            return this;
        }
    }
}
