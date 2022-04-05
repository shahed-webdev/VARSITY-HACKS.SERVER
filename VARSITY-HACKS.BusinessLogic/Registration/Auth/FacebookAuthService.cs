using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public class FacebookAuthService: IFacebookAuthService
{
    private const string TokenValidationUrl = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}";
    private const string UserInfoUrl = "https://graph.facebook.com/me?fields=id,name,email,picture&access_token={0}";
    private readonly FacebookAuthSettings _facebookAuthSettings;
    private readonly IHttpClientFactory _httpClientFactory;

    public FacebookAuthService( IHttpClientFactory httpClientFactory)
    {
        _facebookAuthSettings = new FacebookAuthSettings();
        _httpClientFactory = httpClientFactory;
    }

    public async Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken)
    {
        var formattedUrl = string.Format(TokenValidationUrl, accessToken, _facebookAuthSettings.AppId + "|" + _facebookAuthSettings.AppSecret);
        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
        result.EnsureSuccessStatusCode();
        var jsonResult = await result.Content.ReadAsStringAsync();
        var facebookTokenValidationResult = JsonConvert.DeserializeObject<FacebookTokenValidationResult>(jsonResult);
        return facebookTokenValidationResult;
    }

    public async Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken)
    {
        var formattedUrl = string.Format(UserInfoUrl, accessToken);
        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
        result.EnsureSuccessStatusCode();
        var jsonResult = await result.Content.ReadAsStringAsync();
        var facebookUserInfoResult = JsonConvert.DeserializeObject<FacebookUserInfoResult>(jsonResult);
        return facebookUserInfoResult;
    }
}