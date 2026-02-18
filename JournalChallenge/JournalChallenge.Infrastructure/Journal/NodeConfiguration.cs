namespace JournalChallenge.Infrastructure.Journal;

using JournalChallenge.Domain.Journal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("Nodes");

        builder.HasKey(n => n.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(n => n.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasOne(n => n.Parent)
               .WithMany(n => n.Children)
               .HasForeignKey(n => n.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(n => n.TreeId);
    }
}
