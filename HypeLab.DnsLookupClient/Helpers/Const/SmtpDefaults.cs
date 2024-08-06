namespace HypeLab.DnsLookupClient.Helpers.Const
{
    public static class SmtpDefaults
    {
        public const string SmtpHeloCommand = "HELO";
        public const string SmtpMailFromCommand = "MAIL FROM:<{0}>";
        public const string SmtpRcptToCommand = "RCPT TO:<{0}>";
        public const string SmtpQuitCommand = "QUIT";
    }
}
