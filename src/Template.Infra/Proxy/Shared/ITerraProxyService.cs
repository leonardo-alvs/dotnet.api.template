namespace Template.Infra.Proxy.Shared;
public interface ITemplateProxyService
{
    Task DeleteAsync(string url, object body);
    Task<TGet> GetAsync<TGet>(string url);
    Task PostAsync(string url, object body);
    Task<TReturn> PostAsync<TReturn>(string url, object body);
    Task PutAsync(string url, object body);
    Task<TReturn> PutAsync<TReturn>(string url);
    Task<TReturn> PutAsync<TReturn>(string url, object body);
}