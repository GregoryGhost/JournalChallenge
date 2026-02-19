namespace JournalChallenge.Infrastructure.Journal;

using JournalChallenge.Domain.Journal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExceptionJournalConfiguration : IEntityTypeConfiguration<ExceptionJournal>
{
    public void Configure(EntityTypeBuilder<ExceptionJournal> builder)
    {
        builder.ToTable("ExceptionJournal");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.EventId)
               .IsRequired();

        builder.Property(e => e.Timestamp)
               .IsRequired();

        builder.Property(e => e.QueryParams).HasColumnType("jsonb");
        builder.Property(e => e.BodyParams).HasColumnType("jsonb");
        
        builder.Property(e => e.StackTrace)
               .IsRequired()
               .HasColumnType("text");
        
        builder.Property(e => e.ExceptionType)
               .IsRequired()
               .HasMaxLength(255);
        
        builder.Property(e => e.Message)
               .IsRequired();
    }
}
