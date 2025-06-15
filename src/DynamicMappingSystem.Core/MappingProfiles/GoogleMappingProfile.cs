using DynamicMappingSystem.Core.Interfaces;
using DIRS21.API.Models.Internal;
using DIRS21.API.Models.Google;

namespace DynamicMappingSystem.Core.MappingProfiles
{
    /// <summary>
    /// Provides mapping configurations and logic for converting between
    /// DIRS21 internal models and Google models.
    /// </summary>
    public class GoogleMappingProfile : IMappingProfile
    {
        /// <summary>
        /// Configures the mappings between source and target types.
        /// </summary>
        /// <param name="registry">The registry where mappings are registered.</param>
        public void ConfigureMappings(IMappingRegistry registry)
        {
            // Register mapping functions for Reservation <-> GoogleReservation
            registry.Register<Reservation, GoogleReservation>(MapToGoogle);
            registry.Register<GoogleReservation, Reservation>(MapFromGoogle);
        }

        /// <summary>
        /// Configures type aliases to be used for resolving types.
        /// </summary>
        /// <param name="typeResolver">The type resolver used to register aliases.</param>
        public void ConfigureTypeAliases(ITypeResolver typeResolver)
        {
            // Register type aliases for Reservation and GoogleReservation
            typeResolver.RegisterAlias("Model.Reservation", typeof(Reservation));
            typeResolver.RegisterAlias("Google.Reservation", typeof(GoogleReservation));
        }

        /// <summary>
        /// Maps a DIRS21 <see cref="Reservation"/> object to a <see cref="GoogleReservation"/> object.
        /// </summary>
        /// <param name="source">The source <see cref="Reservation"/> object to map from.</param>
        /// <returns>A <see cref="GoogleReservation"/> object mapped from the source.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source object is null.</exception>
        private GoogleReservation MapToGoogle(Reservation source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new GoogleReservation
            {
                ExternalId = source.Id.ToString(),
                GuestFullName = source.GuestName,
                StartDate = source.CheckInDate,
                EndDate = source.CheckOutDate,
                RoomDetails = MapToGoogleRoom(source.Room)
            };
        }

        /// <summary>
        /// Maps a <see cref="GoogleReservation"/> object to a DIRS21 <see cref="Reservation"/> object.
        /// </summary>
        /// <param name="source">The source <see cref="GoogleReservation"/> object to map from.</param>
        /// <returns>A <see cref="Reservation"/> object mapped from the source.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source object is null.</exception>
        private Reservation MapFromGoogle(GoogleReservation source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new Reservation
            {
                Id = int.TryParse(source.ExternalId, out int id) ? id : 0,
                GuestName = source.GuestFullName,
                CheckInDate = source.StartDate,
                CheckOutDate = source.EndDate,
                Room = MapFromGoogleRoom(source.RoomDetails)
            };
        }

        /// <summary>
        /// Maps a DIRS21 <see cref="Room"/> object to a <see cref="GoogleRoom"/> object.
        /// </summary>
        /// <param name="source">The source <see cref="Room"/> object to map from.</param>
        /// <returns>A <see cref="GoogleRoom"/> object mapped from the source, or null if the source is null.</returns>
        protected virtual GoogleRoom MapToGoogleRoom(Room source)
        {
            if (source == null)
                return null;

            return new GoogleRoom
            {
                RoomId = source.RoomId.ToString(),
                RoomType = source.RoomType.ToString(),
                PricePerNight = source.PricePerNight,
                Capacity = source.Capacity,
                IsAvailable = source.IsAvailable,
                Description = source.Description,
                Amenities = source.Amenities
            };
        }

        /// <summary>
        /// Maps a <see cref="GoogleRoom"/> object to a DIRS21 <see cref="Room"/> object.
        /// </summary>
        /// <param name="source">The source <see cref="GoogleRoom"/> object to map from.</param>
        /// <returns>A <see cref="Room"/> object mapped from the source, or null if the source is null.</returns>
        /// <exception cref="ArgumentException">Thrown if the room type is invalid.</exception>
        protected virtual Room MapFromGoogleRoom(GoogleRoom source)
        {
            if (source == null)
                return null;

            return new Room
            {
                RoomId = Guid.TryParse(source.RoomId, out Guid roomId)
                    ? roomId : Guid.Empty,
                RoomType = Enum.TryParse(source.RoomType, out RoomType roomType)
                    ? roomType : throw new ArgumentException($"Invalid room type: {source.RoomType}"),
                PricePerNight = source.PricePerNight,
                Capacity = source.Capacity,
                IsAvailable = source.IsAvailable,
                Description = source.Description,
                Amenities = source.Amenities
            };
        }
    }
}