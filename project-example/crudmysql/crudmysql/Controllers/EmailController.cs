using crudmysql.Helpers;
using crudmysql.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/email")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send()
    {
        var to = "agung.kusaeri.work@gmail.com";
        var subject = "Test";


        var body = @"
    <div style='font-family: sans-serif; padding: 20px;'>
        <h1 style='color: #333;'>Welcome to MyApp</h1>
        <p>Thanks for signing up, <strong>Franco</strong>!</p>
        <a href='https://yourdomain.com' style='color: white; background: blue; padding: 10px; border-radius: 5px; text-decoration: none;'>Get Started</a>
    </div>
";
        await _emailService.SendEmailAsync(to, subject, body);
        return ResponseFormatter.Success(null, "Email has been sented");
    }
}
