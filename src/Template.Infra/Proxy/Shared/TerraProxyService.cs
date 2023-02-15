using System.Net.Mime;
using System.Text;
using System.Text.Json;

using Microsoft.Net.Http.Headers;

using Serilog;

namespace Template.Infra.Proxy.Shared;

public class TemplateProxyService : ITemplateProxyService
{
    private readonly ITemplateAuthenticationService _authenticationService;

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

    private readonly HttpClient _httpClient;

    public TemplateProxyService(ITemplateAuthenticationService authenticationService)
    {
        _httpClient = new HttpClient();
        _authenticationService = authenticationService;
    }

    public async Task<TGet> GetAsync<TGet>(string url)
    {
        var httpRequestMessage = await CreateRequest(HttpMethod.Get, url);

        var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        ValidateResponse(httpResponseMessage, httpRequestMessage);

        return await HandleResponse<TGet>(httpResponseMessage, httpRequestMessage);
    }

    public async Task<TReturn> PostAsync<TReturn>(string url, object body)
    {
        var httpRequestMessage = await CreateRequest(HttpMethod.Post, url, body);

        using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        ValidateResponse(httpResponseMessage, httpRequestMessage);

        return await HandleResponse<TReturn>(httpResponseMessage, httpRequestMessage);
    }

    public async Task PostAsync(string url, object body)
    {
        var httpRequestMessage = await CreateRequest(HttpMethod.Post, url, body);

        using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        ValidateResponse(httpResponseMessage, httpRequestMessage);
    }

    public async Task PutAsync(string url, object body)
    {
        var httpRequestMessage = await CreateRequest(HttpMethod.Put, url, body);

        using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        ValidateResponse(httpResponseMessage, httpRequestMessage);
    }

    public async Task<TReturn> PutAsync<TReturn>(string url)
    {
        var httpRequestMessage = await CreateRequest(HttpMethod.Put, url);

        using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        ValidateResponse(httpResponseMessage, httpRequestMessage);

        return await HandleResponse<TReturn>(httpResponseMessage, httpRequestMessage);
    }

    public async Task<TReturn> PutAsync<TReturn>(string url, object body)
    {
        var httpRequestMessage = await CreateRequest(HttpMethod.Put, url, body);

        using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        ValidateResponse(httpResponseMessage, httpRequestMessage);

        return await HandleResponse<TReturn>(httpResponseMessage, httpRequestMessage);
    }

    public async Task DeleteAsync(string url, object body)
    {
        var httpRequestMessage = await CreateRequest(HttpMethod.Delete, url, body);

        using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        ValidateResponse(httpResponseMessage, httpRequestMessage);
    }

    private void ValidateResponse(HttpResponseMessage httpResponseMessage, HttpRequestMessage httpRequestMessage)
    {
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStreamErro = httpResponseMessage.Content.ReadAsStream();
            StreamReader reader = new StreamReader(contentStreamErro);
            string responseText = reader.ReadToEnd();
            throw new Exception($"ProxyService ValidateResponse. ERROR on request: {httpRequestMessage.Method} - {httpRequestMessage.RequestUri} \n  Status: {httpResponseMessage.StatusCode} Response: {responseText}");
        }
    }

    private async Task<T> HandleResponse<T>(HttpResponseMessage httpResponseMessage, HttpRequestMessage httpRequestMessage)
    {
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        Log.Information($"{httpRequestMessage.Method} - {httpRequestMessage.RequestUri} \n Response {responseContent}");
        try
        {
            return JsonSerializer.Deserialize<T>(responseContent, JsonSerializerOptions)!;
        }
        catch (Exception ex)
        {
            Log.Error($"ProxyService HandleResponse<T>. ERROR deserializing {responseContent} into {typeof(T)} \n  Error: {ex.Message}");
            throw;
        }
    }


    private async Task<HttpRequestMessage> CreateRequest(HttpMethod httpMethod, string url, object body)
    {
        Log.Information($"{httpMethod.Method} - {url.Trim()}");

        var httpRequest = await CreateRequest(httpMethod, url);
        httpRequest.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, MediaTypeNames.Application.Json);
        return httpRequest;
    }

    private async Task<HttpRequestMessage> CreateRequest(HttpMethod httpMethod, string url)
    {
        var httpRequestMessage = new HttpRequestMessage(httpMethod, url)
        {
            Headers = {
                { "ContentType", "application/json" },
                { HeaderNames.Authorization, $"Bearer {await _authenticationService.GetToken()}" },
                { HeaderNames.UserAgent, "api" }
            }
        };

        return httpRequestMessage;
    }


}