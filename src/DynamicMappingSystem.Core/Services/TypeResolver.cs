using DynamicMappingSystem.Core.Exceptions;
using DynamicMappingSystem.Core.Interfaces;

namespace DynamicMappingSystem.Core.Services
{
    /// <summary>
    /// Provides functionality to resolve types using aliases.
    /// </summary>
    public class TypeResolver : ITypeResolver
    {
        // Dictionary to store type aliases and their corresponding types.
        private readonly Dictionary<string, Type> _aliases = new();

        /// <summary>
        /// Registers an alias for a specific type.
        /// </summary>
        /// <param name="alias">The alias to associate with the type.</param>
        /// <param name="type">The type to be associated with the alias.</param>
        /// <exception cref="ArgumentException">Thrown if the alias is null, empty, or whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the type is null.</exception>
        public void RegisterAlias(string alias, Type type)
        {
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentException("Alias cannot be empty", nameof(alias));

            _aliases[alias] = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Resolves a type based on its registered alias.
        /// </summary>
        /// <param name="alias">The alias of the type to resolve.</param>
        /// <returns>The resolved <see cref="Type"/> associated with the alias.</returns>
        /// <exception cref="TypeAliasNotFoundException">Thrown if the alias is not registered.</exception>
        public Type Resolve(string alias)
        {
            if (_aliases.TryGetValue(alias, out Type? type))
                return type;

            throw new TypeAliasNotFoundException(alias);
        }
    }
}