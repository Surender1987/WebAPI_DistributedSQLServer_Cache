using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_DistributedSQLServer_Cache.DataaccessLayer.Interfaces;
using WebAPI_DistributedSQLServer_Cache.DataAdapter;
using WebAPI_DistributedSQLServer_Cache.DataAdapter.Models;
using WebAPI_DistributedSQLServer_Cache.Models;

namespace WebAPI_DistributedSQLServer_Cache.DataaccessLayer
{
    /// <summary>
    /// Student data adapter
    /// </summary>
    public class StudentDataAdapeter : IStudentDataAdapeter
    {
        /// <summary>
        /// Student db context
        /// </summary>
        private readonly StudentDBContext _studentDBContext;

        /// <summary>
        /// Mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialize instance for <see cref="StudentDataAdapeter"/>
        /// </summary>
        public StudentDataAdapeter(StudentDBContext studentDBContext, IMapper mapper)
        {
            _studentDBContext = studentDBContext ?? throw new ArgumentNullException(nameof(studentDBContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns></returns>
        public async Task<List<StudentDTO>> Get()
        {
            var students = await _studentDBContext.Students.ToListAsync();
            return _mapper.Map<List<StudentDTO>>(students);
        }

        /// <summary>
        /// Get Student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<StudentDTO> Get(int id)
        {
            var student = _studentDBContext.Students.Where<Student>(x => x.Id == id);
            return Task.FromResult(_mapper.Map<StudentDTO>(student));
        }

        /// <summary>
        /// Add new student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<int> Post(StudentDTO student)
        {
            var studentEntity = _mapper.Map<Student>(student);
            var std = _studentDBContext.Students.Add(studentEntity);
            _studentDBContext.SaveChanges();
            return Task.FromResult(std.Entity.Id);
        }

        public Task<int> Put(StudentDTO student)
        {
            var studentEntity = _mapper.Map<Student>(student);
            var std = _studentDBContext.Students.Update(studentEntity);
            return Task.FromResult(std.Entity.Id);
        }
    }
}
