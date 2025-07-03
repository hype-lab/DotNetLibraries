namespace HypeLab.IO.Core.Helpers
{
    /// <summary>
    /// Provides utility methods for validating common data types.
    /// </summary>
    /// <remarks>This class contains static helper methods for performing validation checks, such as
    /// determining whether an object is null or empty. It is designed to simplify common validation
    /// scenarios.</remarks>
    public static class ValidatorHelper
    {
        /// <summary>
        /// Determines whether the specified value is null, empty, or represents a default state.
        /// </summary>
        /// <param name="value">The value to evaluate. Can be an object, string, enumerable, <see cref="DateTime"/>, or <see cref="Guid"/>.</param>
        /// <returns><see langword="true"/> if the value is <see langword="null"/>, an empty string, a string containing only
        /// whitespace, an empty enumerable, a default <see cref="DateTime"/>, or an empty <see cref="Guid"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool IsNullOrEmpty(object? value)
        {
            return value == null ||
                   (value is string s && string.IsNullOrWhiteSpace(s)) ||
                   (value is IEnumerable<object> enumerable && !enumerable.Any()) ||
                   (value is DateTime dt && dt == default) ||
                   (value is Guid guid && guid == Guid.Empty);
        }
    }
}
