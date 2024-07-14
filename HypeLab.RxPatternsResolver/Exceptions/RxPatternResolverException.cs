using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HypeLab.RxPatternsResolver.Exceptions
{
    /// <summary>
    /// class
    /// </summary>
    public class RxPatternResolverException : Exception
    {
        /// <summary>
        /// param
        /// </summary>
        public string? ResourceName { get; }

        /// <summary>
        /// param
        /// </summary>
        public ICollection<string>? Errors { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public RxPatternResolverException() { }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        public RxPatternResolverException(string? message)
            : base(message) { }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resourceName"></param>
        public RxPatternResolverException(string? message, string resourceName) : base(message)
        {
            ResourceName = resourceName;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="resourceName"></param>
        /// <param name="errors"></param>
        public RxPatternResolverException(string? message, Exception? innerException, string? resourceName, ICollection<string> errors) : base(message, innerException)
        {
            ResourceName = resourceName;
            Errors = errors;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public RxPatternResolverException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
