using HypeLab.MailEngine.Data.Enums;
using SendGrid.Helpers.Reliability;
using System.Net.Http.Headers;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials
{
    /// <summary>
    /// Represents the access info for SendGrid.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="apiKey"></param>
    /// <param name="isDefault"></param>
    /// <param name="requestHeaders"></param>
    /// <param name="reliabilitySettings"></param>
    /// <param name="host"></param>
    /// <param name="version"></param>
    /// <param name="urlPath"></param>
    /// <param name="auth"></param>
    /// <param name="httpErrorAsException"></param>
    public class SendGridAccessInfo(
        string clientId,
        string apiKey,
        bool? isDefault = null,
        Dictionary<string, string>? requestHeaders = null,
        ReliabilitySettings? reliabilitySettings = null,
        string? host = null,
        string? version = null,
        string? urlPath = null,
        AuthHeaderValue? auth = null,
        bool? httpErrorAsException = null) : IMailAccessInfo
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
        /// The request headers to use on HTTP Requests.
        /// </summary>
        public Dictionary<string, string>? RequestHeaders { get; set; } = requestHeaders;

        /// <summary>
        /// The reliability settings to use on HTTP Requests.
        /// </summary>
        public ReliabilitySettings? ReliabilitySettings { get; set; } = reliabilitySettings;

        /// <summary>
        /// The base URL.
        /// </summary>
        public string? Host { get; set; } = host;

        /// <summary>
        /// The API version.
        /// </summary>
        public string? Version { get; set; } = version;

        /// <summary>
        /// The path to the API endpoint.
        /// </summary>
        public string? UrlPath { get; set; } = urlPath;

        /// <summary>
        /// The Auth header value.
        /// </summary>
        public AuthHeaderValue? Auth { get; set; } = auth;

        /// <summary>
        /// Gets or sets a value indicating whether HTTP error responses should be raised as exceptions. Default is false.
        /// </summary>
        public bool HttpErrorAsException { get; set; } = httpErrorAsException ?? false;


        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ClientId: {ClientId}, IsDefault: {IsDefault}, EmailSenderType: {EmailSenderType}, Host: {Host}, Version: {Version}, UrlPath: {UrlPath}, Auth: {Auth}, HttpErrorAsException: {HttpErrorAsException}";
        }

        /// <summary>
        /// Creates a new instance of the SendGridAccessInfo.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="apiKey"></param>
        /// <param name="isDefault"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="reliabilitySettings"></param>
        /// <param name="host"></param>
        /// <param name="version"></param>
        /// <param name="urlPath"></param>
        /// <param name="auth"></param>
        /// <param name="httpErrorAsException"></param>
        /// <returns></returns>
        public static SendGridAccessInfo Create(
            string clientId,
            string apiKey,
            bool? isDefault = null,
            Dictionary<string, string>? requestHeaders = null,
            ReliabilitySettings? reliabilitySettings = null,
            string? host = null,
            string? version = null,
            string? urlPath = null,
            AuthHeaderValue? auth = null,
            bool? httpErrorAsException = null)
        {
            return new SendGridAccessInfo(clientId, apiKey, isDefault, requestHeaders, reliabilitySettings, host, version, urlPath, auth, httpErrorAsException);
        }
    }

    /// <summary>
    /// Represents the auth header value.
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="parameter"></param>
    public class AuthHeaderValue(string scheme, string? parameter = null)
    {
        /// <summary>
        /// The auth scheme.
        /// </summary>
        public string Scheme { get; } = scheme;
        /// <summary>
        /// Auth parameter.
        /// </summary>
        public string? Parameter { get; } = parameter;
    }
}
