using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities.Configurations
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.HasKey(x => x.BoardId);

            builder.Property(x => x.BoardName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.BoardDescription)
                .HasMaxLength(300);

            builder.HasOne(x => x.Team)
                .WithMany(x => x.Boards)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}