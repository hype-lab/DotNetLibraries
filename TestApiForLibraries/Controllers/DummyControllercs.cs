using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Services;
using HypeLab.MailEngine.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestApiForLibraries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyControllercs : ControllerBase
    {
        private readonly IEmailService _emailService;

        public DummyControllercs(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("sendDummyEmail")]
        public async Task<IActionResult> SendDummyEmail([FromQuery] string? clientId = null)
        {
            try
            {
                CustomMailMessage mailMessage = CustomMailMessage.Create(
                    emailTo: "your_email@to",
                    emailSubject: "MailEngine .NET Library Email",
                    emailFrom: "info@hype-lab.it",
                    htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
                    plainTextContent: "Test email from MailEngine .NET Library",
                    emailToName: "Matt P",
                    emailFromName: "Info Hype-Lab");

                EmailServiceResponse resp = await _emailService.SendEmailAsync(mailMessage, clientId);
                if (resp.IsError)
                    throw new InvalidOperationException(resp.Message);

                // ...

                return Ok(resp.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}\n{ex.InnerException?.Message}\n{ex.StackTrace}");
            }
        }
    }
}
