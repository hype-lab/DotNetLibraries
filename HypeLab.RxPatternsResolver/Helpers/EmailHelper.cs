using HypeLab.RxPatternsResolver.Enums;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace HypeLab.RxPatternsResolver.Helpers
{
    internal struct EmailHelperConst
    {
        internal const string UrlAuthority = "https://";
        internal const string DnsGoogleDomain = "dns.google.com";
        internal const string DnsGoogleQueryParams = "/resolve?name={0}&type={1}";
    }

    internal static class EmailHelper
    {
        internal const string RequestUrl = EmailHelperConst.UrlAuthority + EmailHelperConst.DnsGoogleDomain + EmailHelperConst.DnsGoogleQueryParams;

        internal static string GetDomain(this string email)
        {
            return new MailAddress(email).Host;
        }

        internal static string RetrieveRequestUrlWithGivenDomain(string domain, DnsDomainType domainType = DnsDomainType.MX)
        {
            return string.Format(RequestUrl, domain, domainType);
        }
    }
}
