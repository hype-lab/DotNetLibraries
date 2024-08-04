using HypeLab.RxPatternsResolver.Models;
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
        /// <param name="email"></param>
        /// <param name="checkDomain"></param>
        Task<EmailCheckerResponse> IsValidEmailAsync(string email, bool checkDomain = false);

        /// <summary>
        /// Checks if given input string is a valid email address
        /// </summary>
        /// <param name="email"></param>
        EmailCheckerResponse IsValidEmail(string email);

        /// <summary>
        /// Checks if given email address is existing
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        EmailCheckerResponse IsEmailExisting(string email);

        /// <summary>
        /// Checks if given email address is existing
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<EmailCheckerResponse> IsEmailExistingAsync(string email);
    }
}
