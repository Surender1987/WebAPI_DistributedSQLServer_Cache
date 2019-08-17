namespace WebAPI_DistributedSQLServer_Cache.DataAdapter.Models
{
    /// <summary>
    /// Address entity
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Get or set for Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or set for address line 1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Get or set for address line 2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Get or set for city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or set for state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or set for Zip
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Get or set for country
        /// </summary>
        public string Country { get; set; }

    }
}
