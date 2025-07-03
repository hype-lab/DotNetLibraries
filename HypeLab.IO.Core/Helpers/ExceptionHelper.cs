using System.Text;

namespace HypeLab.IO.Core.Helpers
{
    /// <summary>
    /// Provides utility methods for working with exceptions.
    /// </summary>
    /// <remarks>This class contains static methods that simplify common exception handling tasks, such as
    /// retrieving detailed information about an exception and its inner exceptions.</remarks>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Retrieves the full error message from the specified exception, including messages from all inner exceptions.
        /// </summary>
        /// <remarks>This method is useful for logging or displaying detailed error information,
        /// especially when exceptions are nested.</remarks>
        /// <param name="exception">The exception from which to extract the messages. If <paramref name="exception"/> is <see langword="null"/>,
        /// an empty string is returned.</param>
        /// <returns>A string containing the message of the provided exception and all inner exception messages, separated by new
        /// lines. Returns an empty string if <paramref name="exception"/> is <see langword="null"/>.</returns>
        public static string GetFullMessage(this Exception exception)
        {
            if (exception == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(exception.Message);

            Exception inner = exception.InnerException;
            while (inner != null)
            {
                sb.AppendLine(inner.Message);
                inner = inner.InnerException;
            }

            return sb.ToString();
        }
    }
}
