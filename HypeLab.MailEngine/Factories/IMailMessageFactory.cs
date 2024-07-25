using HypeLab.MailEngine.Data.Models.Impl.Base;

namespace HypeLab.MailEngine.Factories
{
    /// <summary>
    /// The factory that resolves the IMailMessage implementation.
    /// </summary>
    public interface IMailMessageFactory
    {
        /// <summary>
        /// Returns the concrete class implementation of IMailMessage.
        /// </summary>
        /// <returns></returns>
        IMailMessage CreateMailMessage(IMailMessage msg);
    }
}
