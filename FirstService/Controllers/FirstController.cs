using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FirstService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly List<Course> courseslist = new List<Course>()
        {
            new Course{Cname="DotnetFS", Cfee=13000},
            new Course{Cname="JavaFS",Cfee=15000},
        };
        public FirstController(HttpClient http,IMemoryCache cache)
        {
            _http = http;
            _cache = cache;
        }
        [HttpGet]
        [Route("Courselist")]
        public ActionResult Courselist()
        {
            return Ok(courseslist);
        }

        [HttpGet]
        [Route("SayHello")]
        public async Task<ActionResult> SayHello()
        {
            await Task.Delay(10000);
            return Ok("Hello students! I am from first Microservice"); 
        }

        [HttpGet]
        [Route("Hello")]
        public async Task<IActionResult> Hello()
        {
            var httpclient=new HttpClient();
            string response=await _http.GetStringAsync("http://localhost:5000/Second/GreetMe");
            return Ok(response);
        }

        [HttpGet]
        [Route("CoursethroughApi")]
        [EnableRateLimiting("FixedPolicy")]
        public async Task<IActionResult> CoursethroughApi()
        {
            if (_cache.TryGetValue("InMemCache", out List<Course>? courses))
            {
                Console.WriteLine("in-memory courses found" + courses);
                return Ok(courses.ToList());
            }
            else
            {
                var cacheEntryPoints = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                   .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                   .SetPriority(CacheItemPriority.Normal);
                _cache.Set("InMemCache", this.courseslist, cacheEntryPoints);
                await Task.Delay(5000);
                return Ok(this.courseslist);
            }
               
        }
        internal class Course
        {
            public string Cname { get; set; }
            public int Cfee { get; set; }
        }
    }
}
