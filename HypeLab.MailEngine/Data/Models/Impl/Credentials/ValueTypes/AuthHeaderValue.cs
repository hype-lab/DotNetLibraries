namespace HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes
{
    /// <summary>
    /// Represents the auth header value.
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="parameter"></param>
    public struct AuthHeaderValue(string scheme, string? parameter = null)
    {
        /// <summary>
        /// The auth scheme.
        /// </summary>
        public string Scheme { get; set; } = scheme;
        /// <summary>
        /// Auth parameter.
        /// </summary>
        public string? Parameter { get; set; } = parameter;
    }
}
