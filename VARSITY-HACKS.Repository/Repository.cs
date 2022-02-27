using AutoMapper;
using VARSITY_HACKS.DATA;

namespace VARSITY_HACKS.Repository
{

    public class Repository
    {
        protected readonly ApplicationDbContext Db;
        protected readonly IMapper _mapper;

        public Repository(ApplicationDbContext db, IMapper mapper)
        {
            Db = db;
            _mapper = mapper;
        }
    }
}