using HypeLab.BackgroundTasks.Helpers.Const;
using System.Diagnostics;

namespace HypeLab.BackgroundTasks.Data.Exceptions
{
    [DebuggerDisplay(ExceptionDefaults.BackgroundProcessingError.DebuggerDisplay)]
    public class BackgroundProcessingErrorException : Exception
    {
        public BackgroundProcessingErrorException()
            : base(ExceptionDefaults.BackgroundProcessingError.DefaultMessage) { }

        public BackgroundProcessingErrorException(string? message)
            : base(message ?? ExceptionDefaults.BackgroundProcessingError.DefaultMessage) { }

        public BackgroundProcessingErrorException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.BackgroundProcessingError.DefaultMessage, innerException) { }
    }
}