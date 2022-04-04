using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VARSITY_HACKS.DATA;

public class UserEventDayConfiguration: IEntityTypeConfiguration<UserEventDay>
{
    public void Configure(EntityTypeBuilder<UserEventDay> builder)
    {
        builder.ToTable("UserEventDay");

        builder.Property(e => e.Day)
            .HasMaxLength(50)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(d => d.UserEvent)
            .WithMany(p => p.Days)
            .HasForeignKey(d => d.UserEventId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserEventDay_UserEvent");
    }
}