using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VARSITY_HACKS.DATA;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public virtual DbSet<Registration> Registrations { get; set; } = null!;
    public virtual DbSet<UserEvent> UserEvents { get; set; } = null!;
    public virtual DbSet<UserEventDay> UserEventDays { get; set; } = null!;
    public virtual DbSet<UserCalendarEvent> UserCalendarEvents { get; set; } = null!;

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlServer("Server=.;Database=TestAPI;Trusted_Connection=True;MultipleActiveResultSets=true");
    //    }

    //    base.OnConfiguring(optionsBuilder);
    //}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RegistrationConfiguration());
        modelBuilder.ApplyConfiguration(new UserEventConfiguration());
        modelBuilder.ApplyConfiguration(new UserEventDayConfiguration());
        modelBuilder.ApplyConfiguration(new UserCalendarEventConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}