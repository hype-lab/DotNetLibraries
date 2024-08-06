using HypeLab.RxPatternsResolver.Models;
using System;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Interfaces
{
    /// <summary>
    /// Exposes a method which checks if given input string is a valid email address
    /// </summary>
    public interface IEmailValidable
    {
        /// <summary>
        /// Checks if given input string is a valid email address
        /// </summary>
        /// <param name="email">The provided email address</param>
        /// <param name="checkDomain">If true checks for domain validity</param>
        Task<EmailCheckerResponse> IsValidEmailAsync(string email, bool checkDomain = false);

        /// <summary>
        /// Checks if given input string is a valid email address
        /// </summary>
        /// <param name="email">The provided email address</param>
        EmailCheckerResponse IsValidEmail(string email);

        /// <summary>
        /// Checks if given input string is a valid email address
        /// </summary>
        /// <param name="email">The provided email address</param>
        /// <param name="checkDomain">If true checks for domain validity</param>
        EmailCheckerResponse IsValidEmail(string email, bool checkDomain);

        /// <summary>
        /// Checks if given email address is existing
        /// </summary>
        /// <param name="email">The provided email address</param>
        EmailCheckerResponse IsEmailExisting(string email);

        /// <summary>
        /// Checks if given email address is existing
        /// </summary>
        /// <param name="email">The provided email address</param>
        Task<EmailCheckerResponse> IsEmailExistingAsync(string email);
    }
}
