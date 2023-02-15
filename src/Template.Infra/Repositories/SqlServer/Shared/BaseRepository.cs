using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities.Shared;
using Template.Infra.Data.SqlServer;

namespace Template.Infra.Repositories.SqlServer.Shared;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
{
    internal DbSet<TEntity> DbSet;
    internal SqlServerContext DefaultContext;
    internal SqlServerDbSession Session;

    public BaseRepository(SqlServerContext defaultContext, SqlServerDbSession session)
    {
        DefaultContext = defaultContext;
        Session = session;
        DbSet = DefaultContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll()
    {
        return DbSet
            .AsQueryable()
            .AsNoTracking();
    }

    public async Task<TEntity?> GetById(int id)
        => await DbSet.FindAsync(id);

    public async Task<TEntity> Create(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public async Task<ICollection<TEntity>> CreateMany(ICollection<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
        return entities;
    }

    public void Update(TEntity entity)
        => DbSet.Update(entity);

    public async Task<bool> Delete(int id)
    {
        var registro = await DbSet.FindAsync(id);

        if (registro != null)
        {
            DbSet.Remove(registro);
            return true;
        }

        return false;
    }
}