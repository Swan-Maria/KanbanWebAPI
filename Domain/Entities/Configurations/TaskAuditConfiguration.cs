using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities.Configurations
{
    public class TaskAuditConfiguration : IEntityTypeConfiguration<TaskAudit>
    {
        public void Configure(EntityTypeBuilder<TaskAudit> builder)
        {
            builder.HasKey(x => x.AuditId);

            builder.Property(x => x.ChangeDescription)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.ChangedAt)
                .IsRequired();

            builder.HasOne(x => x.TaskItem)
                .WithMany()
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ChengedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChengedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}