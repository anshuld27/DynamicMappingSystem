using DynamicMappingSystem.Core.Interfaces;

namespace DynamicMappingSystem.Core.Services
{
    /// <summary>
    /// Provides an implementation of <see cref="IMappingRegistry"/> to manage mappings
    /// between source and target types.
    /// </summary>
    public class MappingRegistry : IMappingRegistry
    {
        // Dictionary to store mapping functions, keyed by source and target types.
        private readonly Dictionary<(Type, Type), Func<object, object>> _mappings = new();

        /// <summary>
        /// Registers a mapping function for converting objects of the source type to the target type.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <param name="mappingFunc">The function that performs the mapping.</param>
        public void Register<TSource, TTarget>(Func<TSource, TTarget> mappingFunc)
        {
            // Create a key using the source and target types.
            var key = (typeof(TSource), typeof(TTarget));

            // Store the mapping function in the dictionary, wrapping it to handle object types.
            _mappings[key] = obj => mappingFunc((TSource)obj)!;
        }

        /// <summary>
        /// Retrieves the mapping function for converting objects from the specified source type to the target type.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>
        /// A function that performs the mapping, or <c>null</c> if no mapping is registered for the specified types.
        /// </returns>
        public Func<object, object>? GetMappingFunc(Type sourceType, Type targetType)
        {
            // Attempt to retrieve the mapping function from the dictionary.
            return _mappings.TryGetValue((sourceType, targetType), out var func)
                ? func
                : null;
        }
    }
}