using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI_DistributedSQLServer_Cache.Models;

namespace WebAPI_DistributedSQLServer_Cache.DataaccessLayer.Interfaces
{
    /// <summary>
    /// Student data adapter
    /// </summary>
    public interface IStudentDataAdapeter
    {
        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns></returns>
        Task<List<StudentDTO>> Get();

        /// <summary>
        /// Replace entity
        /// </summary>
        /// <returns></returns>
        Task<int> Put(StudentDTO student);

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <returns></returns>
        Task<StudentDTO> Get(int id);

        /// <summary>
        /// Add new entity
        /// </summary>
        /// <returns></returns>
        Task<int> Post(StudentDTO student);
    }
}
