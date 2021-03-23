using System;
using System.Runtime.Serialization;

namespace Pds.Contracts.Data.Api.Client.Exceptions
{
    /// <summary>
    /// Contract status exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ContractStatusClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractStatusClientException" /> class.
        /// </summary>
        public ContractStatusClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractStatusClientException" /> class.
        /// </summary>
        /// <param name="message">Message to set in base exception.</param>
        public ContractStatusClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractStatusClientException" /> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ContractStatusClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractStatusClientException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ContractStatusClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}