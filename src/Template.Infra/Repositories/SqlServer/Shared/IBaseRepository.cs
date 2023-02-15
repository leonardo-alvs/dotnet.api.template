using Template.Domain.Entities.Shared;

namespace Template.Infra.Repositories.SqlServer.Shared;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetById(int id);
    Task<TEntity> Create(TEntity entity);
    Task<ICollection<TEntity>> CreateMany(ICollection<TEntity> entities);
    void Update(TEntity entity);
    Task<bool> Delete(int id);
}