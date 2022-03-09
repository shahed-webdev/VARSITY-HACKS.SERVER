using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VARSITY_HACKS.DATA;

public class RegistrationConfiguration: IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.ToTable("Registration");
        builder.Property(e => e.Name).HasMaxLength(128);
        builder.Property(e => e.UserName).HasMaxLength(50);
        builder.Property(e => e.Email).HasMaxLength(50);
        builder.Property(e => e.City).HasMaxLength(128);
        builder.Property(e => e.State).HasMaxLength(128);
        builder.Property(e => e.UniversityName).HasMaxLength(128);
        builder.Property(e => e.Subject).HasMaxLength(128);
        builder.Property(e => e.SocialMediaLink).HasMaxLength(512);
       
        builder.Property(e => e.InsertDateUtc)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getutcdate()");

        builder.Property(e => e.Personality)
            //.HasMaxLength(50)
            //.HasConversion<string>()
            .HasColumnName("PersonalityTypeId")
            .IsRequired();
        builder.Property(e => e.Mode)
            //.HasMaxLength(50)
            //.HasConversion<string>()
            .HasColumnName("UserModeId")
            .HasDefaultValueSql("1")
            .IsRequired();

        builder.Property(e => e.Validation)
            .IsRequired()
            .HasDefaultValueSql("((1))");
    }
}