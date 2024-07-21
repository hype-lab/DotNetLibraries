using HypeLab.MailEngine.Data.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes
{
    /// <summary>
    /// Represents the certificate for the SMTP client.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="password"></param>
    /// <param name="keyStorageFlags"></param>
    public struct SmtpClientCertValue(string fileName, string? password = null, string? keyStorageFlags = null) : IEquatable<SmtpClientCertValue>
    {
        private static X509KeyStorageFlags? ValidateKeyStorageFlags(string? ksf)
        {
            if (!string.IsNullOrEmpty(ksf))
            {
                if (Enum.TryParse(ksf, out X509KeyStorageFlags ksf2))
                    return ksf2;
                else
                    throw new SmtpClientCertException("Invalid key storage flags.");
            }

            return null;
        }

        /// <summary>
        /// The certificate path.
        /// </summary>
        public string FileName { get; set; } = fileName;
        /// <summary>
        /// The certificate password if provided.
        /// </summary>
        public string? Password { get; set; } = password;

        /// <summary>
        /// String representation of the key storage flags.
        /// </summary>
        public string? KeyStorageFlags { get; set; } = keyStorageFlags;
        /// <summary>
        /// Set the key storage flags if provided.
        /// </summary>
        public readonly X509KeyStorageFlags? KeyStorageFlagsEnum
        {
            get
            {
                return ValidateKeyStorageFlags(KeyStorageFlags);
            }
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(FileName, Password, KeyStorageFlagsEnum);
        }

        /// <summary>
        /// Checks if the object is equal to another object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not SmtpClientCertValue smtpClientCert)
                return false;

            return Equals(smtpClientCert);
        }

        /// <summary>
        /// Checks if the object is equal to another object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(SmtpClientCertValue other)
        {
            return FileName == other.FileName && Password == other.Password && KeyStorageFlagsEnum == other.KeyStorageFlagsEnum;
        }

        /// <summary>
        /// The operator for equality.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SmtpClientCertValue left, SmtpClientCertValue right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// The operator for inequality.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SmtpClientCertValue left, SmtpClientCertValue right)
        {
            return !(left == right);
        }
    }
}
