using AutoMapper;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.Repository;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic.Registration;

public class RegistrationCore : Core, IRegistrationCore
{
    public RegistrationCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
    {
    }

    public Task<ResponseModel<RegistrationEditModel>> GetUserAsync(string userName)
    {
        try
        {
            return Task.FromResult(_db.Registration.Get(userName));
        }
        catch (Exception e)
        {
            return Task.FromResult(
                new ResponseModel<RegistrationEditModel>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<RegistrationEditModel>> EditAsync(string userName, RegistrationEditModel model)
    {
        try
        {
            return Task.FromResult(_db.Registration.Edit(userName, model));
        }
        catch (Exception e)
        {
            return Task.FromResult(
                new ResponseModel<RegistrationEditModel>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel> CreateAsync(string name, string userName)
    {
        try
        {
            _db.Registration.Create(name,userName);
            return Task.FromResult(new ResponseModel(true, $"{userName} Registered successfully"));
        }
        catch (Exception e)
        {
            return Task.FromResult(
                new ResponseModel(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<string>> GetModeAsync(string userName)
    {
        try
        {
    
            return Task.FromResult(_db.Registration.GetMode(userName));
        }
        catch (Exception e)
        {
            return Task.FromResult(
                new ResponseModel<string>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }

    public Task<ResponseModel<string>> SetModeAsync(string userName, UserMode mode)
    {
        try
        {
            return Task.FromResult(_db.Registration.SetMode(userName, mode));
        }
        catch (Exception e)
        {
            return Task.FromResult(
                new ResponseModel<string>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
        }
    }
}