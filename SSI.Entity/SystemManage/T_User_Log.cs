using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.SystemManage
{
    [Description("用户日志")]
    [PrimaryKey("F_Id")]
    public class T_User_Log : BaseEntity<T_User_Log>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("用户ID")]
        //用户ID
        public int F_User_Id { get; set; }

        [PropertyCN("账号")]
        //账号
        public string F_Account { get; set; }

        [PropertyCN("IP地址")]
        //IP地址
        public string F_IPAddress { get; set; }

        [PropertyCN("菜单")]
        //菜单
        public string F_Menu { get; set; }

        [PropertyCN("操作")]
        //操作
        public string F_Action { get; set; }

        [PropertyCN("结果标志")]
        //结果标志(1成功,2失败,3异常)
        public int F_Result_Mark { get; set; }

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

        public override T_User_Log Create()
        {
            base.Create();
            F_Id = DataFactory.Database().FindCountBySql("SELECT ISNULL(MAX(F_ID), 0) + 1 FROM T_USER_LOG");
            return this;
        }
    }
}
