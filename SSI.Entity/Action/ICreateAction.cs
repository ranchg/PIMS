using System;

namespace SSI.Entity.Action
{
    //创建操作接口 By 阮创 2017/11/30
    public interface ICreateAction
    {
        string F_Id { get; set; }
        string F_Create_By { get; set; }
        DateTime? F_Create_Time { get; set; }
    }
}
