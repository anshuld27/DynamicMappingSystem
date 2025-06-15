namespace DynamicMappingSystem.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested mapping between two types is not found.
    /// </summary>
    public class MappingNotFoundException : MappingSystemException
    {
        /// <summary>
        /// Gets the source type for which the mapping was requested.
        /// </summary>
        public string SourceType { get; }

        /// <summary>
        /// Gets the target type to which the mapping was requested.
        /// </summary>
        public string TargetType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingNotFoundException"/> class
        /// with the specified source and target types.
        /// </summary>
        /// <param name="source">The source type for the mapping.</param>
        /// <param name="target">The target type for the mapping.</param>
        public MappingNotFoundException(string source, string target)
            : base($"No mapping from '{source}' to '{target}'")
            => (SourceType, TargetType) = (source, target);
    }
}