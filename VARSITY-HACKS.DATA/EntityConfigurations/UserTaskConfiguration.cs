using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VARSITY_HACKS.DATA;

public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.ToTable("UserTask");
        builder.Property(e => e.TaskName).HasMaxLength(128);
        builder.Property(e => e.StartDate).HasColumnType("date");
        builder.Property(e => e.EndDate).HasColumnType("date");

        builder.Property(e => e.InsertDateUtc)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getutcdate()");

        builder.Property(e => e.Priority)
            //.HasMaxLength(50)
            //.HasConversion<string>()
            .HasColumnName("PriorityLevelId")
            .IsRequired();
        builder.Property(e => e.IsSimultaneous)
            .IsRequired()
            .HasDefaultValueSql("((1))");

        //builder.HasOne(d => d.Registration)
        //    .WithMany(p => p.Tasks)
        //    .HasForeignKey(d => d.RegistrationId)
        //    .OnDelete(DeleteBehavior.ClientSetNull)
        //    .HasConstraintName("FK_UserTask_Registration");
    }
}