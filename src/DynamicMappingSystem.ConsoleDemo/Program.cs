using DIRS21.API.Models.Google;
using DIRS21.API.Models.Internal;
using DynamicMappingSystem.ConsoleDemo;
using DynamicMappingSystem.Core.Interfaces;
using System.Globalization;

// Set default culture for the application to "en-US"
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

Console.WriteLine("DIRS21 Dynamic Mapping System Demo");
Console.WriteLine("==================================");

// Setup the mapping system using the DemoSetup class
IMapHandler mapHandler = DemoSetup.CreateMapHandler();

// Create a sample DIRS21 reservation object
var dirsReservation = new Reservation
{
    Id = 1001,
    GuestName = "John Doe",
    CheckInDate = new DateTime(2023, 10, 15),
    CheckOutDate = new DateTime(2023, 10, 20),
    Room = new Room
    {
        RoomId = Guid.NewGuid(),
        RoomType = RoomType.Suite,
        PricePerNight = 249.99m,
        Capacity = 4,
        IsAvailable = true,
        Description = "Luxury suite with ocean view",
        Amenities = new List<string> { "WiFi", "Mini-bar", "Jacuzzi", "Balcony" }
    }
};

Console.WriteLine("\n[1] Mapping DIRS21 Reservation => Google Reservation");
Console.WriteLine("------------------------------------------------");
// Print the details of the original DIRS21 reservation
PrintReservation(dirsReservation);

// Map the DIRS21 reservation to a Google reservation format
var googleReservation = mapHandler.Map(
    dirsReservation,
    "Model.Reservation",
    "Google.Reservation") as GoogleReservation;

Console.WriteLine("\n[2] Mapped Google Reservation:");
Console.WriteLine("------------------------------");
// Print the details of the mapped Google reservation
PrintGoogleReservation(googleReservation!);

// Map the Google reservation back to the DIRS21 reservation format
var roundTripReservation = mapHandler.Map(
    googleReservation!,
    "Google.Reservation",
    "Model.Reservation") as Reservation;

Console.WriteLine("\n[3] Round-trip Mapped DIRS21 Reservation:");
Console.WriteLine("----------------------------------------");
// Print the details of the round-trip mapped DIRS21 reservation
PrintReservation(roundTripReservation!);

// Demonstrate error handling during mapping
Console.WriteLine("\n[4] Error Handling Demonstration");
Console.WriteLine("--------------------------------");
try
{
    // Create an invalid Google reservation object to trigger an error
    var invalidGoogleRes = new GoogleReservation
    {
        ExternalId = "3003",
        GuestFullName = "Invalid Room",
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(3),
        RoomDetails = new GoogleRoom
        {
            RoomId = "invalid-guid", // Invalid GUID format
            RoomType = "InvalidType", // Invalid room type
            PricePerNight = 100m,
            Capacity = 1,
            IsAvailable = true
        }
    };

    Console.WriteLine("Attempting mapping with invalid data...");
    // Attempt to map the invalid Google reservation to a DIRS21 reservation
    var invalidResult = mapHandler.Map(
        invalidGoogleRes,
        "Google.Reservation",
        "Model.Reservation");
}
catch (Exception ex)
{
    // Catch and display the error details
    Console.WriteLine($"Error caught: {ex.GetType().Name}");
    Console.WriteLine($"Message: {ex.Message}");

    if (ex.InnerException != null)
    {
        Console.WriteLine($"Details: {ex.InnerException.Message}");
    }
}

// Helper method to print the details of a DIRS21 reservation
static void PrintReservation(Reservation res)
{
    Console.WriteLine($"ID: {res.Id}");
    Console.WriteLine($"Guest: {res.GuestName}");
    Console.WriteLine($"Check-in: {res.CheckInDate:d}");
    Console.WriteLine($"Check-out: {res.CheckOutDate:d}");

    if (res.Room != null)
    {
        Console.WriteLine($"Room ID: {res.Room.RoomId}");
        Console.WriteLine($"Type: {res.Room.RoomType}");
        Console.WriteLine($"Price: {res.Room.PricePerNight:C}");
        Console.WriteLine($"Capacity: {res.Room.Capacity}");
        Console.WriteLine($"Available: {res.Room.IsAvailable}");
        Console.WriteLine($"Description: {res.Room.Description}");
        Console.WriteLine($"Amenities: {string.Join(", ", res.Room.Amenities)}");
    }
    else
    {
        Console.WriteLine("No room details");
    }
}

// Helper method to print the details of a Google reservation
static void PrintGoogleReservation(GoogleReservation res)
{
    Console.WriteLine($"External ID: {res.ExternalId}");
    Console.WriteLine($"Guest: {res.GuestFullName}");
    Console.WriteLine($"Dates: {res.StartDate:d} to {res.EndDate:d}");

    if (res.RoomDetails != null)
    {
        Console.WriteLine($"Room ID: {res.RoomDetails.RoomId}");
        Console.WriteLine($"Type: {res.RoomDetails.RoomType}");
        Console.WriteLine($"Price: {res.RoomDetails.PricePerNight:C}");
        Console.WriteLine($"Capacity: {res.RoomDetails.Capacity}");
        Console.WriteLine($"Available: {res.RoomDetails.IsAvailable}");
        Console.WriteLine($"Description: {res.RoomDetails.Description}");
        Console.WriteLine($"Amenities: {string.Join(", ", res.RoomDetails.Amenities)}");
    }
    else
    {
        Console.WriteLine("No room details");
    }
}

Console.WriteLine("\nDemo complete. Press any key to exit...");
Console.ReadKey();