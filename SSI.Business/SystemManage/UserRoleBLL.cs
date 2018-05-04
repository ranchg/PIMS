using SSI.Entity.SystemManage;
using SSI.Repository;

namespace SSI.Business.SystemManage
{
    public class UserRoleBLL : RepositoryFactory<T_User_Role>
    {
        //根据用户ID删除 By 阮创 2017/11/30
        public int DeleteByUserId(string userId)
        {
            string sql = string.Format(
            @"UPDATE T_USER_ROLE SET F_DELETE_MARK = 1 WHERE F_USER_ID = {0}", userId);
            return Repository().ExecuteBySql(new System.Text.StringBuilder(sql));
        }

        //根据角色ID删除 By 阮创 2017/11/30
        public int DeleteByRoleId(string roleId)
        {
            string sql = string.Format(
            @"UPDATE T_USER_ROLE SET F_DELETE_MARK = 1 WHERE F_ROLE_ID = {0}", roleId);
            return Repository().ExecuteBySql(new System.Text.StringBuilder(sql));
        }
    }
}
