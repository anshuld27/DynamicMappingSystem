namespace DynamicMappingSystem.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a registry that manages mappings between source and target types.
    /// </summary>
    public interface IMappingRegistry
    {
        /// <summary>
        /// Registers a mapping function for converting objects of the source type to the target type.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <param name="mappingFunc">The function that performs the mapping.</param>
        void Register<TSource, TTarget>(Func<TSource, TTarget> mappingFunc);

        /// <summary>
        /// Retrieves the mapping function for converting objects from the specified source type to the target type.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>
        /// A function that performs the mapping, or <c>null</c> if no mapping is registered for the specified types.
        /// </returns>
        Func<object, object>? GetMappingFunc(Type sourceType, Type targetType);
    }
}