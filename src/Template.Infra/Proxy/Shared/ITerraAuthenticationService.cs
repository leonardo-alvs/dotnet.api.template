namespace Template.Infra.Proxy.Shared;

public interface ITemplateAuthenticationService
{
    Task<string> GetToken();

    bool TokenExpired();
}
