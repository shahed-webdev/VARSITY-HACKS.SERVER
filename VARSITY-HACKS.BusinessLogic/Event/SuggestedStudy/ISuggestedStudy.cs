using VARSITY_HACKS.DATA;
using VARSITY_HACKS.Repository;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public interface ISuggestedStudy
{
    void AddSuggestedStudy(int registrationId, int userEventId, DifficultyLevel difficulty, IUnitOfWork db);
}