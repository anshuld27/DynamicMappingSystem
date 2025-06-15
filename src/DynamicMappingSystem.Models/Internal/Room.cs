namespace DIRS21.API.Models.Internal
{
    /// <summary>
    /// Represents the type of a room in the DIRS21 internal system.
    /// </summary>
    public enum RoomType
    {
        /// <summary>
        /// A single room, typically for one person.
        /// </summary>
        Single,

        /// <summary>
        /// A double room, typically for two people.
        /// </summary>
        Double,

        /// <summary>
        /// A suite, typically a larger room with additional amenities.
        /// </summary>
        Suite,

        /// <summary>
        /// A king-sized room, typically with a king-sized bed.
        /// </summary>
        King,

        /// <summary>
        /// A queen-sized room, typically with a queen-sized bed.
        /// </summary>
        Queen,

        /// <summary>
        /// A family room, typically designed for multiple occupants.
        /// </summary>
        Family
    }

    /// <summary>
    /// Represents a room in the DIRS21 internal system.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the globally unique identifier (GUID) for the room.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Gets or sets the type of the room.
        /// </summary>
        public RoomType RoomType { get; set; }

        /// <summary>
        /// Gets or sets the price per night for the room.
        /// </summary>
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// Gets or sets the maximum capacity of the room.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the room is available for booking.
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets a description of the room.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of amenities available in the room.
        /// </summary>
        public List<string> Amenities { get; set; } = new();
    }
}