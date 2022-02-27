using AutoMapper;
using VARSITY_HACKS.Repository;

namespace VARSITY_HACKS.BusinessLogic
{
    public class Core
    {
        protected readonly IUnitOfWork _db;
        protected readonly IMapper _mapper;

        public Core(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
    }
}