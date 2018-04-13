using SSI.Entity.Action;
using SSI.Entity.Base;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SSI.Entity.SystemManage
{
    [Description("车辆省份分布")]
    public class VehicleDis : BaseEntity<VehicleDis>, IModifyAction
    {
        [PropertyCN("Id编号")]
        //ID 编号
        public int F_Id { get; set; }

        [PropertyCN("省份名称")]
        //省份名称
        public string F_Name { get; set; }

        [PropertyCN("车辆总数")]
        //车辆总数
        public int F_Total { get; set; }

    }
}
