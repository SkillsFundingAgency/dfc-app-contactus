using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Repository.CosmosDb
{
    /// <summary>
    /// Used to supply Cosmos DB connection values from app settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CosmosDbConnection
    {
        /// <summary>
        /// Cosmos DB - Access Key.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Cosmos DB - Endpoint Url.
        /// </summary>
        public string EndpointUrl { get; set; }

        /// <summary>
        /// Cosmos DB - Database Id.
        /// </summary>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Cosmos DB - Collection Id.
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Cosmos DB - Partition Key.
        /// </summary>
        public string PartitionKey { get; set; }
    }
}
