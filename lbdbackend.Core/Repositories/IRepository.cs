using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lbdbackend.Core.Repositories {
    public interface IRepository<TEntity> {
        Task AddAsync(TEntity entity);
        Task<int> CommitAsync();
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);

        Task RemoveOrRestore(int? id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, params string[] includes);


    }
}
