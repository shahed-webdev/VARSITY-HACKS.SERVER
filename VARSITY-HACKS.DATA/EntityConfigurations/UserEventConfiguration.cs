using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VARSITY_HACKS.DATA;

public class UserEventConfiguration : IEntityTypeConfiguration<UserEvent>
{
    public void Configure(EntityTypeBuilder<UserEvent> builder)
    {
        builder.ToTable("UserEvent");
        builder.Property(e => e.EventName).HasMaxLength(128);
        builder.Property(e => e.EventType)
            .HasColumnName("EventTypeId")
            .IsRequired();
        builder.Property(e => e.StartDate).HasColumnType("date");
        builder.Property(e => e.EndDate).HasColumnType("date");

        builder.Property(e => e.EndTime)
            .HasComputedColumnSql("(DATEADD(MINUTE, DurationMinute, StartTime))",true); 

        builder.Property(e => e.InsertDateUtc)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getutcdate()");

        builder.Property(e => e.Priority)
            .HasColumnName("PriorityLevelId")
            .IsRequired();

        builder.Property(e => e.Difficulty)
            .HasColumnName("DifficultyLevelId")
            .IsRequired();

        builder.Property(e => e.IsSimultaneous)
            .IsRequired()
            .HasDefaultValueSql("((1))");

        builder.HasOne(d => d.Registration)
            .WithMany(p => p.Events)
            .HasForeignKey(d => d.RegistrationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UserEvent_Registration");
    }
}