using SSI.Entity.Action;
using SSI.Entity.Base;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace SSI.Repository
{
    public class RepositoryFactory<TEntity> where TEntity : BaseEntity<TEntity>, IModifyAction, new()
    {
        //获取仓库 By 阮创 2017/11/30
        public IRepository<TEntity> Repository()
        {
            return new Repository<TEntity>();
        }

        //获取表单 By 阮创 2017/11/30
        public virtual TEntity GetForm(string F_Id)
        {
            return Repository().FindEntity(F_Id);
        }

        //提交表单 By 阮创 2017/11/30
        public virtual int SubmitForm(TEntity entity)
        {
            if (string.IsNullOrEmpty(entity.F_Id))
            {
                return Repository().Insert(entity.Create());
            }
            else
            {
                return Repository().Update(entity.Modify());
            }
        }

        //提交表单 By 阮创 2017/11/30
        public virtual int SubmitForm(TEntity entity, DbTransaction isOpenTrans)
        {
            if (string.IsNullOrEmpty(entity.F_Id))
            {
                return Repository().Insert(entity.Create(), isOpenTrans);
            }
            else
            {
                return Repository().Update(entity.Modify(), isOpenTrans);
            }
        }

        //删除表单 By 阮创 2017/11/30
        public virtual int DeleteForm(string F_Id)
        {
            return Repository().Update(GetForm(F_Id).Delete());
        }

        //删除表单 By 阮创 2017/11/30
        public virtual int DeleteForm(string F_Id, DbTransaction isOpenTrans)
        {
            return Repository().Update(GetForm(F_Id).Delete(), isOpenTrans);
        }


        //批量插入表单 By 阮创 2017/11/30
        public virtual int InsertFormBatch(List<TEntity> entitys)
        {
            entitys.ForEach(entity =>
            {
                entity.Create();
            });
            return Repository().Insert(entitys);
        }

        //批量插入表单 By 阮创 2017/11/30
        public virtual int InsertFormBatch(List<TEntity> entitys, DbTransaction isOpenTrans)
        {
            entitys.ForEach(entity =>
            {
                entity.Create();
            });
            return Repository().Insert(entitys, isOpenTrans);
        }


        //批量更新表单 By 阮创 2017/11/30
        public virtual int UpdateFormBatch(List<TEntity> entitys)
        {
            entitys.ForEach(entity =>
            {
                entity.Modify();
            });
            return Repository().Update(entitys);
        }

        //批量更新表单 By 阮创 2017/11/30
        public virtual int UpdateFormBatch(List<TEntity> entitys, DbTransaction isOpenTrans)
        {
            entitys.ForEach(entity =>
            {
                entity.Modify();
            });
            return Repository().Update(entitys, isOpenTrans);
        }

        //删除表单批量 By 阮创 2017/11/30
        public virtual int DeleteFormBatch(List<TEntity> entitys)
        {
            entitys.ForEach(entity =>
            {
                entity.Delete();
            });
            return Repository().Update(entitys);
        }

        //删除表单批量 By 阮创 2017/11/30
        public virtual int DeleteFormBatch(List<TEntity> entitys, DbTransaction isOpenTrans)
        {
            entitys.ForEach(entity =>
            {
                entity.Delete();
            });
            return Repository().Update(entitys, isOpenTrans);
        }

        //删除表单批量 By 阮创 2017/11/30
        public virtual int DeleteFormBatch(string F_Id)
        {
            return Repository().Update(F_Id.Split(',').Select(e => GetForm(e).Delete()).ToList());
        }

        //删除表单批量 By 阮创 2017/11/30
        public virtual int DeleteFormBatch(string F_Id, DbTransaction isOpenTrans)
        {
            return Repository().Update(F_Id.Split(',').Select(e => GetForm(e).Delete()).ToList(), isOpenTrans);
        }
    }
}
