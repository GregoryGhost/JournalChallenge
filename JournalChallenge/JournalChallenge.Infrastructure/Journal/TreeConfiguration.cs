namespace JournalChallenge.Infrastructure.Journal;

using JournalChallenge.Domain.Journal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TreeConfiguration : IEntityTypeConfiguration<Tree>
{
    public void Configure(EntityTypeBuilder<Tree> builder)
    {
        builder.ToTable("Trees");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasIndex(t => t.Name)
               .IsUnique();

        builder.HasMany(t => t.Nodes)
               .WithOne(n => n.Tree)
               .HasForeignKey(n => n.TreeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
