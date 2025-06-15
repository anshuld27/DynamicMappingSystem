namespace DynamicMappingSystem.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a handler that performs object mapping between different types.
    /// </summary>
    public interface IMapHandler
    {
        /// <summary>
        /// Maps an object from a source type to a target type.
        /// </summary>
        /// <param name="data">The object to be mapped.</param>
        /// <param name="sourceType">The name of the source type.</param>
        /// <param name="targetType">The name of the target type.</param>
        /// <returns>The mapped object of the target type.</returns>
        object Map(object data, string sourceType, string targetType);
    }
}