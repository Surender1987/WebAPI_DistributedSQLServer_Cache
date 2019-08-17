using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI_DistributedSQLServer_Cache.BusinessLayer.Interfaces;
using WebAPI_DistributedSQLServer_Cache.DataaccessLayer.Interfaces;
using WebAPI_DistributedSQLServer_Cache.Models;

namespace WebAPI_DistributedSQLServer_Cache.BusinessLayer
{
    /// <summary>
    /// Student service
    /// </summary>
    public class StudentService : IStudentService
    {
        /// <summary>
        /// Student data adapter
        /// </summary>
        private readonly IStudentDataAdapeter _studentDataAdapter;

        /// <summary>
        /// Create instance for <see cref="StudentService"/>
        /// </summary>
        public StudentService(IStudentDataAdapeter studentDataAdapter)
            => _studentDataAdapter = studentDataAdapter ?? throw new ArgumentNullException(nameof(studentDataAdapter));

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns></returns>
        public Task<List<StudentDTO>> Get() => _studentDataAdapter.Get();

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<StudentDTO> Get(int id) => _studentDataAdapter.Get(id);

        /// <summary>
        /// Add new student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<int> Post(StudentDTO student) => _studentDataAdapter.Post(student);

        /// <summary>
        /// Replace student 
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<int> Put(StudentDTO student) => _studentDataAdapter.Put(student);
    }
}