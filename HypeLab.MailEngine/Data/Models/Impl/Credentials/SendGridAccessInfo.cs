using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes;
using Newtonsoft.Json;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials
{
    /// <summary>
    /// Represents the access info for SendGrid.
    /// </summary>
    public class SendGridAccessInfo : IMailAccessInfo
    {
        /// <summary>
        /// the default constructor.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="apiKey"></param>
        /// <param name="isDefault"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="host"></param>
        /// <param name="version"></param>
        /// <param name="urlPath"></param>
        /// <param name="auth"></param>
        /// <param name="reliability"></param>
        /// <param name="httpErrorAsException"></param>
        public SendGridAccessInfo(
            string clientId,
            string apiKey,
            bool? isDefault = null,
            HashSet<RequestHeaderKeyValue>? requestHeaders = null,
            string? host = null,
            string? version = null,
            string? urlPath = null,
            AuthHeaderValue? auth = null,
            ReliabilityValue? reliability = null,
            bool? httpErrorAsException = null)
        {
            IsDefault = isDefault ?? true;
            ClientId = clientId;
            ApiKey = apiKey;
            RequestHeaders = requestHeaders;
            Reliability = reliability;
            Host = host;
            Version = version;
            UrlPath = urlPath;
            Auth = auth;
            HttpErrorAsException = httpErrorAsException ?? false;
        }
        /// <summary>
        /// The type of the email sender.
        /// </summary>
        public EmailSenderType EmailSenderType => EmailSenderType.SendGrid;

        /// <summary>
        /// Indicates whether the access info is the default one.
        /// </summary>
        public bool IsDefault { get; }
        /// <summary>
        /// Id of the client.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// The API key.
        /// </summary>
        public string ApiKey { get; }

        /// <summary>
        /// The request headers to use on HTTP Requests.
        /// </summary>
        public HashSet<RequestHeaderKeyValue>? RequestHeaders { get; }

        /// <summary>
        /// The reliability settings to use on HTTP Requests.
        /// </summary>
        public ReliabilityValue? Reliability { get; }

        /// <summary>
        /// The base URL.
        /// </summary>
        public string? Host { get; }

        /// <summary>
        /// The API version.
        /// </summary>
        public string? Version { get; }

        /// <summary>
        /// The path to the API endpoint.
        /// </summary>
        public string? UrlPath { get; }

        /// <summary>
        /// The Auth header value.
        /// </summary>
        public AuthHeaderValue? Auth { get; }

        /// <summary>
        /// Gets or sets a value indicating whether HTTP error responses should be raised as exceptions. Default is false.
        /// </summary>
        public bool HttpErrorAsException { get; }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SendGridAccessInfo"/> class.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="apiKey"></param>
        /// <param name="isDefault"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="host"></param>
        /// <param name="version"></param>
        /// <param name="urlPath"></param>
        /// <param name="auth"></param>
        /// <param name="reliability"></param>
        /// <param name="httpErrorAsException"></param>
        /// <returns></returns>
        public static SendGridAccessInfo Create(
            string clientId,
            string apiKey,
            bool? isDefault = null,
            HashSet<RequestHeaderKeyValue>? requestHeaders = null,
            string? host = null,
            string? version = null,
            string? urlPath = null,
            AuthHeaderValue? auth = null,
            ReliabilityValue? reliability = null,
            bool? httpErrorAsException = null)
        {
            return new SendGridAccessInfo(clientId: clientId, apiKey: apiKey, isDefault: isDefault, requestHeaders: requestHeaders, host: host, version: version, urlPath: urlPath, auth: auth, reliability: reliability, httpErrorAsException: httpErrorAsException);
        }
    }
}
