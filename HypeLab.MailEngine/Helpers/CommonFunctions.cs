using HypeLab.MailEngine.Factories;
using HypeLab.MailEngine.Strategies.EmailSender;

namespace HypeLab.MailEngine.Helpers
{
    internal static class CommonFunctions
    {
        public static IEmailSender DetermineSenderByClientId(this string clientId, IEmailSenderFactory emailSenderFactory)
        {
            ArgumentException.ThrowIfNullOrEmpty(clientId);

            return emailSenderFactory.CreateEmailSender(clientId);
        }
    };
}
