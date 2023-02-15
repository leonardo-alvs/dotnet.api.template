using System.Text.Json;

using Microsoft.Extensions.Configuration;

using Serilog;

namespace Template.Infra.Proxy.Shared;

public class TemplateAuthenticationService : ITemplateAuthenticationService
{
    private readonly IConfiguration _configuration;

    private TemplateAuthenticationToken _token;

    private JsonSerializerOptions JsonSerializerOptions
    {
        get
        {
            return new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
        }
    }

    public TemplateAuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
        _token = new TemplateAuthenticationToken();
    }

    public async Task<string> GetToken()
    {
        if (TokenExpired())
            return await GetNewToken();
        return _token.Access_token;
    }

    public bool TokenExpired()
    {
        return _token.ExpirationTime < DateTime.Now;
    }

    private async Task<string> GetNewToken()
    {
        var _userName = _configuration.GetValue<string>("Template.Authentication:UserName").Trim();
        var _password = _configuration.GetValue<string>("Template.Authentication:Password").Trim();
        var _urlToken = $"{_configuration.GetValue<string>("Template.Authentication:Api.BaseUrl").Trim()}/oauth/token";

        Log.Information($"AuthenticationService - GetTokenRequest on {_urlToken}");

        var requestData = new Dictionary<string, string>();
        requestData.Add("grant_type", "password");
        requestData.Add("username", _userName);
        requestData.Add("password", _password);

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, _urlToken) { Content = new FormUrlEncodedContent(requestData) };
        var response = await client.SendAsync(request);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"AuthenticationService - ERROR on request: {request.Method} - {request.RequestUri} \n  Status: {response.StatusCode} Response: {responseContent}");
        }

        try
        {
            _token = JsonSerializer.Deserialize<TemplateAuthenticationToken>(responseContent, JsonSerializerOptions)!;
            _token.ExpirationTime = DateTime.Now.AddSeconds(_token.Expires_in);
        }
        catch (Exception ex)
        {
            Log.Error($"AuthenticationService. ERROR deserializing {responseContent} into {typeof(TemplateAuthenticationToken)} \n  Error: {ex.Message}");
            throw;
        }

        Log.Information($"AuthenticationService - Authentication successful");

        return _token.Access_token;
    }
}

public class TemplateAuthenticationToken
{
    public string Access_token { get; set; }

    public string Token_type { get; set; }

    public int Expires_in { get; set; }

    public DateTime ExpirationTime { get; set; }

    public TemplateAuthenticationToken()
    {
        ExpirationTime = DateTime.MinValue;
    }
}
