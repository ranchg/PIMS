using SSI.Entity.SystemManage;
using SSI.Repository;
using System.Collections.Generic;

namespace SSI.Business.SystemManage
{
    public class OrgBLL : RepositoryFactory<T_Org>
    {
        //获取列表 By 阮创 2017/11/30
        public List<T_Org> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_ORG T1
             WHERE T1.F_DELETE_MARK = 0
             ORDER BY T1.F_PARENT_ID ASC, T1.F_SORT ASC";
            return Repository().FindListBySql(sql);
        }

        //根据用户ID获取列表 By 阮创 2017/11/30
        public List<T_Org> GetListByUserId(string userId)
        {
            string sql = string.Format(
            @"SELECT T1.*
              FROM T_ORG T1
              JOIN T_ROLE T2
                ON T2.F_ORG_ID = T1.F_ID
              JOIN T_USER_ROLE T3
                ON T3.F_ROLE_ID = T2.F_ID
             WHERE T1.F_DELETE_MARK = 0
               AND T2.F_DELETE_MARK = 0
               AND T3.F_DELETE_MARK = 0
               AND T3.F_USER_ID = {0}
             ORDER BY T1.F_PARENT_ID ASC, T1.F_SORT ASC", userId);
            return Repository().FindListBySql(sql);
        }

        //根据用户ID获取树型列表 By 阮创 2017/11/30
        public List<T_Org> GetTreeListByUserId(string userId)
        {
            //string sql = string.Format(
            //@"SELECT DISTINCT T1.*
            //  FROM T_ORG T1
            // START WITH T1.F_ID IN (SELECT T1.F_ID
            //                          FROM T_ORG T1
            //                          JOIN T_ROLE T2
            //                            ON T2.F_ORG_ID = T1.F_ID
            //                          JOIN T_USER_ROLE T3
            //                            ON T3.F_ROLE_ID = T2.F_ID
            //                         WHERE T1.F_DELETE_MARK = 0
            //                           AND T2.F_DELETE_MARK = 0
            //                           AND T3.F_DELETE_MARK = 0
            //                           AND T3.F_USER_ID = {0})
            //CONNECT BY T1.F_ID = PRIOR T1.F_PARENT_ID
            // ORDER BY T1.F_PARENT_ID ASC, T1.F_CREATE_TIME ASC", userId);
            string sql = string.Format(
            @"WITH W1 AS (
	            SELECT
		            T1.*
	            FROM
		            T_ORG T1
	            LEFT JOIN T_ROLE T2 ON T2.F_ORG_ID = T1.F_ID
	            LEFT JOIN T_USER_ROLE T3 ON T3.F_ROLE_ID = T2.F_ID
	            WHERE
		            T1.F_DELETE_MARK = 0
	            AND T2.F_DELETE_MARK = 0
	            AND T3.F_DELETE_MARK = 0
	            AND T3.F_USER_ID = {0}
	            UNION ALL
		            SELECT
			            U1.*
		            FROM
			            T_ORG U1
		            INNER JOIN W1 U2 ON U2.F_PARENT_ID = U1.F_ID
            ) SELECT DISTINCT
	            *
            FROM
	            W1
            ORDER BY
	            F_PARENT_ID ASC,
	            F_CREATE_TIME ASC", userId);
            return Repository().FindListBySql(sql);
        }
    }
}
