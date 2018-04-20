using SSI.DataAccess;
using SSI.DataAccess.Attributes;
using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.ComponentModel;

namespace SSI.Entity.SystemManage
{
    [Description("用户")]
    [PrimaryKey("F_Id")]
    public class T_User : BaseEntity<T_User>, ICreateAction, IModifyAction, IDeleteAction
    {
        [PropertyCN("主键ID")]
        //主键ID
        public int F_Id { get; set; }

        [PropertyCN("账号")]
        //账号
        public string F_Account { get; set; }

        [PropertyCN("密码")]
        //密码
        public string F_Password { get; set; }

        [PropertyCN("姓名")]
        //姓名
        public string F_Real_Name { get; set; }

        [PropertyCN("呢称")]
        //呢称
        public string F_Nice_Name { get; set; }

        [PropertyCN("性别")]
        //性别
        //性别(1男,2女)
        public string F_Gender { get; set; }

        [PropertyCN("生日")]
        //生日
        public DateTime? F_Birthday { get; set; }

        [PropertyCN("手机")]
        //手机
        public string F_Phone { get; set; }

        [PropertyCN("邮箱")]
        //邮箱
        public string F_Mail { get; set; }

        [PropertyCN("QQ")]
        //QQ
        public string F_QQ { get; set; }

        [PropertyCN("微信")]
        //微信
        public string F_Wechat { get; set; }

        [PropertyCN("头像")]
        //头像
        public string F_Head_Icon { get; set; }

        [PropertyCN("头像地址")]
        [PropertyIgnore]
        //头像
        public string F_Head_Icon_Path
        {
            get
            {
                return string.IsNullOrEmpty(F_Head_Icon) ? null : UploadHelper.GetAbsUrl(F_Head_Icon);
            }
        }

        [PropertyCN("在线标志")]
        //在线标志(1在线,0离线)
        public int F_Online_Mark { get; set; }

        [PropertyCN("系统标识")]
        //系统标识(1系统,0其它)
        public int F_System_Mark { get; set; }

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

        public override T_User Create()
        {
            string whereSql = string.Format("AND F_DELETE_MARK = 0 AND UPPER(F_ACCOUNT) = UPPER('{0}')", F_Account);
            if (DataFactory.Database().FindCount<T_User>(whereSql) == 0)
            {
                base.Create();
                F_Id = DataFactory.Database().FindCountBySql("SELECT ISNULL(MAX(F_ID), 0) + 1 FROM T_USER");
                F_Password = Md5Helper.MD5("123456", 0x20).ToUpper();
                return this;
            }
            else
            {
                throw new Exception("账号已存在");
            }
        }
    }
}
