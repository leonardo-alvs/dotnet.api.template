using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TemplateTestsDomain = Template.Domain.Entities.Template.Template;

namespace Template.Infra.Maps.Template;

public class TemplateMap : EntityMap<TemplateTestsDomain>
{
    public override void Configure(EntityTypeBuilder<TemplateTestsDomain> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Id)
            .HasColumnName("CD_TEMPLATE_TESTS");

        builder.Property(t => t.Description)
            .HasColumnName("DS_DESCRIPTION")
            .HasColumnType("varchar(50)");

        builder.Property(t => t.Active)
            .HasColumnName("FL_ACTIVE")
            .HasColumnType("bit");

        builder.Property(t => t.InsertionDate)
            .HasColumnName("DT_INSERTION")
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.ToTable("TB_TEMPLATE_TESTS");
    }
}