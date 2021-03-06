﻿using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SSI.Business.SystemManage
{
    public class RoleActionBLL : RepositoryFactory<T_Role_Action>
    {
        //根据角色ID删除 By 阮创 2017/11/30
        public int DeleteByRoleId(string roleId)
        {
            string sql = string.Format(
            @"UPDATE T_ROLE_ACTION SET F_DELETE_MARK = 1 WHERE F_ROLE_ID = {0}", roleId);
            return Repository().ExecuteBySql(new System.Text.StringBuilder(sql));
        }

        //根据操作ID删除 By 阮创 2017/11/30
        public int DeleteByActionId(string actionId)
        {
            string sql = string.Format(
            @"UPDATE T_ROLE_ACTION SET F_DELETE_MARK = 1 WHERE F_ACTION_ID = {0}", actionId);
            return Repository().ExecuteBySql(new System.Text.StringBuilder(sql));
        }
    }
}
