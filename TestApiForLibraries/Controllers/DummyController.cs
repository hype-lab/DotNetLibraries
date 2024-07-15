using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestApiForLibraries.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DummyController(IEmailService emailService) : ControllerBase
    {
        [HttpGet("sendDummyEmail")]
        public async Task<IActionResult> SendDummyEmail([FromQuery] string? clientId = null)
        {
            try
            {
                CustomMailMessage mailMessage = CustomMailMessage.Create(
                    emailTo: "matteo.peru92@gmail.com",
                    emailSubject: "MailEngine .NET Library Email",
                    emailFrom: "info@hype-lab.it",
                    htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
                    plainTextContent: "Test email from MailEngine .NET Library",
                    emailToName: "Matteo",
                    emailFromName: "Info Hype-Lab");

                EmailServiceResponse resp = await emailService.SendEmailAsync(mailMessage, clientId);
                if (resp.IsError)
                    return BadRequest($"Error sending test mail.\n{resp.Message}");

                return Ok(resp.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error sending test mail.\n{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
}
