using System;

namespace VARSITY_HACKS.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRegistrationRepository Registration { get; }
        IUserEventRepository UserEvent { get; }
        int SaveChanges();
    }
}