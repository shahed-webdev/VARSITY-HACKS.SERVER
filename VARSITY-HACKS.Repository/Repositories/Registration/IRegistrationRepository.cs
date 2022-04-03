using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.Repository;

public interface IRegistrationRepository
{
    int RegistrationIdByUserName(string userName);
    ResponseModel<RegistrationEditModel> Edit(string userName, RegistrationEditModel model);
    void Create(string name, string userName);
    ResponseModel<RegistrationEditModel> Get(string userName);
    ResponseModel<string> GetMode(string userName);
    ResponseModel<string> SetMode(string userName, UserMode mode);
    PersonalityType GetPersonalityType(string userName);
}