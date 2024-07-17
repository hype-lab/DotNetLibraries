using HypeLab.MailEngine.Data.Enums;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials
{
    /// <summary>
    /// Represents the access info for SendGrid.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="apiKey"></param>
    /// <param name="isDefault"></param>
    public class SendGridAccessInfo(string clientId, string apiKey, bool? isDefault = null) : IMailAccessInfo
    {
        /// <summary>
        /// The type of the email sender.
        /// </summary>
        public EmailSenderType EmailSenderType => EmailSenderType.SendGrid;

        /// <summary>
        /// Indicates whether the access info is the default one.
        /// </summary>
        public bool IsDefault { get; } = isDefault ?? true;
        /// <summary>
        /// Id of the client.
        /// </summary>
        public string ClientId { get; } = clientId;

        /// <summary>
        /// The API key.
        /// </summary>
        public string ApiKey { get; } = apiKey;

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ApiKey: {ApiKey}, ClientId: {ClientId}, IsDefault: {IsDefault}";
        }

        /// <summary>
        /// Creates a new instance of the SendGridAccessInfo.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="apiKey"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public static SendGridAccessInfo Create(string clientId, string apiKey, bool isDefault)
        {
            return new SendGridAccessInfo(clientId, apiKey, isDefault);
        }
    }
}
