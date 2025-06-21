using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Models.Impl.Base;

namespace HypeLab.MailEngine.Factories.Impl
{
    /// <summary>
    /// Provides functionality for creating specific mail message instances based on the input type.
    /// </summary>
    /// <remarks>This factory is designed to process mail messages and return instances of their specific
    /// types. It supports recognized mail message types and throws an exception for unrecognized types.</remarks>
    public class MailMessageFactory : IMailMessageFactory
    {
        /// <summary>
        /// Creates a mail message instance based on the provided input.
        /// </summary>
        /// <param name="msg">The input mail message to process. Must be an instance of a recognized mail message type.</param>
        /// <returns>The processed mail message instance, cast to its specific type.</returns>
        /// <exception cref="UnknownMailMessageTypeException">Thrown if the input mail message is not of a recognized type.</exception>
        public IMailMessage CreateMailMessage(IMailMessage msg)
        {
            if (msg is CustomMailMessage customMailMessage)
                return customMailMessage;

            if (msg is MultipleToesMailMessage multipleToesMailMessage)
                return multipleToesMailMessage;

            throw new UnknownMailMessageTypeException("Unknown mail message type.");
        }
    }
}
