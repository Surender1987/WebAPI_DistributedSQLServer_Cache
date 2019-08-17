# Distributed caching in ASP.NET Core
.Net core have two types of cache one is In-Memory cache and second is distributed cache. I have described In-Memory cache in a seprate repository https://github.com/Surender1987/Webapi_inmemory_cache. This repository will cover distributed cache and how we can achieve it with sql server.

In-memory cache is quite best fit if scalability or server memory uses is not an issue for application. But today most of the applications are hosted on server farm so that they can be scaled when required or load increases. We can use in-memory cache in case of multi server application but will restricted to sticky session which make sure all subsequent request from client will go to same server. For example Azure web app use ARR(Application request routing) to make sure all subsequent request will go to same server.

A distributed cache is shared by multiple servers or we can say that distributed cache is an external service which shared by multiple servers. In .net core we can use distributed sql server cache, redis and NCache. In this repository we will use distributed sql server cache.

## IDistributedCache Interface
Distributed cache can be used with the help of IDistributedCache interface which exist in nuget package "Microsoft.Extensions.Caching.Distributed". This interface provides following methods 

1. Get, GetAsync : These methods can be used to fatch cached data as array of bytes corresponding to key. Get fetched cached data synchronously while GetAsync provide same functionality asynchronously.
2. Set, SetAsync: These are the methods which takes three parameters key, value(Array of bytes) and distributed cached entry option and add object to cache corresponding to provided key.
3. Refresh, RefreshAsync: These methods refresh the cached object and reset the sliding expiration.
4. Remove, RemoveAsync: As name indicates these methods can be used to remove cached object corresponding to provided key.

## Distributed SQL server cache
We can use distributed sql server caching with the help of "Microsoft.Extensions.Caching.SqlServer" nuget package which exists in "Microsoft.ASPNetCore.App" nuget package. Once package has restore add all dependencies to container in configureServices method with following code

	services.AddDistributedSqlServerCache(option =>
        {
            option.ConnectionString = Configuration.GetValue<string>("ConnectionString");
            option.SchemaName = "dbo";
            option.TableName = "cacheTable";
        });

AddDistributedSqlServerCache is an extension method which required SQL server connection string, schema name and tabe name and all data cached to this sql server to provided table. Before using this method we have to create table in sql server. We can create this table with the help of sql-cache tool command is below

	dotnet sql-cache create "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DistCache;Integrated Security=True;" dbo cacheTable

We have to run above command from visual studio console window. Now we all set and all dependencies required for distributed sql server cache has been registered to the container. 

Once all dependencies are registered, we can inject IDistributedCached instance to any implementation and can get or set cache objects. As in current repository we inject IDistributedCache instance to student controller constructor as below

	using Microsoft.Extensions.Caching.Distributed;
	
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

Once distrubuted cache instance injected to controller or class we can use provided methods to get, set remove and refresh cached data as we used in Get action of student controller with following code 

	/// <summary>
    /// Get all students
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    public async Task<List<StudentDTO>> Get()
    {
        var students = await _memoryCache.GetAsync(ALLSTUDENTCACHEKEY);
            
        if (students == null)
        {
            var lstStudents = await _studentService.Get();
            var memoryCacheOption = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };

            BinaryFormatter bf1 = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf1.Serialize(ms, lstStudents);
                students =  ms.ToArray();
            }

            await _memoryCache.SetAsync(ALLSTUDENTCACHEKEY, students, memoryCacheOption);
            return lstStudents;
        }
            
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(students))
        {
            object obj = bf.Deserialize(ms);
            return (List<StudentDTO>)obj;
        }
    }

In above code DistributedCacheEntryOptions provides a way to define behaviour and life of cached object as i set a sliding expiration for cached object.
