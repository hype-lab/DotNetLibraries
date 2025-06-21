using HypeLab.MailEngine.Data.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes
{
    /// <summary>
    /// Represents the configuration details for an SMTP client certificate, including the certificate file path,
    /// optional password, and optional key storage flags.
    /// </summary>
    /// <remarks>This struct is used to encapsulate the necessary information for loading and using an SMTP
    /// client certificate.  The <see cref="KeyStorageFlagsEnum"/> property provides a validated representation of the
    /// key storage flags  as an <see cref="X509KeyStorageFlags"/> enumeration.</remarks>
    public struct SmtpClientCertValue : IEquatable<SmtpClientCertValue>
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
        /// Gets or sets the file name of the certificate.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the password for the certificate, if required.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string" /> representation of the key storage flags.
        /// </summary>
        public string? KeyStorageFlags { get; set; }

        /// <summary>
        /// Gets the <see cref="X509KeyStorageFlags"/> enumeration value representing the key storage flags.
        /// </summary>
        public readonly X509KeyStorageFlags? KeyStorageFlagsEnum
        {
            get
            {
                return ValidateKeyStorageFlags(KeyStorageFlags);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpClientCertValue"/> class, representing an SMTP client
        /// certificate.
        /// </summary>
        /// <remarks>Use this constructor to create an instance of <see cref="SmtpClientCertValue"/> when
        /// configuring an SMTP client to use a specific certificate for authentication. Ensure that the <paramref
        /// name="fileName"/> points to a valid certificate file.</remarks>
        /// <param name="fileName">The file path to the certificate. This parameter cannot be null or empty.</param>
        /// <param name="password">The password used to access the certificate, if required. This parameter can be null if the certificate does
        /// not require a password.</param>
        /// <param name="keyStorageFlags">Optional key storage flags that specify how and where the certificate's private key is stored. This
        /// parameter can be null if no specific storage flags are needed.</param>
        public SmtpClientCertValue(string fileName, string? password = null, string? keyStorageFlags = null)
        {
            FileName = fileName;
            Password = password;
            KeyStorageFlags = keyStorageFlags;
        }

        /// <summary>
        /// Generates a hash code for the current object based on its properties.
        /// </summary>
        /// <remarks>The hash code is computed using the <see cref="HashCode.Combine{T1, T2, T3}"/> method,
        /// incorporating  the values of <see cref="FileName"/>, <see cref="Password"/>, and <see
        /// cref="KeyStorageFlagsEnum"/>.  This ensures that objects with identical property values produce the same
        /// hash code.</remarks>
        /// <returns>An integer representing the hash code of the current object.</returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(FileName, Password, KeyStorageFlagsEnum);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Must be of type <see cref="SmtpClientCertValue"/> to be
        /// considered equal.</param>
        /// <returns><see langword="true"/> if the specified object is of type <see cref="SmtpClientCertValue"/> and is equal to
        /// the current instance; otherwise, <see langword="false"/>.</returns>
        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not SmtpClientCertValue smtpClientCert)
                return false;

            return Equals(smtpClientCert);
        }

        /// <summary>
        /// Determines whether the current instance is equal to the specified <see cref="SmtpClientCertValue"/>
        /// instance.
        /// </summary>
        /// <param name="other">The <see cref="SmtpClientCertValue"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the specified instance; otherwise, <see
        /// langword="false"/>. Equality is determined by comparing the <see cref="FileName"/>, <see cref="Password"/>,
        /// and <see cref="KeyStorageFlagsEnum"/> properties.</returns>
        public readonly bool Equals(SmtpClientCertValue other)
        {
            return FileName == other.FileName && Password == other.Password && KeyStorageFlagsEnum == other.KeyStorageFlagsEnum;
        }

        /// <summary>
        /// Determines whether two <see cref="SmtpClientCertValue"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="SmtpClientCertValue"/> instance to compare.</param>
        /// <param name="right">The second <see cref="SmtpClientCertValue"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="SmtpClientCertValue"/> instances are equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(SmtpClientCertValue left, SmtpClientCertValue right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="SmtpClientCertValue"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="SmtpClientCertValue"/> instance to compare.</param>
        /// <param name="right">The second <see cref="SmtpClientCertValue"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(SmtpClientCertValue left, SmtpClientCertValue right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a string representation of the current <see cref="SmtpClientCertValue"/> instance.
        /// </summary>
        /// <remarks>The returned string includes the values of the <see cref="FileName"/>, <see
        /// cref="Password"/>, and <see cref="KeyStorageFlags"/> properties, formatted for debugging or logging
        /// purposes.</remarks>
        /// <returns>A string that represents the current <see cref="SmtpClientCertValue"/> instance, including the file name,
        /// password, and key storage flags.</returns>
        public readonly override string ToString()
        {
            return $"SmtpClientCertValue: FileName={FileName}, Password={Password}, KeyStorageFlags={KeyStorageFlags}";
        }
    }
}
