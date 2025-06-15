namespace DynamicMappingSystem.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur during the mapping process.
    /// </summary>
    public class MappingException : MappingSystemException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MappingException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class with a specified 
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public MappingException(string message, Exception inner)
            : base(message, inner) { }
    }
}