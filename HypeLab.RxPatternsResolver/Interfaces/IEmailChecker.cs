using HypeLab.RxPatternsResolver.Models;
using System;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Interfaces
{
    /// <summary>
    /// Interface for email checker
    /// </summary>
    public interface IEmailChecker : IDisposable
    {
        /// <summary>
        /// Check if email is valid
        /// </summary>
        /// <param name="email">The email to check</param>
        bool IsValidEmailAddress(string email);
        /// <summary>
        /// Check if domain is valid
        /// </summary>
        /// <param name="domain">The email address domain</param>
        Task<EmailCheckerResponseStatus> IsDomainValidAsync(string domain);
        /// <summary>
        /// Check if domain is valid
        /// </summary>
        /// <param name="domain">The email address domain</param>
        EmailCheckerResponseStatus IsDomainValid(string domain);
        /// <summary>
        /// Check if email exists
        /// </summary>
        /// <param name="email">The email to check</param>
        bool EmailExists(string email);
        /// <summary>
        /// Check if email exists asynchronously
        /// </summary>
        /// <param name="email">The email to check</param>
        Task<bool> EmailExistsAsync(string email);
    }
}
