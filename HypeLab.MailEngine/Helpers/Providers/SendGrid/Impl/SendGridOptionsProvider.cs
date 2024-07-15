//using SendGrid;
//using System.Collections.Concurrent;

//namespace HypeLab.MailEngine.Helpers.Providers.SendGrid.Impl
//{
//    /// <summary>
//    /// Sendgrid client options provider by clientId.
//    /// </summary>
//    public class SendGridOptionsProvider : ISendGridOptionsProvider
//    {
//        private static readonly ConcurrentDictionary<string, SendGridClientOptions> _options = new();

//        /// <summary>
//        /// SendGridOptionsProvider constructor.
//        /// </summary>
//        public SendGridOptionsProvider() { }

//        /// <summary>
//        /// Configures the SendGrid client options.
//        /// </summary>
//        /// <param name="clientId"></param>
//        /// <param name="options"></param>
//        public void Configure(string clientId, SendGridClientOptions options)
//        {
//            _options.TryAdd(clientId, options);
//        }

//        /// <summary>
//        /// Get the SendGrid client options by clientId.
//        /// </summary>
//        /// <param name="clientId"></param>
//        /// <returns></returns>
//        /// <exception cref="KeyNotFoundException"></exception>
//        public SendGridClientOptions GetOptions(string clientId)
//        {
//            if (_options.TryGetValue(clientId, out var options))
//            {
//                return options;
//            }

//            throw new KeyNotFoundException($"No SendGridClientOptions found for clientId: {clientId}");
//        }
//    }
//}
