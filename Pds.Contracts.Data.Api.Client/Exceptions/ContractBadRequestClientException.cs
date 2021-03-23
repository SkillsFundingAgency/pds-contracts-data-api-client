using System;
using System.Runtime.Serialization;

namespace Pds.Contracts.Data.Api.Client.Exceptions
{
    /// <summary>
    /// Contract bad request exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ContractBadRequestClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBadRequestClientException"/> class.
        /// </summary>
        public ContractBadRequestClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBadRequestClientException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ContractBadRequestClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBadRequestClientException" /> class.
        /// </summary>
        /// <param name="message">Message to set in base exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public ContractBadRequestClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBadRequestClientException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ContractBadRequestClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}