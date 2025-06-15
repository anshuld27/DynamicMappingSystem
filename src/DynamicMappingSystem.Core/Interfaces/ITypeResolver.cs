namespace DynamicMappingSystem.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for resolving types using aliases.
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Registers an alias for a specific type.
        /// </summary>
        /// <param name="alias">The alias to associate with the type.</param>
        /// <param name="type">The type to be associated with the alias.</param>
        void RegisterAlias(string alias, Type type);

        /// <summary>
        /// Resolves a type based on its registered alias.
        /// </summary>
        /// <param name="alias">The alias of the type to resolve.</param>
        /// <returns>The resolved <see cref="Type"/> associated with the alias.</returns>
        /// <exception cref="TypeAliasNotFoundException">
        /// Thrown if the alias is not registered in the resolver.
        /// </exception>
        Type Resolve(string alias);
    }
}