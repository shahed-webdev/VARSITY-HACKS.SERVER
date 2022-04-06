using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.BusinessLogic;

public class ExternalAuthService : IExternalAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public ExternalAuthService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task<FacebookTokenValidationResult> ValidateFacebookAccessTokenAsync(string accessToken)
    {
        const string tokenValidationUrl = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}";

        var formattedUrl = string.Format(tokenValidationUrl, accessToken,
            _config["Authentication:Facebook:ClientId"] + "|" + _config["Authentication:Facebook:ClientSecret"]);
        
        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
        result.EnsureSuccessStatusCode();
        var jsonResult = await result.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<FacebookTokenValidationResult>(jsonResult) ?? new FacebookTokenValidationResult();

    }

    public async Task<FacebookUserInfoResult> GetFacebookUserInfoAsync(string accessToken)
    {
        const string userInfoUrl = "https://graph.facebook.com/me?fields=id,name,email,picture&access_token={0}";

        var formattedUrl = string.Format(userInfoUrl, accessToken);
        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
        result.EnsureSuccessStatusCode();
        var jsonResult = await result.Content.ReadAsStringAsync();
        
        return JsonConvert.DeserializeObject<FacebookUserInfoResult>(jsonResult) ?? new FacebookUserInfoResult();

    }

    public async Task<GoogleApiTokenInfo> ValidateGoogleUserInfoAsync(string accessToken)
    {
        const string userInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";

        var formattedUrl = string.Format(userInfoUrl, accessToken);
        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
        result.EnsureSuccessStatusCode();
        var jsonResult = await result.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<GoogleApiTokenInfo>(jsonResult) ?? new GoogleApiTokenInfo();

    }
}