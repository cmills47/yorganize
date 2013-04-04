using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yorganize.Business.Repository
{
    public interface IKeyedRepository<TKey, TEntity> : IReadOnlyRepository<TEntity> where TEntity : class , IEntity<TKey>
    {
        TEntity FindByID(TKey id);

        TKey Add(TEntity entity);
        void Add(IEnumerable<TEntity> items);

        TKey Insert(TEntity entity);
        void Insert(IEnumerable<TEntity> items);

        void Save(TEntity entity);

        void Update(TEntity entity);
        void Update(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
    }
}
