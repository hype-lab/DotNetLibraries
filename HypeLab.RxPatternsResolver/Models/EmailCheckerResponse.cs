using System;
using System.Collections.Generic;
using System.Text;

namespace HypeLab.RxPatternsResolver.Models
{
    /// <summary>
    /// Enum that represents the possible status could be returned
    /// </summary>
    public enum EmailCheckerResponseStatus
    {
        /// <summary>
        /// email is valid
        /// </summary>
        EMAIL_VALID,
        /// <summary>
        /// email address is valid
        /// </summary>
        EMAIL_NOT_VALID,
        /// <summary>
        /// email not exists
        /// </summary>
        EMAIL_NOT_EXISTS,
        /// <summary>
        /// domain is not valid
        /// </summary>
        DOMAIN_NOT_VALID,
        /// <summary>
        /// given input string is null or empty
        /// </summary>
        INPUT_NULL_OR_EMPTY,
        /// <summary>
        /// domain is valid
        /// </summary>
        DOMAIN_VALID,
    }

    /// <summary>
    /// Response of the email check
    /// </summary>
    public struct EmailCheckerResponse
    {
        /// <summary>
        /// Initialize it with response result items
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        public EmailCheckerResponse(string message, EmailCheckerResponseStatus status = EmailCheckerResponseStatus.EMAIL_VALID)
        {
            Message = message;
            ResponseStatus = status;
        }

        /// <summary>
        /// Response message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Status of the email after check
        /// </summary>
        public EmailCheckerResponseStatus ResponseStatus { get; }
    }
}
