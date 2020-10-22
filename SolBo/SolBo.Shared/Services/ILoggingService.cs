using NLog;

namespace SolBo.Shared.Services
{
    public interface ILoggingService
    {
        void Trace(string msg);
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
    }

    public class LoggingService : ILoggingService
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public void Trace(string msg)
        {
            Logger.Trace(msg);
        }

        public void Info(string msg)
        {
            Logger.Info(msg);
        }

        public void Warn(string msg)
        {
            Logger.Warn(msg);
        }

        public void Error(string msg)
        {
            Logger.Error(msg);
        }
    }
}