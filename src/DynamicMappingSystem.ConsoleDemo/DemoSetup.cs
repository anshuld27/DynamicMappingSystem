using DIRS21.API.Models.Google;
using DIRS21.API.Models.Internal;
using DynamicMappingSystem.Core.Interfaces;
using DynamicMappingSystem.Core.MappingProfiles;
using DynamicMappingSystem.Core.Services;

namespace DynamicMappingSystem.ConsoleDemo
{
    /// <summary>
    /// Provides setup functionality for creating and configuring the mapping system.
    /// </summary>
    public static class DemoSetup
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="IMapHandler"/>.
        /// </summary>
        /// <returns>An instance of <see cref="IMapHandler"/> configured with mapping profiles and type resolvers.</returns>
        public static IMapHandler CreateMapHandler()
        {
            // Create core components for type resolution and mapping registration
            ITypeResolver typeResolver = new TypeResolver();
            IMappingRegistry mappingRegistry = new MappingRegistry();

            // Register mapping profiles
            var googleProfile = new EnhancedGoogleMappingProfile();
            googleProfile.ConfigureTypeAliases(typeResolver); // Configure type aliases for Google mappings
            googleProfile.ConfigureMappings(mappingRegistry); // Register mappings for Google objects

            // Create and return the map handler
            return new MapHandler(mappingRegistry, typeResolver);
        }
    }

    /// <summary>
    /// Extends the <see cref="GoogleMappingProfile"/> to provide custom mapping logic for GoogleRoom and Room objects.
    /// </summary>
    public class EnhancedGoogleMappingProfile : GoogleMappingProfile
    {
        /// <summary>
        /// Maps a <see cref="Room"/> object to a <see cref="GoogleRoom"/> object.
        /// </summary>
        /// <param name="source">The source <see cref="Room"/> object to map from.</param>
        /// <returns>A <see cref="GoogleRoom"/> object mapped from the source, or null if the source is null.</returns>
        protected override GoogleRoom? MapToGoogleRoom(Room? source)
        {
            if (source == null)
                return null;

            return new GoogleRoom
            {
                RoomId = source.RoomId.ToString(),
                RoomType = source.RoomType.ToString(),
                PricePerNight = ConvertCurrency(source.PricePerNight, "EUR", "USD"),
                Capacity = source.Capacity,
                IsAvailable = source.IsAvailable,
                Description = source.Description ?? string.Empty,
                Amenities = source.Amenities ?? new List<string>()
            };
        }

        /// <summary>
        /// Maps a <see cref="GoogleRoom"/> object to a <see cref="Room"/> object.
        /// </summary>
        /// <param name="source">The source <see cref="GoogleRoom"/> object to map from.</param>
        /// <returns>A <see cref="Room"/> object mapped from the source, or null if the source is null.</returns>
        protected override Room? MapFromGoogleRoom(GoogleRoom? source)
        {
            if (source == null)
                return null;

            return new Room
            {
                RoomId = Guid.TryParse(source.RoomId, out Guid roomId) ? roomId : Guid.Empty,
                RoomType = Enum.TryParse(source.RoomType, out RoomType roomType)
                    ? roomType : throw new ArgumentException($"Invalid room type: {source.RoomType}"),
                PricePerNight = ConvertCurrency(source.PricePerNight, "USD", "EUR"),
                Capacity = source.Capacity,
                IsAvailable = source.IsAvailable,
                Description = source.Description ?? string.Empty,
                Amenities = source.Amenities ?? new List<string>()
            };
        }

        /// <summary>
        /// Converts a currency amount from one currency to another.
        /// </summary>
        /// <param name="amount">The amount to convert.</param>
        /// <param name="fromCurrency">The source currency code (e.g., "EUR").</param>
        /// <param name="toCurrency">The target currency code (e.g., "USD").</param>
        /// <returns>The converted amount.</returns>
        private decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            // Simplified currency conversion logic (real implementation would use an external API)
            if (fromCurrency == "EUR" && toCurrency == "USD")
                return amount * 1.08m; // EUR to USD

            if (fromCurrency == "USD" && toCurrency == "EUR")
                return amount * 0.93m; // USD to EUR

            return amount;
        }
    }
}