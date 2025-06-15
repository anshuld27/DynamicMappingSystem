using DynamicMappingSystem.Core.Exceptions;
using DynamicMappingSystem.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace DynamicMappingSystem.Core.Services
{
    /// <summary>
    /// Provides functionality to handle object mapping between different types.
    /// </summary>
    public class MapHandler : IMapHandler
    {
        private readonly IMappingRegistry _mappingRegistry;
        private readonly ITypeResolver _typeResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapHandler"/> class.
        /// </summary>
        /// <param name="mappingRegistry">The registry that manages mappings between types.</param>
        /// <param name="typeResolver">The resolver used to resolve type aliases.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="mappingRegistry"/> or <paramref name="typeResolver"/> is null.
        /// </exception>
        public MapHandler(IMappingRegistry mappingRegistry, ITypeResolver typeResolver)
        {
            _mappingRegistry = mappingRegistry ?? throw new ArgumentNullException(nameof(mappingRegistry));
            _typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
        }

        /// <summary>
        /// Maps an object from a source type to a target type.
        /// </summary>
        /// <param name="data">The object to be mapped.</param>
        /// <param name="sourceType">The alias of the source type.</param>
        /// <param name="targetType">The alias of the target type.</param>
        /// <returns>The mapped object of the target type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sourceType"/> or <paramref name="targetType"/> is empty or invalid.</exception>
        /// <exception cref="MappingException">Thrown if the mapping operation fails.</exception>
        [return: NotNull]
        public object Map([NotNull] object data, string sourceType, string targetType)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(sourceType)) throw new ArgumentException("Source type cannot be empty", nameof(sourceType));
            if (string.IsNullOrWhiteSpace(targetType)) throw new ArgumentException("Target type cannot be empty", nameof(targetType));

            try
            {
                // Resolve the source and target types
                var sourceTypeObj = ResolveType(sourceType);
                var targetTypeObj = ResolveType(targetType);

                // Validate that the data matches the source type
                ValidateDataType(data, sourceTypeObj, sourceType);

                // Retrieve the mapping function
                var mappingFunc = GetMappingFunction(sourceTypeObj, targetTypeObj, sourceType, targetType);

                // Execute the mapping function
                return ExecuteMapping(mappingFunc, data);
            }
            catch (MappingSystemException)
            {
                throw; // Preserve custom exceptions
            }
            catch (ArgumentException)
            {
                throw; // Preserve argument exceptions
            }
            catch (Exception ex)
            {
                throw new MappingException("Mapping operation failed", ex);
            }
        }

        /// <summary>
        /// Resolves a type based on its alias.
        /// </summary>
        /// <param name="typeAlias">The alias of the type to resolve.</param>
        /// <returns>The resolved <see cref="Type"/>.</returns>
        /// <exception cref="MappingException">Thrown if type resolution fails.</exception>
        private Type ResolveType(string typeAlias)
        {
            try
            {
                return _typeResolver.Resolve(typeAlias);
            }
            catch (TypeAliasNotFoundException)
            {
                throw; // Preserve original exception
            }
            catch (Exception ex)
            {
                throw new MappingException($"Type resolution failed for '{typeAlias}'", ex);
            }
        }

        /// <summary>
        /// Validates that the provided data matches the expected source type.
        /// </summary>
        /// <param name="data">The data to validate.</param>
        /// <param name="expectedType">The expected type of the data.</param>
        /// <param name="sourceType">The alias of the source type.</param>
        /// <exception cref="ArgumentException">Thrown if the data does not match the expected type.</exception>
        private void ValidateDataType(object data, Type expectedType, string sourceType)
        {
            if (!expectedType.IsInstanceOfType(data))
            {
                throw new ArgumentException(
                    $"Data type '{data.GetType().Name}' does not match expected type '{sourceType}'",
                    nameof(data));
            }
        }

        /// <summary>
        /// Retrieves the mapping function for the specified source and target types.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="sourceAlias">The alias of the source type.</param>
        /// <param name="targetAlias">The alias of the target type.</param>
        /// <returns>The mapping function.</returns>
        /// <exception cref="MappingNotFoundException">Thrown if no mapping function is found.</exception>
        /// <exception cref="MappingException">Thrown if retrieving the mapping function fails.</exception>
        private Func<object, object> GetMappingFunction(Type sourceType, Type targetType, string sourceAlias, string targetAlias)
        {
            try
            {
                return _mappingRegistry.GetMappingFunc(sourceType, targetType)
                    ?? throw new MappingNotFoundException(sourceAlias, targetAlias);
            }
            catch (MappingNotFoundException)
            {
                throw; // Preserve original exception
            }
            catch (Exception ex)
            {
                throw new MappingException(
                    $"Failed to retrieve mapping for {sourceAlias} → {targetAlias}", ex);
            }
        }

        /// <summary>
        /// Executes the mapping function on the provided data.
        /// </summary>
        /// <param name="mappingFunc">The mapping function to execute.</param>
        /// <param name="data">The data to map.</param>
        /// <returns>The mapped object.</returns>
        /// <exception cref="MappingException">Thrown if the mapping function fails or returns null.</exception>
        private object ExecuteMapping(Func<object, object> mappingFunc, object data)
        {
            try
            {
                return mappingFunc(data)
                    ?? throw new MappingException("Mapping function returned null");
            }
            catch (Exception ex)
            {
                throw new MappingException("Mapping execution failed", ex);
            }
        }
    }
}