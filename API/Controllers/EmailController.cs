using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmailController : Controller
    {

        [HttpGet("test-email")]
        public IActionResult TestEmail([FromServices] EmailService emailService)
        {
            try
            {
                emailService.SendEmail("himasree303@gmail.com", "Test Subject", "Test Body");
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Email failed: {ex.Message}");
            }
        }
    }
}
