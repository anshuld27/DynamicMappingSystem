# DynamicMappingSystem

DynamicMappingSystem is a flexible and extensible object mapping system designed to handle complex mappings between different data models. It supports type aliasing, custom mapping profiles, and error handling to ensure robust and maintainable mappings.

## Table of Contents
- [Solution Structure](#solution-structure)
- [Features](#features)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

## Solution Structure
```
DynamicMappingSystem/
|── src/
|   |── DynamicMappingSystem.Core/       # Core mapping logic
|   |── DynamicMappingSystem.Models/     # Data models
│   └── DynamicMappingSystem.ConsoleDemo # Demo application
|── tests/
│   └── DynamicMappingSystem.Tests/      # Unit tests
|── docs/
|   └── Architecture Documentation       # Design documentation
|── README.md                            # Getting started guide
└── DynamicMappingSystem.sln             # Solution file
```

## Features
- **Type Aliasing**: Resolve types using human-readable aliases.
- **Custom Mapping Profiles**: Define mappings between different models.
- **Error Handling**: Robust exception handling for invalid mappings or type resolutions.
- **Demo Application**: A console application demonstrating the mapping system in action.
- **Unit Tests**: Comprehensive test coverage to ensure reliability.

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022 or any compatible IDE

### Installation
1. Clone the repository:
```bash
git clone https://github.com/anshuld27/DynamicMappingSystem.git
```
2. Open the solution file `DynamicMappingSystem.sln` in Visual Studio.
3. Restore NuGet packages:
```bash
dotnet restore
```
4. Build the solution:
```bash
dotnet build
```

## Usage

### Running the Demo Application
1. Navigate to the `src/DynamicMappingSystem.ConsoleDemo` directory.
2. Run the application:
```bash
dotnet run
```
3. Follow the on-screen instructions to see the mapping system in action.

### Example Code
The following example demonstrates how to use the mapping system to map a `Reservation` object to a `GoogleReservation` object:
```csharp
IMapHandler mapHandler = DemoSetup.CreateMapHandler();
var dirsReservation = new Reservation { Id = 1001, GuestName = "John Doe", CheckInDate = new DateTime(2023, 10, 15), CheckOutDate = new DateTime(2023, 10, 20), Room = new Room { RoomId = Guid.NewGuid(), RoomType = RoomType.Suite, PricePerNight = 249.99m, Capacity = 4, IsAvailable = true, Description = "Luxury suite with ocean view", Amenities = new List<string> { "WiFi", "Mini-bar", "Jacuzzi", "Balcony" } } };
var googleReservation = mapHandler.Map( dirsReservation, "Model.Reservation", "Google.Reservation") as GoogleReservation;
```

## Testing
Run the unit tests to verify the fun
```bash
dotnet test
```

## Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Commit your changes and push them to your fork.
4. Submit a pull request.

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.
