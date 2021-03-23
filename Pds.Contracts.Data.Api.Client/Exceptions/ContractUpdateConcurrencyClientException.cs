using System;
using System.Runtime.Serialization;

namespace Pds.Contracts.Data.Api.Client.Exceptions
{
    /// <summary>
    /// This class handle Contract Update Concurrency Exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ContractUpdateConcurrencyClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractUpdateConcurrencyClientException"/> class.
        /// </summary>
        public ContractUpdateConcurrencyClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractUpdateConcurrencyClientException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ContractUpdateConcurrencyClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractUpdateConcurrencyClientException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public ContractUpdateConcurrencyClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractUpdateConcurrencyClientException" /> class.
        /// </summary>
        /// <param name="contractNumber">Contract number.</param>
        /// <param name="contractVersion">Contract version number.</param>
        /// <param name="innerException">The inner exception.</param>
        public ContractUpdateConcurrencyClientException(string contractNumber, int contractVersion, Exception innerException)
           : base($"Contract may have been modified or deleted since Contract were loaded - Contract Number: {contractNumber}, ContractVersion: {contractVersion}.", innerException)
        {
            ContractNumber = contractNumber;
            ContractVersion = contractVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractUpdateConcurrencyClientException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ContractUpdateConcurrencyClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Gets ContractNumber.
        /// </summary>
        /// <value>
        /// The contract number.
        /// </value>
        public string ContractNumber { get; }

        /// <summary>
        /// Gets VersionNumber.
        /// </summary>
        /// <value>
        /// The contract version.
        /// </value>
        public int ContractVersion { get; }
    }
}