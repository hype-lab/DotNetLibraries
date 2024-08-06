using HypeLab.DnsLookupClient;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.DnsLookupClient.Helpers.Const;
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
        private readonly ILookupClient _lookupClient;

        internal EmailChecker(ILookupClient? lookupClient = null)
        {
            _lookupClient = lookupClient ?? new LookupClient();
        }

        /// <summary>
        /// Checks if the email address exists
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            try
            {
                // Extract the domain part of the email address
                string domain = email.GetDomain();

                // Get MX records for the domain
                List<string> mxRecords = await _lookupClient.GetMxRecordsAsync(domain).ConfigureAwait(false);

                if (mxRecords == null || mxRecords.Count == 0)
                    return false;

                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(mxRecords[0], 25).ConfigureAwait(false);
                await using NetworkStream networkStream = tcpClient.GetStream();
                using StreamReader reader = new StreamReader(networkStream);
                await using StreamWriter writer = new StreamWriter(networkStream) { AutoFlush = true };

                await reader.ReadLineAsync().ConfigureAwait(false);
                await writer.WriteLineAsync($"{SmtpDefaults.SmtpHeloCommand} {Dns.GetHostName()}").ConfigureAwait(false);
                await reader.ReadLineAsync().ConfigureAwait(false);
                await writer.WriteLineAsync(string.Format(SmtpDefaults.SmtpMailFromCommand, InternalInfoDefaults.EmailCheckerEmailFrom)).ConfigureAwait(false);
                await reader.ReadLineAsync().ConfigureAwait(false);
                await writer.WriteLineAsync(string.Format(SmtpDefaults.SmtpRcptToCommand, email)).ConfigureAwait(false);
                string response = await reader.ReadLineAsync().ConfigureAwait(false);

                await writer.WriteLineAsync(SmtpDefaults.SmtpQuitCommand).ConfigureAwait(false);

                return response.StartsWith("250");
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
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            try
            {
                // Extract the domain part of the email address
                string domain = email.GetDomain();

                // Get MX records for the domain
                List<string> mxRecords = _lookupClient.GetMxRecords(domain);

                if (mxRecords == null || mxRecords.Count == 0)
                    return false;

                // Try to connect to the first MX record
                using TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(mxRecords[0], 25);
                using NetworkStream networkStream = tcpClient.GetStream();
                using StreamReader reader = new StreamReader(networkStream);
                using StreamWriter writer = new StreamWriter(networkStream) { AutoFlush = true };

                reader.ReadLine();
                writer.WriteLine($"{SmtpDefaults.SmtpHeloCommand} {Dns.GetHostName()}");
                reader.ReadLine();
                writer.WriteLine(string.Format(SmtpDefaults.SmtpMailFromCommand, InternalInfoDefaults.EmailCheckerEmailFrom));
                reader.ReadLine();
                writer.WriteLine(string.Format(SmtpDefaults.SmtpRcptToCommand, email));
                string response = reader.ReadLine();

                writer.WriteLine(SmtpDefaults.SmtpQuitCommand);

                return response.StartsWith("250");
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
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

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
                List<string> mxRecords = await _lookupClient.GetMxRecordsAsync(domain).ConfigureAwait(false);
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
                List<string> mxRecords = _lookupClient.GetMxRecords(domain);
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
    }
}
