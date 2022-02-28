using AutoMapper;
using VARSITY_HACKS.DATA;

namespace VARSITY_HACKS.Repository;

public class UserEventRepository:Repository, IUserEventRepository
{
    public UserEventRepository(ApplicationDbContext db, IMapper mapper) : base(db, mapper)
    {
    }
}