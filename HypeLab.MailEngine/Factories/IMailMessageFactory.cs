using HypeLab.MailEngine.Data.Models.Impl.Base;

namespace HypeLab.MailEngine.Factories
{
    /// <summary>
    /// Defines a factory for creating instances of <see cref="IMailMessage"/>.
    /// </summary>
    /// <remarks>This interface provides a method for creating new mail message instances based on an existing
    /// <see cref="IMailMessage"/> template. Implementations of this interface are responsible for initializing the
    /// properties of the new mail message using the provided input message.</remarks>
    public interface IMailMessageFactory
    {
        /// <summary>
        /// Creates a new instance of a mail message based on the specified input message.
        /// </summary>
        /// <param name="msg">The input mail message used as a template for creating the new mail message. Cannot be null.</param>
        /// <returns>A new <see cref="IMailMessage"/> instance initialized with the properties of the specified input message.</returns>
        IMailMessage CreateMailMessage(IMailMessage msg);
    }
}
