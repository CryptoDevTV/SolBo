namespace SolBo.Shared.Services
{
    public interface IPushOverNotificationService
    {
        void Send(string title, string message);
        bool IsActive();
    }
}