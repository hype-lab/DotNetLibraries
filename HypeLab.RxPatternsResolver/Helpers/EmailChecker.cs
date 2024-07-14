using HypeLab.RxPatternsResolver.Enums;
using HypeLab.RxPatternsResolver.Interfaces;
using HypeLab.RxPatternsResolver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Helpers
{
    internal class EmailChecker : IEmailValidityChecker, IEmailDomainChecker, IEmailExistanceChecker
    {
        public bool EmailExists()
        {
            // todo
            return true;
        }

        public bool IsValidEmailAddress(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        public async Task<EmailCheckerResponseStatus> IsDomainValidAsync(string checkUrl)
        {
            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(checkUrl).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                EmailCheckerApiResponse apiResponse =
                    JsonConvert.DeserializeObject<EmailCheckerApiResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                if (apiResponse.Status != 0)
                    return EmailCheckerResponseStatus.DOMAIN_NOT_VALID;

                return EmailCheckerResponseStatus.DOMAIN_VALID;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
