using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanWebAPI.Domain.Entities.Configurations
{
    public class TaskAuditConfiguration : IEntityTypeConfiguration<TaskAudit>
    {
        public void Configure(EntityTypeBuilder<TaskAudit> builder)
        {
            builder.HasKey(x => x.AuditId);

            builder.Property(x => x.Action)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.CreateAt)
                .IsRequired();

            builder.HasOne(x => x.TaskItem)
                .WithMany()
                .HasForeignKey(x => x.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.CreateByUser)
                .WithMany()
                .HasForeignKey(x => x.CreateByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}