using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.Repository;

public interface IRegistrationRepository
{
    int RegistrationIdByUserName(string userName);

    ResponseModel Edit(string userName, RegistrationEditModel model);
    void Create(string name, string userName);
    ResponseModel<RegistrationEditModel> Get(string userName);
}