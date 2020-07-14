using System;
using System.Collections.Specialized;
using System.Net;

namespace SolBo.Shared.Services.Implementations
{
    public class PushOverNotificationService : IPushOverNotificationService
    {
        private readonly string _token;
        private readonly string _recipients;
        private readonly string _endpoint;
        private readonly bool _isActive;
        public PushOverNotificationService(
            string token,
            string recipients,
            string endpoint,
            bool isActive)
        {
            _token = token;
            _recipients = recipients;
            _endpoint = endpoint;
            _isActive = isActive;
        }
        public void Send(string title, string message)
        {
            if (_isActive)
            {
                if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(message))
                {
                    var parameters = new NameValueCollection
                    {
                        { "token", _token },
                        { "user", _recipients },
                        { "message", message },
                        { "title", title },
                        { "html", "1" }
                    };

                    try
                    {
                        using var client = new WebClient();
                        client.UploadValues(_endpoint, parameters);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        public bool IsActive()
            => _isActive;
    }
}