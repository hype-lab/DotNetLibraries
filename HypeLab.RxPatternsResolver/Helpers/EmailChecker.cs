using HypeLab.DnsLookupClient;
using HypeLab.DnsLookupClient.Data.Clients;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.RxPatternsResolver.Constants;
using HypeLab.RxPatternsResolver.Interfaces;
using HypeLab.RxPatternsResolver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Helpers
{
    internal class EmailChecker : IEmailChecker
    {
        private readonly HttpClient _httpClient;
        private readonly HypeLabSmtpClient _smtpClient;
        private readonly ILookupClient _lookupClient;

        internal EmailChecker(HttpClient? httpClient = null, HypeLabSmtpClient? smtpClient = null, ILookupClient? lookupClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _smtpClient = smtpClient ?? new HypeLabSmtpClient();
            _lookupClient = lookupClient ?? new LookupClient();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                // Extract the domain part of the email address
                string domain = email.GetDomain();

                // Get MX records for the domain
                List<string> mxRecords = await _lookupClient.GetMXRecordsAsync(domain).ConfigureAwait(false);

                if (mxRecords == null || mxRecords.Count == 0)
                    return false;

                // Try to connect to the first MX record
                _smtpClient.Host = mxRecords[0];
                _smtpClient.Timeout = 5000; // Set a timeout for the connection
                await _smtpClient.SendMailAsync(InternalInfoDefaults.EmailCheckerEmailFrom, email, InternalInfoDefaults.EmailCheckerEmailSubject, InternalInfoDefaults.EmailCheckerEmailSubject).ConfigureAwait(false);

                return true;
            }
            catch (SmtpException ex)
            {
                throw new SmtpException($"[SmtpException] Error while checking if email '{email}' exists.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while checking if email '{email}' exists.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
            }
        }

        public bool EmailExists(string email)
        {
            try
            {
                // Extract the domain part of the email address
                string domain = email.GetDomain();

                // Get MX records for the domain
                List<string> mxRecords = _lookupClient.GetMXRecords(domain);

                if (mxRecords == null || mxRecords.Count == 0)
                    return false;

                // Try to connect to the first MX record
                _smtpClient.Host = mxRecords[0];
                _smtpClient.Timeout = 5000; // Set a timeout for the connection
                _smtpClient.Send(InternalInfoDefaults.EmailCheckerEmailFrom, email, InternalInfoDefaults.EmailCheckerEmailSubject, InternalInfoDefaults.EmailCheckerEmailSubject);

                return true;
            }
            catch (SmtpException ex)
            {
                throw new SmtpException($"[SmtpException] Error while checking if email '{email}' exists.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while checking if email '{email}' exists.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
            }
        }

        public bool IsValidEmailAddress(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        public async Task<EmailCheckerResponseStatus> IsDomainValidAsync(string checkUrl)
        {
            string? content = null;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(checkUrl).ConfigureAwait(false);
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                EmailCheckerApiResponse apiResponse =
                    JsonConvert.DeserializeObject<EmailCheckerApiResponse>(content);

                return apiResponse.Status == 0
                    ? EmailCheckerResponseStatus.DOMAIN_VALID
                    : EmailCheckerResponseStatus.DOMAIN_NOT_VALID;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException($"Error checking domain.\n{content}\n{ex.Message}", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Error checking domain.\n{content}\n{ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking domain.\n{content}\n{ex.Message}", ex);
            }
        }

        #region dispose
        ~EmailChecker()
        {
            Dispose(false);
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _httpClient?.Dispose();
                _smtpClient?.Dispose();
                _lookupClient?.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}
