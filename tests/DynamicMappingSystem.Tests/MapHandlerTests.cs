using DynamicMappingSystem.Core.Exceptions;
using DynamicMappingSystem.Core.Interfaces;
using DynamicMappingSystem.Core.Services;
using Moq;

namespace DynamicMappingSystem.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="MapHandler"/> class to verify its mapping functionality.
    /// </summary>
    public class MapHandlerTests
    {
        private readonly Mock<IMappingRegistry> _mockRegistry;
        private readonly Mock<ITypeResolver> _mockTypeResolver;
        private readonly IMapHandler _mapHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapHandlerTests"/> class
        /// and sets up mocked dependencies for testing.
        /// </summary>
        public MapHandlerTests()
        {
            _mockRegistry = new Mock<IMappingRegistry>();
            _mockTypeResolver = new Mock<ITypeResolver>();

            _mapHandler = new MapHandler(_mockRegistry.Object, _mockTypeResolver.Object);
        }

        /// <summary>
        /// Tests that a valid mapping function correctly maps a source object to a target object.
        /// </summary>
        [Fact]
        public void Map_WithValidMapping_ReturnsMappedObject()
        {
            // Arrange
            var source = new TestSource { Id = 1, Name = "Test" };
            var target = new TestTarget { Id = 1, DisplayName = "Test" };

            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Source"))
                .Returns(typeof(TestSource));
            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Target"))
                .Returns(typeof(TestTarget));
            _mockRegistry
                .Setup(r => r.GetMappingFunc(typeof(TestSource), typeof(TestTarget)))
                .Returns(data => target);

            // Act
            var result = _mapHandler.Map(source, "Test.Source", "Test.Target") as TestTarget;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.DisplayName);
        }

        /// <summary>
        /// Tests that an <see cref="ArgumentException"/> is thrown when the source object type does not match the expected type.
        /// </summary>
        [Fact]
        public void Map_WithInvalidSourceType_ThrowsArgumentException()
        {
            // Arrange
            var invalidData = "invalid data";

            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Source"))
                .Returns(typeof(TestSource));
            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Target"))
                .Returns(typeof(TestTarget));

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                _mapHandler.Map(invalidData, "Test.Source", "Test.Target"));

            Assert.Contains(
                $"Data type '{invalidData.GetType().Name}' does not match expected type 'Test.Source'",
                ex.Message);
            Assert.Equal("data", ex.ParamName);
        }

        /// <summary>
        /// Tests that a <see cref="MappingNotFoundException"/> is thrown when no mapping function is registered for the source and target types.
        /// </summary>
        [Fact]
        public void Map_WithMissingMapping_ThrowsMappingNotFoundException()
        {
            // Arrange
            var source = new TestSource();

            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Source"))
                .Returns(typeof(TestSource));
            _mockTypeResolver
                .Setup(r => r.Resolve("Invalid.Target"))
                .Returns(typeof(TestTarget));
            _mockRegistry
                .Setup(r => r.GetMappingFunc(typeof(TestSource), typeof(TestTarget)))
                .Returns((Func<object, object>?)null);

            // Act & Assert
            var ex = Assert.Throws<MappingNotFoundException>(() =>
                _mapHandler.Map(source, "Test.Source", "Invalid.Target"));

            Assert.Equal("Test.Source", ex.SourceType);
            Assert.Equal("Invalid.Target", ex.TargetType);
        }

        /// <summary>
        /// Tests that a <see cref="TypeAliasNotFoundException"/> is thrown when a type alias is not registered.
        /// </summary>
        [Theory]
        [InlineData("Missing.Source", "Test.Target")]
        [InlineData("Test.Source", "Missing.Target")]
        public void Map_WithUnregisteredTypeAlias_ThrowsTypeAliasNotFoundException(string sourceAlias, string targetAlias)
        {
            // Arrange
            _mockTypeResolver
                .Setup(r => r.Resolve(sourceAlias))
                .Throws(new TypeAliasNotFoundException(sourceAlias));

            var source = new TestSource();

            // Act & Assert
            var ex = Assert.Throws<TypeAliasNotFoundException>(() =>
                _mapHandler.Map(source, sourceAlias, targetAlias));

            Assert.Equal(sourceAlias, ex.Alias);
        }

        /// <summary>
        /// Tests that an <see cref="ArgumentNullException"/> is thrown when the source object is null.
        /// </summary>
        [Fact]
        public void Map_WithNullData_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                _mapHandler.Map(null!, "Test.Source", "Test.Target"));

            Assert.Equal("data", ex.ParamName);
        }

        /// <summary>
        /// Tests that a <see cref="MappingException"/> is thrown when an unexpected error occurs during mapping.
        /// </summary>
        [Fact]
        public void Map_WithUnexpectedError_ThrowsMappingException()
        {
            // Arrange
            var source = new TestSource();

            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Source"))
                .Returns(typeof(TestSource));
            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Target"))
                .Returns(typeof(TestTarget));
            _mockRegistry
                .Setup(r => r.GetMappingFunc(typeof(TestSource), typeof(TestTarget)))
                .Throws(new InvalidOperationException("Unexpected error"));

            // Act & Assert
            var ex = Assert.Throws<MappingException>(() =>
                _mapHandler.Map(source, "Test.Source", "Test.Target"));

            Assert.Equal(
                "Failed to retrieve mapping for Test.Source → Test.Target",
                ex.Message);
            Assert.Equal("Unexpected error", ex.InnerException?.Message);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
        }

        /// <summary>
        /// Tests that an <see cref="ArgumentException"/> is thrown when type aliases are null or empty.
        /// </summary>
        [Theory]
        [InlineData(null, "Test.Target")]
        [InlineData("", "Test.Target")]
        [InlineData("Test.Source", null)]
        [InlineData("Test.Source", "")]
        public void Map_WithEmptyOrNullTypeAliases_ThrowsArgumentException(string sourceAlias, string targetAlias)
        {
            // Arrange
            var source = new TestSource();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                _mapHandler.Map(source, sourceAlias, targetAlias));

            Assert.Contains("cannot be empty", ex.Message);
        }

        /// <summary>
        /// Tests that a round-trip mapping (source to target and back) is handled correctly.
        /// </summary>
        [Fact]
        public void Map_WithCircularMapping_HandlesGracefully()
        {
            // Arrange
            var source = new TestSource { Id = 1, Name = "Test" };

            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Source"))
                .Returns(typeof(TestSource));
            _mockTypeResolver
                .Setup(r => r.Resolve("Test.Target"))
                .Returns(typeof(TestTarget));
            _mockRegistry
                .Setup(r => r.GetMappingFunc(typeof(TestSource), typeof(TestTarget)))
                .Returns(data => new TestTarget { Id = 1, DisplayName = "Test" });
            _mockRegistry
                .Setup(r => r.GetMappingFunc(typeof(TestTarget), typeof(TestSource)))
                .Returns(data => new TestSource { Id = 1, Name = "Test" });

            // Act
            var target = _mapHandler.Map(source, "Test.Source", "Test.Target") as TestTarget;
            var roundTrip = _mapHandler.Map(target!, "Test.Target", "Test.Source") as TestSource;

            // Assert
            Assert.NotNull(target);
            Assert.NotNull(roundTrip);
            Assert.Equal(source.Id, roundTrip!.Id);
            Assert.Equal(source.Name, roundTrip.Name);
        }

        /// <summary>
        /// Tests that a <see cref="MappingException"/> is thrown when type resolution fails unexpectedly.
        /// </summary>
        [Fact]
        public void Map_WithInvalidTypeResolution_ThrowsMappingException()
        {
            // Arrange
            var source = new TestSource();

            _mockTypeResolver
                .Setup(r => r.Resolve("Invalid.Source"))
                .Throws(new InvalidOperationException("Unexpected resolution error"));

            // Act & Assert
            var ex = Assert.Throws<MappingException>(() =>
                _mapHandler.Map(source, "Invalid.Source", "Test.Target"));

            Assert.Contains("Type resolution failed for 'Invalid.Source'", ex.Message);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
        }

        /// <summary>
        /// Represents a test source object for mapping.
        /// </summary>
        private class TestSource
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        /// <summary>
        /// Represents a test target object for mapping.
        /// </summary>
        private class TestTarget
        {
            public int Id { get; set; }
            public string? DisplayName { get; set; }
        }
    }
}