using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VARSITY_HACKS.DATA;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public virtual DbSet<Registration> Registrations { get; set; } = null!;
    //public virtual DbSet<UserTask> UserTasks { get; set; } = null!;
    //public virtual DbSet<UserTaskDay> UserTaskDays { get; set; } = null!;
    //public virtual DbSet<UserTaskCalendar> UserTaskCalendars { get; set; } = null!;

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
        //modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
        //modelBuilder.ApplyConfiguration(new UserTaskDayConfiguration());
        //modelBuilder.ApplyConfiguration(new UserTaskCalendarConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}