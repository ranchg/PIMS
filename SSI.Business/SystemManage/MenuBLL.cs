﻿using SSI.Entity.SystemManage;
using SSI.Repository;
using System.Collections.Generic;

namespace SSI.Business.SystemManage
{
    public class MenuBLL : RepositoryFactory<T_Menu>
    {
        //获取列表 By 阮创 2017/11/30
        public List<T_Menu> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_MENU T1
             WHERE T1.F_DELETE_MARK = 0
             ORDER BY T1.F_PARENT_ID ASC, T1.F_SORT ASC";
            return Repository().FindListBySql(sql);
        }

        //根据用户ID获取列表 By 阮创 2017/11/30
        public List<T_Menu> GetListByUserId(int userId)
        {
            string sql = string.Format(
            @"SELECT T1.*
              FROM T_MENU T1
              JOIN T_ACTION T2
                ON T2.F_MENU_ID = T1.F_ID
              JOIN T_ROLE_ACTION T3
                ON T3.F_ACTION_ID = T2.F_ID
              JOIN T_USER_ROLE T4
                ON T4.F_ROLE_ID = T3.F_ROLE_ID
             WHERE T1.F_DELETE_MARK = 0
               AND T2.F_DELETE_MARK = 0
               AND T2.F_TYPE_MARK = 1
               AND T3.F_DELETE_MARK = 0
               AND T4.F_DELETE_MARK = 0
               AND T4.F_USER_ID = {0}
             ORDER BY T1.F_PARENT_ID ASC, T1.F_SORT ASC", userId);
            return Repository().FindListBySql(sql);
        }

        //根据用户ID获取树型列表 By 阮创 2017/11/30
        public List<T_Menu> GetTreeByUserId(int userId)
        {
            string sql = string.Format(
            @"SELECT DISTINCT T1.*
              FROM T_MENU T1
             START WITH T1.F_ID IN (SELECT T1.F_ID
                                      FROM T_MENU T1
                                      JOIN T_ACTION T2
                                        ON T2.F_MENU_ID = T1.F_ID
                                      JOIN T_ROLE_ACTION T3
                                        ON T3.F_ACTION_ID = T2.F_ID
                                      JOIN T_USER_ROLE T4
                                        ON T4.F_ROLE_ID = T3.F_ROLE_ID
                                     WHERE T1.F_DELETE_MARK = 0
                                       AND T2.F_DELETE_MARK = 0
                                       AND T2.F_TYPE_MARK = 1
                                       AND T3.F_DELETE_MARK = 0
                                       AND T4.F_DELETE_MARK = 0
                                       AND T4.F_USER_ID = {0})
            CONNECT BY T1.F_ID = PRIOR T1.F_PARENT_ID
             ORDER BY T1.F_PARENT_ID ASC, T1.F_SORT ASC", userId);
            return Repository().FindListBySql(sql);
        }
    }
}
