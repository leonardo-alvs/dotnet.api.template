using Serilog.Events;
using Serilog;

namespace Template.Api.Config
{
    public class SerilogConfig
    {
        public static void AddSerilogConfig()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
               .Enrich.FromLogContext()               
               .WriteTo.Console(
                   outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:l} {Properties:j}{NewLine}{Exception}",
                   standardErrorFromLevel: LogEventLevel.Error)
               .CreateLogger();
        }
    }
}
