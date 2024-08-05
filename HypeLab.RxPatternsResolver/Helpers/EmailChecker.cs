using HypeLab.DnsLookupClient;
using HypeLab.DnsLookupClient.Data.Clients;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.RxPatternsResolver.Constants;
using HypeLab.RxPatternsResolver.Interfaces;
using HypeLab.RxPatternsResolver.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Helpers
{
    internal class EmailChecker : IEmailChecker
    {
        private readonly HttpClient _httpClient;
        private readonly HypeLabTcpClient _tcpClient;
        private readonly ILookupClient _lookupClient;

        internal EmailChecker(HttpClient? httpClient = null, HypeLabTcpClient? tcpClient = null, ILookupClient? lookupClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _tcpClient = tcpClient ?? new HypeLabTcpClient();
            _lookupClient = lookupClient ?? new LookupClient();
        }

        /// <summary>
        /// Checks if the email address exists
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="InvalidOperationException"></exception>
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
                await _tcpClient.ConnectAsync(mxRecords[0], 25).ConfigureAwait(false);
                await using NetworkStream networkStream = _tcpClient.GetStream();
                using StreamReader reader = new StreamReader(networkStream);
                await using StreamWriter writer = new StreamWriter(networkStream) { AutoFlush = true };
                // Read server response
                await reader.ReadLineAsync().ConfigureAwait(false);

                // Send HELO command
                await writer.WriteLineAsync($"HELO {Dns.GetHostName()}").ConfigureAwait(false);
                await reader.ReadLineAsync().ConfigureAwait(false);

                // Send MAIL FROM command
                await writer.WriteLineAsync($"MAIL FROM:<{InternalInfoDefaults.EmailCheckerEmailFrom}>").ConfigureAwait(false);
                await reader.ReadLineAsync().ConfigureAwait(false);

                // Send RCPT TO command
                await writer.WriteLineAsync($"RCPT TO:<{email}>").ConfigureAwait(false);
                string response = await reader.ReadLineAsync().ConfigureAwait(false);

                // Check if the response indicates the email exists
                if (response.StartsWith("250"))
                {
                    // Send QUIT command
                    await writer.WriteLineAsync("QUIT").ConfigureAwait(false);
                    return true;
                }
                else
                {
                    // Send QUIT command
                    await writer.WriteLineAsync("QUIT").ConfigureAwait(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while checking if email '{email}' exists.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if the email address exists
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="SmtpException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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
                _tcpClient.Connect(mxRecords[0], 25);
                using NetworkStream networkStream = _tcpClient.GetStream();
                using StreamReader reader = new StreamReader(networkStream);
                using StreamWriter writer = new StreamWriter(networkStream) { AutoFlush = true };
                // Read server response
                reader.ReadLine();

                // Send HELO command
                writer.WriteLine($"HELO {Dns.GetHostName()}");
                reader.ReadLine();

                // Send MAIL FROM command
                writer.WriteLine($"MAIL FROM:<{InternalInfoDefaults.EmailCheckerEmailFrom}>");
                reader.ReadLine();

                // Send RCPT TO command
                writer.WriteLine($"RCPT TO:<{email}>");
                string response = reader.ReadLine();

                // Check if the response indicates the email exists
                if (response.StartsWith("250"))
                {
                    // Send QUIT command
                    writer.WriteLine("QUIT");
                    return true;
                }
                else
                {
                    // Send QUIT command
                    writer.WriteLine("QUIT");
                    return false;
                }
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

        /// <summary>
        /// Checks if the domain is valid
        /// </summary>
        /// <param name="domain"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<EmailCheckerResponseStatus> IsDomainValidAsync(string domain)
        {
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentNullException(nameof(domain));

            try
            {
                List<string> mxRecords = await _lookupClient.GetMXRecordsAsync(domain).ConfigureAwait(false);
                return mxRecords?.Count > 0
                    ? EmailCheckerResponseStatus.DOMAIN_VALID
                    : EmailCheckerResponseStatus.DOMAIN_NOT_VALID;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException($"Error checking domain.\n{ex.Message}", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Error checking domain.\n{ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking domain.\n{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if the domain is valid
        /// </summary>
        /// <param name="domain"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public EmailCheckerResponseStatus IsDomainValid(string domain)
        {
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentNullException(nameof(domain));

            try
            {
                List<string> mxRecords = _lookupClient.GetMXRecords(domain);
                return mxRecords?.Count > 0
                    ? EmailCheckerResponseStatus.DOMAIN_VALID
                    : EmailCheckerResponseStatus.DOMAIN_NOT_VALID;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException($"Error checking domain.\n{ex.Message}", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Error checking domain.\n{ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking domain.\n{ex.Message}", ex);
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
                _tcpClient?.Dispose();
                _lookupClient?.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}
