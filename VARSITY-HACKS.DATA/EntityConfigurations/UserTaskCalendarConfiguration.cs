using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VARSITY_HACKS.DATA;

public class UserTaskCalendarConfiguration:IEntityTypeConfiguration<UserTaskCalendar>
{
    public void Configure(EntityTypeBuilder<UserTaskCalendar> builder)
    {
        throw new NotImplementedException();
    }
}