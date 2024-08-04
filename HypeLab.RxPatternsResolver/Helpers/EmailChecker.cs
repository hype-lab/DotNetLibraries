using HypeLab.RxPatternsResolver.Interfaces;
using HypeLab.RxPatternsResolver.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Helpers
{
    internal class EmailChecker : IEmailChecker, IDisposable
    {
        private readonly HttpClient _httpClient;

        internal EmailChecker(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
        }

        public bool EmailExists(string email)
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
            if (_disposed) return;

            if (disposing)
                _httpClient?.Dispose();

            _disposed = true;
        }
        #endregion
    }
}
