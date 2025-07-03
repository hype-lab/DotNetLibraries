namespace HypeLab.IO.Core.Helpers.Const
{
    /// <summary>
    /// Provides default values and constants used across the core library.
    /// </summary>
    /// <remarks>This class contains constants that represent commonly used attribute and namespace names in
    /// the .NET runtime, particularly for compiler-related features.</remarks>
    public static class CoreDefaults
    {
        /// <summary>
        /// Represents the fully qualified name of the RequiredMemberAttribute type in the
        /// System.Runtime.CompilerServices namespace.
        /// </summary>
        /// <remarks>This constant can be used to reference the RequiredMemberAttribute type by name, for
        /// example, in reflection-based scenarios.</remarks>
        public const string RequiredMemberAttributeName = "System.Runtime.CompilerServices.RequiredMemberAttribute";
        /// <summary>
        /// Represents the namespace for the <see langword="IsExternalInit"/> feature,  which enables support for
        /// init-only properties in C#.
        /// </summary>
        /// <remarks>This constant provides the fully qualified namespace string for the  <see
        /// langword="IsExternalInit"/> type, which is used to enable init-only  properties in C# 9.0 and later. It is
        /// primarily intended for use in  scenarios where compatibility with older frameworks or custom tooling  is
        /// required.</remarks>
        public const string IsExternalInitNamespace = "System.Runtime.CompilerServices.IsExternalInit";
    }
}
