using SSI.Entity.Action;
using SSI.Entity.Manage;
using System;

namespace SSI.Entity.Base
{
    public class BaseEntity<TEntity> where TEntity : class, new()
    {
        //创建操作时执行 By 阮创 2017/11/30
        public virtual TEntity Create()
        {
            var entity = this as ICreateAction;
            entity.F_Id = Guid.NewGuid().ToString();
            if (ManageProvider.Provider.Current() != null)
            {
                entity.F_Create_By = ManageProvider.Provider.Current().User.F_Account;
            }
            entity.F_Create_Time = DateTime.Now;
            return this as TEntity;
        }
        //修改操作时执行 By 阮创 2017/11/30
        public virtual TEntity Modify()
        {
            var entity = this as IModifyAction;
            return this as TEntity;
        }
        //删除操作时执行 By 阮创 2017/11/30
        public virtual TEntity Delete()
        {
            var entity = this as IDeleteAction;
            entity.F_Delete_Mark = 1;
            return this as TEntity;
        }
    }
}
