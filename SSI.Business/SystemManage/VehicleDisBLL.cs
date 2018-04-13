using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSI.Business.SystemManage
{
    public class VehicleDisBLL : RepositoryFactory<VehicleDis>
    {
        public Series GetMapDis()
        {
            string strSeriesName = string.Empty;
            DataTable dt = this.Repository().FindTableBySql(string.Format(@"select count(*) as vehicle_count ,province from
                           (select v.*,t.province from t_vehicle v
                            inner join (
                            select a.*,c.province from 
                            (select t.*,p.province from 
                            (select  f_code,case when substr(f_name,0,2)='内蒙'  then '内蒙古'  when substr(f_name,0,2)='黑龙' then '黑龙江' else to_char(substr(f_name,0,2)) end as province  from t_area where f_partent_code  is null) p
                            inner join t_area t on p.f_code=t.f_partent_code ) c
                            inner join t_area a on c.f_code=a.f_partent_code
                             ) t  on t.f_id=v.f_area_id
                            ) group by province"));//获取各省份车辆分布数量
            strSeriesName = "车辆总数";//暂时不分用户权限,加载所有车辆
            Series s = new Series();
            s.name = strSeriesName;
            s.type = "map";
            s.mapType = "china";
            s.label = new
            {
                normal = new { show=true },
                emphasis = new { show = true }
            };
            s.data = new List<Data>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.data.Add(new Data(){name=dt.Rows[i]["province"].ToString(),value=int.Parse(dt.Rows[i]["vehicle_count"].ToString())});
            }
            return s;
        }
    }
}
