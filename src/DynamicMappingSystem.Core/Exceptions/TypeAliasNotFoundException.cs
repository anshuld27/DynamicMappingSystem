namespace DynamicMappingSystem.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested type alias is not found in the TypeResolver registry.
    /// </summary>
    public class TypeAliasNotFoundException : MappingSystemException
    {
        /// <summary>
        /// Gets the alias that was not found in the registry.
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeAliasNotFoundException"/> class
        /// with the specified alias that could not be resolved.
        /// </summary>
        /// <param name="alias">The type alias that was not found.</param>
        public TypeAliasNotFoundException(string alias)
            : base($"Type alias '{alias}' not registered") => Alias = alias;
    }
}