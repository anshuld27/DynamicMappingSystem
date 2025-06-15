namespace DIRS21.API.Models.Google
{
    /// <summary>
    /// Represents a reservation in the Google system.
    /// </summary>
    public class GoogleReservation
    {
        /// <summary>
        /// Gets or sets the external identifier for the reservation.
        /// </summary>
        public string ExternalId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name of the guest associated with the reservation.
        /// </summary>
        public string GuestFullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the reservation.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the reservation.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the details of the room associated with the reservation.
        /// </summary>
        public GoogleRoom RoomDetails { get; set; } = new GoogleRoom();
    }
}