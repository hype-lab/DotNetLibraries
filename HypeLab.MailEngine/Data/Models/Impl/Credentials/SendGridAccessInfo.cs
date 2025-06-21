using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes;
using Newtonsoft.Json;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials
{
    /// <summary>
    /// Represents the access information required to interact with the SendGrid email service.
    /// </summary>
    /// <remarks>This class encapsulates the configuration details necessary for making requests to the
    /// SendGrid API,  including authentication credentials, API endpoint details, and optional settings such as
    /// reliability  and error handling behavior. It implements the <see cref="IMailAccessInfo"/> interface.</remarks>
    public class SendGridAccessInfo : IMailAccessInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridAccessInfo"/> class, which encapsulates configuration
        /// details for accessing the SendGrid API.
        /// </summary>
        /// <remarks>This constructor allows for flexible configuration of SendGrid API access, including
        /// optional parameters for advanced scenarios. Use this class to encapsulate all necessary details for
        /// interacting with the SendGrid API.</remarks>
        /// <param name="clientId">The unique identifier for the client accessing the SendGrid API. This value cannot be null or empty.</param>
        /// <param name="apiKey">The API key used for authenticating requests to the SendGrid API. This value cannot be null or empty.</param>
        /// <param name="isDefault">A value indicating whether this instance is the default configuration. If not specified, the default value
        /// is <see langword="true"/>.</param>
        /// <param name="requestHeaders">A collection of custom headers to include in API requests. If not specified, no additional headers are
        /// included.</param>
        /// <param name="host">The base host URL for the SendGrid API. If not specified, the default SendGrid host is used.</param>
        /// <param name="version">The version of the SendGrid API to target. If not specified, the default API version is used.</param>
        /// <param name="urlPath">The URL path segment for the specific API endpoint. If not specified, no additional path is appended.</param>
        /// <param name="auth">The authentication header value to use for API requests. If not specified, the default authentication
        /// mechanism is applied.</param>
        /// <param name="reliability">The reliability settings for API requests, such as retry policies or timeout configurations. If not
        /// specified, default reliability settings are applied.</param>
        /// <param name="httpErrorAsException">A value indicating whether HTTP errors should be treated as exceptions. If not specified, the default value
        /// is <see langword="false"/>.</param>
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
        /// Gets the type of email sender.
        /// </summary>
        public EmailSenderType EmailSenderType => EmailSenderType.SendGrid;

        /// <summary>
        /// Gets a value indicating whether this instance is the default configuration.
        /// </summary>
        public bool IsDefault { get; }

        /// <summary>
        /// Gets the client ID for this email access info.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Gets the API key used for authenticating requests to the SendGrid API.
        /// </summary>
        public string ApiKey { get; }

        /// <summary>
        /// Gets a collection of custom headers to include in API requests.
        /// </summary>
        public HashSet<RequestHeaderKeyValue>? RequestHeaders { get; }

        /// <summary>
        /// Gets the reliability settings for API requests, such as retry policies or timeout configurations.
        /// </summary>
        public ReliabilityValue? Reliability { get; }

        /// <summary>
        /// Gets the base host URL for the SendGrid API.
        /// </summary>
        public string? Host { get; }

        /// <summary>
        /// Gets the version of the SendGrid API to target.
        /// </summary>
        public string? Version { get; }

        /// <summary>
        /// Gets the URL path segment for the specific API endpoint.
        /// </summary>
        public string? UrlPath { get; }

        /// <summary>
        /// Gets the authentication header value to use for API requests.
        /// </summary>
        public AuthHeaderValue? Auth { get; }

        /// <summary>
        /// Gets or sets a value indicating whether HTTP error responses should be raised as exceptions. Default is <see langword="false"/>.
        /// </summary>
        public bool HttpErrorAsException { get; }

        /// <summary>
        /// Returns a string representation of the current object in JSON format.
        /// </summary>
        /// <remarks>The JSON string is formatted with indentation for readability and excludes properties
        /// with null values.</remarks>
        /// <returns>A JSON-formatted string representing the current object.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
            });
        }

        /// <summary>
        /// Creates a new instance of <see cref="SendGridAccessInfo"/> with the specified configuration parameters.
        /// </summary>
        /// <param name="clientId">The unique identifier for the client. This value cannot be null or empty.</param>
        /// <param name="apiKey">The API key used for authenticating requests. This value cannot be null or empty.</param>
        /// <param name="isDefault">A value indicating whether this instance should be treated as the default configuration. If <see
        /// langword="true"/>, this instance is considered the default; otherwise, <see langword="false"/>.</param>
        /// <param name="requestHeaders">A collection of custom request headers to include in outgoing requests. If <see langword="null"/>, no
        /// additional headers are added.</param>
        /// <param name="host">The base host URL for the SendGrid API. If <see langword="null"/>, the default host is used.</param>
        /// <param name="version">The API version to use. If <see langword="null"/>, the default version is used.</param>
        /// <param name="urlPath">The URL path segment for API requests. If <see langword="null"/>, the default path is used.</param>
        /// <param name="auth">The authentication header value to use for requests. If <see langword="null"/>, the default authentication
        /// mechanism is applied.</param>
        /// <param name="reliability">The reliability configuration for handling requests. If <see langword="null"/>, the default reliability
        /// settings are applied.</param>
        /// <param name="httpErrorAsException">A value indicating whether HTTP errors should be treated as exceptions. If <see langword="true"/>, HTTP
        /// errors are thrown as exceptions; otherwise, they are handled gracefully.</param>
        /// <returns>A new instance of <see cref="SendGridAccessInfo"/> configured with the specified parameters.</returns>
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
