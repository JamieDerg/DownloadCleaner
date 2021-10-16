using Serilog;

namespace DownloadCleaner
{
    public class Logger 
    {
        private static Serilog.Core.Logger instance;

        private Logger()
        {
            
        }

        public static Serilog.Core.Logger getInstance()
        {
            return instance ??= new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.log", 
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}