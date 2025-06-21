using SendGrid;

namespace HypeLab.MailEngine.SendGrid
{
    /// <summary>
    /// Provides functionality to interact with the SendGrid API, enabling email delivery and related operations.
    /// </summary>
    /// <remarks>This class extends <see cref="SendGridClient"/> to provide additional capabilities or
    /// configurations specific to the Hype-Lab library. It requires a properly configured <see cref="HttpClient"/>
    /// and <see cref="SendGridClientOptions"/> to function correctly.</remarks>
    public class HypeLabSendGridClient : SendGridClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HypeLabSendGridClient"/> class, providing functionality to
        /// interact with the SendGrid API.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance used to send HTTP requests. Must be properly configured for use with
        /// the SendGrid API.</param>
        /// <param name="options">The <see cref="SendGridClientOptions"/> instance containing configuration settings  for the SendGrid client,
        /// such as API key and request options.</param>
        public HypeLabSendGridClient(HttpClient httpClient, SendGridClientOptions options)
            : base(httpClient, options) { }
    }
}
