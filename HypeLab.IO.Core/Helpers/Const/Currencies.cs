namespace HypeLab.IO.Core.Helpers.Const
{
    /// <summary>
    /// Provides a collection of constants representing currency symbols, including both fiat currencies and
    /// cryptocurrencies, as well as a predefined array of all available symbols.
    /// </summary>
    /// <remarks>This class is designed to offer a standardized set of currency symbols for use in
    /// applications that require currency representation, formatting, or display. It includes symbols for major fiat
    /// currencies such as the Euro (€), US Dollar ($), and Japanese Yen (¥), as well as popular cryptocurrencies like
    /// Bitcoin (₿) and Ethereum (Ξ). <para> The <see cref="All"/> field provides an array containing all the currency
    /// symbols defined in this class, which can be useful for iterating over or validating supported currencies.
    /// </para></remarks>
    public static class Currencies
    {
        /// <summary>
        /// Represents an array containing all the currency symbols defined in this class.
        /// </summary>
        public readonly static string[] All =
        [
            Euro,
            US_Dollar,
            British_Pound,
            Japanese_Yen,
            Swiss_Franc,
            Indian_Rupee,
            Canadian_Dollar,
            Australian_Dollar,
            Chinese_Yuan,
            South_Korean_Won,
            Brazilian_Real,
            Bitcoin,
            Ethereum,
            Litecoin
        ];

        /// <summary>
        /// Represents the Euro currency symbol.
        /// </summary>
        public const string Euro = "€";
        /// <summary>
        /// Represents the US Dollar currency symbol.
        /// </summary>
        public const string US_Dollar = "$";
        /// <summary>
        /// Represents the British Pound currency symbol.
        /// </summary>
        public const string British_Pound = "£";
        /// <summary>
        /// Represents the Japanese Yen currency symbol.
        /// </summary>
        public const string Japanese_Yen = "¥";
        /// <summary>
        /// Represents the Swiss Franc currency symbol.
        /// </summary>
        public const string Swiss_Franc = "CHF";
        /// <summary>
        /// Represents the symbol for the Indian Rupee currency.
        /// </summary>
        public const string Indian_Rupee = "₹";
        /// <summary>
        /// Represents the symbol for the Canadian Dollar currency.
        /// </summary>
        /// <remarks>This constant can be used in applications that require a standardized representation
        /// of the Canadian Dollar currency symbol.</remarks>
        public const string Canadian_Dollar = "C$";
        /// <summary>
        /// Represents the symbol for the Australian Dollar currency.
        /// </summary>
        /// <remarks>This constant can be used to display or format monetary values in Australian
        /// Dollars.</remarks>
        public const string Australian_Dollar = "A$";
        /// <summary>
        /// Represents the currency symbol for the Chinese Yuan.
        /// </summary>
        /// <remarks>The Chinese Yuan uses the same symbol (<c>¥</c>) as the Japanese Yen, but the two are
        /// contextually distinct. Ensure the appropriate context is applied when using this symbol.</remarks>
        public const string Chinese_Yuan = "¥"; // Note: Chinese Yuan uses the same symbol as Japanese Yen, but contextually different.
        /// <summary>
        /// Represents the symbol for the South Korean Won currency.
        /// </summary>
        /// <remarks>This constant can be used to display or format monetary values in South Korean
        /// Won.</remarks>
        public const string South_Korean_Won = "₩";
        /// <summary>
        /// Represents the currency symbol for the Brazilian Real.
        /// </summary>
        public const string Brazilian_Real = "R$";

        // Cryptocurrency symbols
        /// <summary>
        /// Represents the symbol for Bitcoin cryptocurrency.
        /// </summary>
        /// <remarks>This constant provides the Unicode symbol for Bitcoin (₿), which can be used in
        /// applications that display or process cryptocurrency-related information.</remarks>
        public const string Bitcoin = "₿";
        /// <summary>
        /// Represents the symbol for the Ethereum cryptocurrency.
        /// </summary>
        /// <remarks>The value of this constant is "Ξ", which is commonly used as the symbol for
        /// Ethereum.</remarks>
        public const string Ethereum = "Ξ";
        /// <summary>
        /// Represents the symbol for Litecoin cryptocurrency.
        /// </summary>
        /// <remarks>This constant can be used to display or reference the Litecoin currency
        /// symbol.</remarks>
        public const string Litecoin = "Ł";
    }
}
