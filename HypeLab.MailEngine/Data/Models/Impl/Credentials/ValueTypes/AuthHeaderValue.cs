namespace HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes
{
    /// <summary>
    /// Represents an HTTP authentication header value, consisting of a scheme and an optional parameter.
    /// </summary>
    /// <remarks>This struct is commonly used to specify authentication information in HTTP requests. The <see
    /// cref="Scheme"/> property defines the authentication scheme (e.g., "Basic", "Bearer"),  while the <see
    /// cref="Parameter"/> property contains the associated credentials or token.</remarks>
    public struct AuthHeaderValue
    {
        /// <summary>
        /// Gets or sets the authentication scheme.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets the parameter value used for the operation.
        /// </summary>
        public string? Parameter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthHeaderValue"/> class with the specified authentication
        /// scheme and optional parameter.
        /// </summary>
        /// <remarks>The <paramref name="scheme"/> specifies the type of authentication being used, while
        /// the <paramref name="parameter"/> provides additional data required for the scheme.</remarks>
        /// <param name="scheme">The authentication scheme to use, such as "Basic" or "Bearer". This value cannot be null or empty.</param>
        /// <param name="parameter">An optional parameter associated with the authentication scheme, such as a token or credentials. Can be
        /// null.</param>
        public AuthHeaderValue(string scheme, string? parameter = null)
        {
            Scheme = scheme;
            Parameter = parameter;
        }
    }
}
