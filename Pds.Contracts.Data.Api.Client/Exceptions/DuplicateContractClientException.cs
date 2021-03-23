using System;
using System.Runtime.Serialization;

namespace Pds.Contracts.Data.Api.Client.Exceptions
{
    /// <summary>
    /// Represents error raised when creating a contract that already exists.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class DuplicateContractClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateContractClientException"/> class.
        /// </summary>
        public DuplicateContractClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateContractClientException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateContractClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateContractClientException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public DuplicateContractClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateContractClientException" /> class.
        /// </summary>
        /// <param name="contractNumber">The contract number.</param>
        /// <param name="contractVersion">The contract version.</param>
        /// <param name="innerException">The inner exception.</param>
        public DuplicateContractClientException(string contractNumber, int contractVersion, Exception innerException)
            : base($"A contract with ContractNumber [{contractNumber}] and ContractVersion [{contractVersion}] already exists.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateContractClientException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected DuplicateContractClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}