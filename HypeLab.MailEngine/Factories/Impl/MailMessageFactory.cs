using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Models.Impl.Base;

namespace HypeLab.MailEngine.Factories.Impl
{
    /// <summary>
    /// The factory that resolves the IMailMessage implementation.
    /// </summary>
    public class MailMessageFactory : IMailMessageFactory
    {
        /// <summary>
        /// Returns the concrete class implementation of IMailMessage.
        /// </summary>
        /// <returns></returns>
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
