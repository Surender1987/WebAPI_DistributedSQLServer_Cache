using AutoMapper;
using WebAPI_DistributedSQLServer_Cache.DataAdapter.Models;
using WebAPI_DistributedSQLServer_Cache.Models;

namespace WebAPI_DistributedSQLServer_Cache.Infrastructure
{
    /// <summary>
    /// Automapper configurations
    /// </summary>
    public class AutomapperProfile: Profile
    {
        /// <summary>
        /// Initialize instance for <see cref="AutomapperProfile"/>
        /// </summary>
        public AutomapperProfile()
        {
            CreateMap<AddressDTO, Address>().ReverseMap();
            CreateMap<StudentDTO, Student>().ReverseMap();
        }
    }
}
