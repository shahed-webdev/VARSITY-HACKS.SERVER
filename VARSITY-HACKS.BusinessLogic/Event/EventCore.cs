using AutoMapper;
using VARSITY_HACKS.Repository;

namespace VARSITY_HACKS.BusinessLogic;

public class EventCore :Core, IEventCore
{
    public EventCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
    {
    }
}