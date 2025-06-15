namespace DynamicMappingSystem.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a mapping profile that configures mappings and type aliases.
    /// </summary>
    public interface IMappingProfile
    {
        /// <summary>
        /// Configures the mappings between source and target types.
        /// </summary>
        /// <param name="registry">The registry where mappings are registered.</param>
        void ConfigureMappings(IMappingRegistry registry);

        /// <summary>
        /// Configures type aliases to be used for resolving types.
        /// </summary>
        /// <param name="typeResolver">The type resolver used to register aliases.</param>
        void ConfigureTypeAliases(ITypeResolver typeResolver);
    }
}