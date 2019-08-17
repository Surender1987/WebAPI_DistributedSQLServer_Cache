using Microsoft.EntityFrameworkCore;
using WebAPI_DistributedSQLServer_Cache.DataAdapter.Models;

namespace WebAPI_DistributedSQLServer_Cache.DataAdapter
{
    /// <summary>
    /// Provide database context for students
    /// </summary>
    public class StudentDBContext: DbContext
    {
        /// <summary>
        /// Initialize instance for <see cref="StudentDBContext"/>
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public StudentDBContext(DbContextOptions<StudentDBContext> dbContextOptions)
            :base(dbContextOptions)
        { }

        public DbSet<Student> Students { get; set; }
    }
}
