using SSI.Entity.PartManage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.PartManage
{
    public class PartCheckTimeBLL : RepositoryFactory<T_Part_Check_Time>
    {
        public List<T_Part_Check_Time> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_PART_CHECK_TIME T1
             WHERE T1.F_DELETE_MARK = 0
             ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }
    }
}
