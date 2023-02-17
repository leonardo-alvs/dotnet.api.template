using Serilog.Context;
using System.Text;


namespace Template.Api.Middlewares;

[ExcludeFromCodeCoverage]
public class SerilogHttpRequestMiddleware
{
    private const string HttpRequestPropertyName = "HttpRequest";
    readonly RequestDelegate _next;

    public SerilogHttpRequestMiddleware(RequestDelegate next)
    {
        if (next == null)
        {
            throw new ArgumentNullException(nameof(next));
        }

        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        var httpRequestInfo = GetHttpRequestInfoAsync(httpContext);

        // Push the user name into the log context so that it is included in all log entries
        using (LogContext.PushProperty(HttpRequestPropertyName, httpRequestInfo, true))
        {
            await _next(httpContext);
        }
    }

    private HttpContextInfo GetHttpRequestInfoAsync(HttpContext httpContext)
    {
        var httpRequest = httpContext?.Request;

        if (httpRequest == null)
        {
            return null;
        }

        string body = "";

        if (httpRequest.ContentLength.HasValue && httpRequest.ContentLength > 0)
        {
            httpRequest.EnableBuffering();

            using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8, false, 1024, true))
            {
                body = AsyncUtil.RunSync(() => reader.ReadToEndAsync());
            }

            // Reset the request body stream position so the next middleware can read it
            httpRequest.Body.Position = 0;
        }

        return new HttpContextInfo()
        {
            AccountNumber = GetAccountNumberClaim(httpContext),
            CpfCnpj = GetDocumentClaim(httpContext),
            Host = httpRequest.Host.ToString(),
            Path = httpRequest.Path,
            Scheme = httpRequest.Scheme,
            Method = httpRequest.Method,
            QueryString = httpRequest.Query.ToDictionary(x => x.Key, y => y.Value.ToString()),
            Headers = httpRequest.Headers
                        .Where(x =>
                            x.Key != "Cookie" && // remove Cookie from header since it is analysed separatly                        
                            x.Key != "Authorization" &&
                            !x.Key.Contains("X-"))
                        .ToDictionary(x => x.Key, y => y.Value.ToString()),
            Cookies = httpRequest.Cookies.ToDictionary(x => x.Key, y => y.Value.ToString()),
            Body = body
        };
    }

    private int GetAccountNumberClaim(HttpContext context)
    {
        if (context?.User == null) return 0;

        var userClaims = context.User.Claims.ToList();
        var accountNumberClaim = userClaims?.FirstOrDefault(a => a.Type == "AccountNumber");

        return accountNumberClaim != null ? int.Parse(accountNumberClaim.Value) : 0;
    }

    private string GetDocumentClaim(HttpContext context)
    {
        if (context?.User == null) return "";

        var userClaims = context.User.Claims.ToList();
        var documentClaim = userClaims?.FirstOrDefault(a => a.Type == "Document");

        return documentClaim != null ? documentClaim.Value : "";
    }
}

internal class HttpContextInfo
{
    public int AccountNumber { get; set; }
    public string CpfCnpj { get; set; }
    public string Host { get; set; }
    public string Path { get; set; }
    public string Scheme { get; set; }
    public string Method { get; set; }
    public Dictionary<string, string> QueryString { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public Dictionary<string, string> Cookies { get; set; }
    public string Body { get; set; }
}

/// <summary>
/// Helper class to run async methods within a sync process.
/// </summary>
internal static class AsyncUtil
{
    private static readonly TaskFactory _taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

    /// <summary>
    /// Executes an async Task method which has a void return value synchronously
    /// USAGE: AsyncUtil.RunSync(() => AsyncMethod());
    /// </summary>
    /// <param name="task">Task method to execute</param>
    public static void RunSync(Func<Task> task)
        => _taskFactory
            .StartNew(task)
            .Unwrap()
            .GetAwaiter()
            .GetResult();

    /// <summary>
    /// Executes an async Task<T> method which has a T return type synchronously
    /// USAGE: T result = AsyncUtil.RunSync(() => AsyncMethod<T>());
    /// </summary>
    /// <typeparam name="TResult">Return Type</typeparam>
    /// <param name="task">Task<T> method to execute</param>
    /// <returns></returns>
    public static TResult RunSync<TResult>(Func<Task<TResult>> task)
        => _taskFactory
            .StartNew(task)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
}
