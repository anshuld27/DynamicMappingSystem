namespace DIRS21.API.Models.Google
{
    /// <summary>
    /// Represents a room in the Google system.
    /// </summary>
    public class GoogleRoom
    {
        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        /// Gets or sets the type of the room (e.g., Suite, Deluxe).
        /// </summary>
        public string RoomType { get; set; }

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