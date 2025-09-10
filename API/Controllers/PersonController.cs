using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DbContext;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PersonController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var persons = _context.People.ToList();
            return Ok(persons);
        }
    }
}
