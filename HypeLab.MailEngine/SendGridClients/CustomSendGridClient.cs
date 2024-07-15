using SendGrid;

namespace HypeLab.MailEngine.SendGrid
{
    /// <summary>
    /// Class like the InjectedSendGridClient, but with a custom implementation used to manage keyed HttpClient and SendGridClientOptions instances.
    /// </summary>
    /// <remarks>
    /// Custom SendGrid client constructor.
    /// </remarks>
    /// <param name="httpClient"></param>
    /// <param name="options"></param>
    public class CustomSendGridClient(HttpClient httpClient, SendGridClientOptions options)
        : SendGridClient(httpClient, options) { }
}
