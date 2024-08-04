namespace HypeLab.BackgroundTasks.Helpers.Const
{
    public static class ExceptionDefaults
    {
        public static class BackgroundProcessingError
        {
            public const string DebuggerDisplay = "BackgroundProcessingException: {Message}";
            public const string DefaultMessage = "An error occurred while processing the background task.";
        }
    }
}
