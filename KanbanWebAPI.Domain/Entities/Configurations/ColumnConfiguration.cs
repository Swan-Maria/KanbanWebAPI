using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanWebAPI.Domain.Entities.Configurations
{
    public class ColumnConfiguration : IEntityTypeConfiguration<Column>
    {
        public void Configure(EntityTypeBuilder<Column> builder)
        {
            builder.HasKey(x => x.ColumnId);

            builder.Property(x => x.ColumnName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(x => x.Board)
                  .WithMany(x => x.Columns)
                  .HasForeignKey(x => x.BoardId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}