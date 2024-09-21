using Serilog;

namespace GTA5ModRemover.Utilities
{
    public static class LoggerSetup
    {
        public static void InitializeLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() 
                .CreateLogger();
        }
    }
}
