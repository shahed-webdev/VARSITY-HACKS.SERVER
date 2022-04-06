using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public interface IExternalAuthService
{
    Task<FacebookTokenValidationResult> ValidateFacebookAccessTokenAsync(string accessToken);
    Task<FacebookUserInfoResult> GetFacebookUserInfoAsync(string accessToken);
    Task<GoogleApiTokenInfo> ValidateGoogleUserInfoAsync(string accessToken);
}