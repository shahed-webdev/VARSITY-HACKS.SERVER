using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VARSITY_HACKS.DATA;

public class UserCalendarEventConfiguration:IEntityTypeConfiguration<UserCalendarEvent>
{
    public void Configure(EntityTypeBuilder<UserCalendarEvent> builder)
    {
        builder.ToTable("UserCalendarEvent");
        builder.Property(e => e.SubTitle).HasMaxLength(128);
        builder.Property(e => e.EventDate).HasColumnType("date");
        builder.Property(e => e.StartDateTime)
            .HasColumnType("datetime")
            .HasComputedColumnSql("(CAST(EventDate AS DATETIME) + CAST(StartTime AS DATETIME))", true);
        builder.Property(e => e.EndDateTime)
            .HasColumnType("datetime")
        .HasComputedColumnSql("(DATEADD(MINUTE, [DurationMinute], (CONVERT([datetime],[EventDate])+CONVERT([datetime],[StartTime]))))", true);

        builder.Property(e => e.EndTime)
            .HasComputedColumnSql("(DATEADD(MINUTE, DurationMinute, StartTime))", true);
        
        builder.Property(e => e.DurationMinute)
            .IsRequired()
            .HasDefaultValueSql("0");
        
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

        builder.Property(e => e.IsSuggested)
            .IsRequired()
            .HasDefaultValueSql("((0))");
        
        builder.HasOne(d => d.Registration)
            .WithMany(p => p.CalendarEvents)
            .HasForeignKey(d => d.RegistrationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UserCalendarEvent_Registration");

        builder.HasOne(d => d.UserEvent)
            .WithMany(p => p.CalendarEvents)
            .HasForeignKey(d => d.UserEventId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserCalendarEvent_UserEvent");
    }
}