using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI_DistributedSQLServer_Cache.BusinessLayer.Interfaces;
using WebAPI_DistributedSQLServer_Cache.Models;

namespace WebAPI_DistributedSQLServer_Cache.Controllers
{
    /// <summary>
    /// Controller to handle requests for student
    /// </summary>
    [Route("v1/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        /// <summary>
        /// Cahey keys
        /// </summary>
        private const string ALLSTUDENTCACHEKEY = "AllStudents";
        private const string STUDENTBYID = "StudentById";

        /// <summary>
        /// Student service
        /// </summary>
        private readonly IStudentService _studentService;

        /// <summary>
        /// Memory cache variable
        /// </summary>
        private readonly IDistributedCache _memoryCache;

        /// <summary>
        /// Initialize instance for <see cref="StudentController"/>
        /// </summary>
        public StudentController(IStudentService studentService, IDistributedCache memoryCache)
        {
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<List<StudentDTO>> Get()
        {
            if (!_memoryCache.TryGetValue(ALLSTUDENTCACHEKEY, out List<StudentDTO> studentList))
            {
                studentList = await _studentService.Get();
                var memoryCacheOption = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };
                _memoryCache.Set(ALLSTUDENTCACHEKEY, studentList, memoryCacheOption);
                SetSource(ref studentList, "From Database");
            }
            else
            {
                SetSource(ref studentList, "From Cache");
            }
            return studentList;
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public Task<StudentDTO> Get(int id)
        {
            return _memoryCache.GetOrCreate(STUDENTBYID, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                var student = await _studentService.Get(id);

                return student;
            });
        }

        /// <summary>
        /// Add new student 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public Task<int> Post(StudentDTO studentDTO) => _studentService.Post(studentDTO);

        /// <summary>
        /// Replace student
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public Task<int> Put(StudentDTO studentDTO) => _studentService.Put(studentDTO);

        private void SetSource(ref List<StudentDTO> students, string source)
            => students.ForEach((std) => std.Source = source);
    }
}