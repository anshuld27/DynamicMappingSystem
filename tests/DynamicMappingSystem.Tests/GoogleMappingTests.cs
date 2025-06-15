using DIRS21.API.Models.Google;
using DIRS21.API.Models.Internal;
using DynamicMappingSystem.Core.Exceptions;
using DynamicMappingSystem.Core.Interfaces;
using DynamicMappingSystem.Core.MappingProfiles;
using DynamicMappingSystem.Core.Services;
using System;
using Xunit;

namespace DynamicMappingSystem.Tests
{
    /// <summary>
    /// Contains unit tests for verifying the mapping logic between DIRS21 internal models and Google models.
    /// </summary>
    public class GoogleMappingTests
    {
        private readonly IMapHandler _mapHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMappingTests"/> class
        /// and sets up the mapping system for testing.
        /// </summary>
        public GoogleMappingTests()
        {
            ITypeResolver typeResolver = new TypeResolver();
            IMappingRegistry mappingRegistry = new MappingRegistry();

            var profile = new GoogleMappingProfile();
            profile.ConfigureTypeAliases(typeResolver);
            profile.ConfigureMappings(mappingRegistry);

            _mapHandler = new MapHandler(mappingRegistry, typeResolver);
        }

        /// <summary>
        /// Tests mapping from a DIRS21 reservation to a Google reservation and verifies the correctness of the mapping.
        /// </summary>
        [Fact]
        public void Map_InternalToGoogle_CorrectMapping()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1001,
                GuestName = "John Doe",
                CheckInDate = new DateTime(2023, 10, 15),
                CheckOutDate = new DateTime(2023, 10, 20),
                Room = new Room
                {
                    RoomId = Guid.NewGuid(),
                    RoomType = RoomType.Suite,
                    PricePerNight = 199.99m,
                    Capacity = 4,
                    IsAvailable = true,
                    Description = "Premium suite",
                    Amenities = new List<string> { "WiFi", "Jacuzzi" }
                }
            };

            // Act
            var result = _mapHandler.Map(
                reservation,
                "Model.Reservation",
                "Google.Reservation") as GoogleReservation;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1001", result.ExternalId);
            Assert.Equal("John Doe", result.GuestFullName);
            Assert.Equal(new DateTime(2023, 10, 15), result.StartDate);
            Assert.Equal("Suite", result.RoomDetails.RoomType);
            Assert.Equal(199.99m, result.RoomDetails.PricePerNight);
            Assert.Equal(4, result.RoomDetails.Capacity);
            Assert.True(result.RoomDetails.IsAvailable);
            Assert.Equal("Premium suite", result.RoomDetails.Description);
            Assert.Equal(new List<string> { "WiFi", "Jacuzzi" }, result.RoomDetails.Amenities);
        }

        /// <summary>
        /// Tests mapping from a Google reservation to a DIRS21 reservation and verifies the correctness of the mapping.
        /// </summary>
        [Fact]
        public void Map_GoogleToInternal_CorrectMapping()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var googleRes = new GoogleReservation
            {
                ExternalId = "2002",
                GuestFullName = "Jane Smith",
                StartDate = new DateTime(2023, 11, 1),
                EndDate = new DateTime(2023, 11, 5),
                RoomDetails = new GoogleRoom
                {
                    RoomId = roomId.ToString(),
                    RoomType = "King",
                    PricePerNight = 249.99m,
                    Capacity = 2,
                    IsAvailable = true,
                    Description = "King size bed",
                    Amenities = new List<string> { "Breakfast", "Parking" }
                }
            };

            // Act
            var result = _mapHandler.Map(
                googleRes,
                "Google.Reservation",
                "Model.Reservation") as Reservation;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2002, result.Id);
            Assert.Equal("Jane Smith", result.GuestName);
            Assert.Equal(new DateTime(2023, 11, 1), result.CheckInDate);
            Assert.Equal(roomId, result.Room.RoomId);
            Assert.Equal(RoomType.King, result.Room.RoomType);
            Assert.Equal(249.99m, result.Room.PricePerNight);
            Assert.Equal(2, result.Room.Capacity);
            Assert.True(result.Room.IsAvailable);
            Assert.Equal("King size bed", result.Room.Description);
            Assert.Equal(new List<string> { "Breakfast", "Parking" }, result.Room.Amenities);
        }

        /// <summary>
        /// Tests mapping a reservation with a null room and verifies that it is handled correctly.
        /// </summary>
        [Fact]
        public void Map_NullRoom_HandlesCorrectly()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 3003,
                GuestName = "No Room",
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                Room = null
            };

            // Act
            var result = _mapHandler.Map(
                reservation,
                "Model.Reservation",
                "Google.Reservation") as GoogleReservation;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.RoomDetails);
        }

        /// <summary>
        /// Tests mapping a Google reservation with an invalid room type and verifies that an exception is thrown.
        /// </summary>
        [Fact]
        public void Map_InvalidRoomType_ThrowsException()
        {
            // Arrange
            var googleRes = new GoogleReservation
            {
                ExternalId = "4004",
                GuestFullName = "Invalid Room",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                RoomDetails = new GoogleRoom
                {
                    RoomId = Guid.NewGuid().ToString(),
                    RoomType = "InvalidType",
                    PricePerNight = 100m,
                    Capacity = 1,
                    IsAvailable = true
                }
            };

            // Act & Assert
            var ex = Assert.Throws<MappingException>(() =>
                _mapHandler.Map(
                    googleRes,
                    "Google.Reservation",
                    "Model.Reservation"));

            var innerEx = Assert.IsType<ArgumentException>(ex.InnerException);
            Assert.Contains("Invalid room type", innerEx.Message);
        }

        /// <summary>
        /// Tests mapping a Google reservation with an invalid GUID and verifies that it is handled gracefully.
        /// </summary>
        [Fact]
        public void Map_InvalidGuid_HandlesGracefully()
        {
            // Arrange
            var googleRes = new GoogleReservation
            {
                ExternalId = "5005",
                GuestFullName = "Invalid GUID",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                RoomDetails = new GoogleRoom
                {
                    RoomId = "invalid-guid",
                    RoomType = "Queen",
                    PricePerNight = 150m,
                    Capacity = 2,
                    IsAvailable = true
                }
            };

            // Act
            var result = _mapHandler.Map(
                googleRes,
                "Google.Reservation",
                "Model.Reservation") as Reservation;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.Room.RoomId);
        }

        /// <summary>
        /// Tests mapping with a missing mapping function and verifies that a <see cref="TypeAliasNotFoundException"/> is thrown.
        /// </summary>
        [Fact]
        public void Map_MissingMappingFunction_ThrowsTypeAliasNotFoundException()
        {
            // Arrange
            var reservation = new Reservation();

            // Act & Assert
            var ex = Assert.Throws<TypeAliasNotFoundException>(() =>
                _mapHandler.Map(reservation, "Model.Reservation", "Unknown.Target"));

            Assert.Contains("Type alias 'Unknown.Target' not registered", ex.Message);
        }

        /// <summary>
        /// Tests mapping with empty or null type aliases and verifies that an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Theory]
        [InlineData(null, "Google.Reservation")]
        [InlineData("", "Google.Reservation")]
        [InlineData("Model.Reservation", null)]
        [InlineData("Model.Reservation", "")]
        public void Map_WithEmptyOrNullTypeAliases_ThrowsArgumentException(string sourceAlias, string targetAlias)
        {
            // Arrange
            var reservation = new Reservation();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                _mapHandler.Map(reservation, sourceAlias, targetAlias));

            Assert.Contains("cannot be empty", ex.Message);
        }

        /// <summary>
        /// Tests a round-trip mapping (internal to Google and back) and verifies that the data remains consistent.
        /// </summary>
        [Fact]
        public void Map_WithCircularMapping_HandlesGracefully()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1,
                GuestName = "Test Guest",
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(1),
                Room = new Room
                {
                    RoomId = Guid.NewGuid(),
                    RoomType = RoomType.Single,
                    PricePerNight = 100m,
                    Capacity = 2,
                    IsAvailable = true
                }
            };

            // Act
            var googleRes = _mapHandler.Map(reservation, "Model.Reservation", "Google.Reservation") as GoogleReservation;
            var roundTrip = _mapHandler.Map(googleRes!, "Google.Reservation", "Model.Reservation") as Reservation;

            // Assert
            Assert.NotNull(googleRes);
            Assert.NotNull(roundTrip);
            Assert.Equal(reservation.Id, roundTrip!.Id);
            Assert.Equal(reservation.GuestName, roundTrip.GuestName);
        }
    }
}