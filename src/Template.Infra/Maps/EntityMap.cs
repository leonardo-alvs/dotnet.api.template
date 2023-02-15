using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Shared;

namespace Template.Infra.Maps;

public class EntityMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Ignore(t => t.Invalid);
        builder.Ignore(t => t.ValidationResult);
        builder.Ignore(t => t.Valid);
    }
}