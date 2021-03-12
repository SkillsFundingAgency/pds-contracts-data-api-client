using System;
using System.Runtime.Serialization;

namespace Pds.Contracts.Data.Api.Client.Exceptions
{
    /// <summary>
    /// Contract not found exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ContractNotFoundClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractNotFoundClientException"/> class.
        /// </summary>
        public ContractNotFoundClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractNotFoundClientException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ContractNotFoundClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractNotFoundClientException" /> class.
        /// </summary>
        /// <param name="message">Exception message text.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public ContractNotFoundClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractNotFoundClientException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ContractNotFoundClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}