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
        /// <param name="checkUrl">The api url where the domain will be validated</param>
        Task<EmailCheckerResponseStatus> IsDomainValidAsync(string checkUrl);
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
