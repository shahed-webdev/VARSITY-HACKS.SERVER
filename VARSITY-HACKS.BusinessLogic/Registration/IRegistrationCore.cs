using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic.Registration;

public interface IRegistrationCore
{
    Task<ResponseModel<RegistrationEditModel>> GetUserAsync(string userName);
    Task<ResponseModel<RegistrationEditModel>> EditAsync(string userName, RegistrationEditModel model);
    Task<ResponseModel> CreateAsync(string name,string userName);
    Task<ResponseModel<string>> GetModeAsync(string userName);
    Task<ResponseModel<string>> SetModeAsync(string userName, UserMode mode);
}