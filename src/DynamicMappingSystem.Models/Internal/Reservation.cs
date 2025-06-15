namespace DIRS21.API.Models.Internal
{
    /// <summary>
    /// Represents a reservation in the DIRS21 internal system.
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the guest associated with the reservation.
        /// </summary>
        public string GuestName { get; set; }

        /// <summary>
        /// Gets or sets the check-in date for the reservation.
        /// </summary>
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Gets or sets the check-out date for the reservation.
        /// </summary>
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Gets or sets the room details associated with the reservation.
        /// </summary>
        public Room Room { get; set; }
    }
}