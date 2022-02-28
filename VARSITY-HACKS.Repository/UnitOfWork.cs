using AutoMapper;
using VARSITY_HACKS.DATA;
using Microsoft.AspNetCore.Identity;

namespace VARSITY_HACKS.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            Registration = new RegistrationRepository(_db, mapper);
            UserEvent = new UserEventRepository(db, mapper);
        }

        public IRegistrationRepository Registration { get; }
        public IUserEventRepository UserEvent { get; }


        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}