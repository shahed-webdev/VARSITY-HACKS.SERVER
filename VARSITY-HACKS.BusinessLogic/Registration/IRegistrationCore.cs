using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic.Registration;

public interface IRegistrationCore
{
    Task<ResponseModel<RegistrationEditModel>> GetUserAsync(string userName);
    Task<ResponseModel> EditAsync(string userName, RegistrationEditModel model);
    Task<ResponseModel> CreateAsync(string name,string userName);
}