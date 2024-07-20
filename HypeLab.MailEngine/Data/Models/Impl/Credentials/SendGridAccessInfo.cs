using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

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
        /// <param name="maximumNumberOfRetries"></param>
        /// <param name="minimumBackOff">In seconds</param>
        /// <param name="deltaBackOff">In seconds</param>
        /// <param name="maximumBackOff">In seconds</param>
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
            int? maximumNumberOfRetries = null,
            int? minimumBackOff = null,
            int? deltaBackOff = null,
            int? maximumBackOff = null,
            bool? httpErrorAsException = null)
        {
            IsDefault = isDefault ?? true;
            ClientId = clientId;
            ApiKey = apiKey;
            RequestHeaders = requestHeaders ?? [];
            MaximumNumberOfRetries = maximumNumberOfRetries;
            MinimumBackOff = minimumBackOff;
            DeltaBackOff = deltaBackOff;
            MaximumBackOff = maximumBackOff;
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
        public HashSet<RequestHeaderKeyValue> RequestHeaders { get; }

        /// <summary>
        /// The reliability settings to use on HTTP Requests.
        /// </summary>
        public int? MaximumNumberOfRetries { get; }

        /// <summary>
        /// The minimum amount of time to wait between HTTP retries.
        /// </summary>
        public int? MinimumBackOff { get; }

        /// <summary>
        /// The delta back off.
        /// </summary>
        public int? DeltaBackOff { get; }

        /// <summary>
        /// The maximum amount of time to wait between HTTP retries. Max value of 30 seconds.
        /// </summary>
        public int? MaximumBackOff { get; }

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
            return $"ClientId: {ClientId}, IsDefault: {IsDefault}, EmailSenderType: {EmailSenderType}, Host: {Host}, Version: {Version}, UrlPath: {UrlPath}, Auth: {Auth}, HttpErrorAsException: {HttpErrorAsException}";
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
        /// <param name="maximumNumberOfRetries"></param>
        /// <param name="minimumBackOff">In seconds</param>
        /// <param name="deltaBackOff">In seconds</param>
        /// <param name="maximumBackOff">In seconds</param>
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
            int? maximumNumberOfRetries = null,
            int? minimumBackOff = null,
            int? deltaBackOff = null,
            int? maximumBackOff = null,
            bool? httpErrorAsException = null)
        {
            return new SendGridAccessInfo(clientId: clientId, apiKey: apiKey, isDefault: isDefault, requestHeaders: requestHeaders, host: host, version: version, urlPath: urlPath, auth: auth, maximumNumberOfRetries: maximumNumberOfRetries, minimumBackOff: minimumBackOff, deltaBackOff: deltaBackOff, maximumBackOff: maximumBackOff, httpErrorAsException: httpErrorAsException);
        }
    }
}
