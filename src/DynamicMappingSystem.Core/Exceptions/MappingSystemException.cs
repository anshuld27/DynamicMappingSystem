namespace DynamicMappingSystem.Core.Exceptions
{
    /// <summary>
    /// Serves as the base class for all custom exceptions in the mapping system.
    /// </summary>
    public abstract class MappingSystemException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingSystemException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected MappingSystemException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingSystemException"/> class
        /// with a specified error message and a reference to the inner exception that is
        /// the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        protected MappingSystemException(string message, Exception inner)
            : base(message, inner) { }
    }
}