[![NuGet version (HypeLab.MailEngine)](https://img.shields.io/nuget/v/HypeLab.MailEngine.svg?style=flat-square)](https://www.nuget.org/packages/HypeLab.MailEngine/)

# Hype-Lab Mail Engine
## Note: This library targets .NET 8, so it's not compatible with lower versions of .NET.

HypeLab.MailEngine is a flexible and modular .NET library **that targets .NET 8** for managing multiple email clients, including support for both SMTP and SendGrid for now.
The package allows to configure and use multiple email clients with distinct settings, leveraging dependency injection for seamless integration.


**Features**

- **Support for multiple email clients**: Configure and use multiple email clients with individual settings.
- **SendGrid and SMTP support**: Easily switch between different email providers.
- **Flexible configuration**: Define your email client settings in appsettings.json.
- **Dependency Injection**: Integrate smoothly with ASP.NET Core's DI system.


**Installation**

Add the NuGet package to your project:

```powershell
dotnet add package HypeLab.MailEngine
```

**Configuration**

In your `appsettings.json`, define the settings for your email clients:

```json
{
	"SendGrid": {
		"ClientId": "Hype-Lab SendGridClient",
		"IsDefault": true,
		"ApiKey": "dummyKey"
	},
	"SendGrid2": {
		"ClientId": "Hype-Lab SendGridClient2",
		"IsDefault": false,
		"ApiKey": "dummyKey"
	},
	"Smtp": {
		"ClientId": "Hype-Lab SmtpClient",
		"IsDefault": false,
		"Server": "smtp.ionos.it",
		"Port": 587,
		"Email": "info@hype-lab.it",
		"Password": "dummyKey",
		"EnableSsl": true
	}
}
```
**NOTE**: All properties are mandatory (both for Smtp and SendGrid). The `IsDefault` property is used to set the default email client.
In case you intend to use a single email sender, the class constructors allow the non-presence of `IsDefault`, and set it as `true`, **so be careful when using multiple email senders**, the engine will throws an exception if more than one sender is set as default.

**Usage**

**Single Email Client**

For applications that need only one email client, use the single email client configuration:

1. **Retrieve configuration sections in Startup.cs**:

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

SendGridAccessInfo sendGridAccessInfo = builder.Configuration.GetSection("SendGrid").Get<SendGridAccessInfo>()
	?? throw new InvalidOperationException("SendGrid section not found");

builder.Services.AddMailEngine(sendGridAccessInfo);

// ...other service registrations

WebApplication app = builder.Build();
```


2. **Inject and use the email service**:

```csharp
public class MyService
{
	private readonly IEmailService _emailService;

	public MyService(IEmailService emailService)
	{
		_emailService = emailService;
	}

	public async Task SendEmailAsync()
	{
		CustomMailMessage mailMessage = CustomMailMessage.Create(
				emailTo: "your_email@to",
				emailSubject: "MailEngine .NET Library Email",
				emailFrom: "info@hype-lab.it",
				htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
				plainTextContent: "Test email from MailEngine .NET Library",
				emailToName: "Matt P",
				emailFromName: "Info Hype-Lab");

        EmailServiceResponse resp = await emailService.SendEmailAsync(mailMessage);
        if (resp.IsError)
		throw new InvalidOperationException(resp.ErrorMessage);

		// ...
	}
}
```


**Multiple Email Clients**

For applications that need to manage multiple email clients, use the multiple email client configuration:

1. **Retrieve configuration sections and pass them to AddMailEngine in Startup.cs**:

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

SendGridAccessInfo sendGridAccessInfo = Configuration.GetSection("SendGrid").Get<SendGridAccessInfo>()
	?? throw new InvalidOperationException("SendGrid section not found");

SendGridAccessInfo sendGridAccessInfo2 = Configuration.GetSection("SendGrid2").Get<SendGridAccessInfo>()
	?? throw new InvalidOperationException("SendGrid2 section not found");

SmtpAccessInfo smtpAccessInfo = Configuration.GetSection("Smtp").Get<SmtpAccessInfo>()
	?? throw new InvalidOperationException("Smtp section not found");

services.AddMailEngine(sendGridAccessInfo, sendGridAccessInfo2, smtpAccessInfo);

// ...other service registrations

WebApplication app = builder.Build();
```

2. **Inject and use the email service**:

```csharp
public class MyService
{
	private readonly IEmailService _emailService;

	public MyService(IEmailService emailService)
	{
		_emailService = emailService;
	}

	public async Task SendEmailAsync()
	{
		CustomMailMessage mailMessage = CustomMailMessage.Create(
				emailTo: "your_email@to",
				emailSubject: "MailEngine .NET Library Email",
				emailFrom: "info@hype-lab.it",
				htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
				plainTextContent: "Test email from MailEngine .NET Library",
				emailToName: "Matt P",
				emailFromName: "Info Hype-Lab");

        EmailServiceResponse resp = await emailService.SendEmailAsync(mailMessage);
        if (resp.IsError)
		throw new InvalidOperationException(resp.ErrorMessage);

		// ...
	}
}
```

2.5 Use a specific sender instead of the default one passing the **clientId** optional argument to **SendEmailAsync**:
```csharp
public async Task SendEmailAsync(string? clientId = null)
{
		CustomMailMessage mailMessage = CustomMailMessage.Create(
				emailTo: "your_email@to",
				emailSubject: "MailEngine .NET Library Email",
				emailFrom: "info@hype-lab.it",
				htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
				plainTextContent: "Test email from MailEngine .NET Library",
				emailToName: "Matt P",
				emailFromName: "Info Hype-Lab");

    EmailServiceResponse resp = await emailService.SendEmailAsync(mailMessage, clientId);
    if (resp.IsError)
	throw new InvalidOperationException(resp.ErrorMessage);

	// ...
}
```


**Overloaded AddMailEngine Methods**

**Single Email Client**

Use this overload if you have only one email client:
```csharp
public static IServiceCollection AddMailEngine(this IServiceCollection services, IMailAccessInfo mailAccessInfo)
```


**Multiple Email Clients**

Use this overload if you need to support multiple email clients:
```csharp
public static IServiceCollection AddMailEngine(this IServiceCollection services, params IMailAccessInfo[] mailAccessInfoParams)
```


**Interfaces and Classes**

- `IMailAccessInfo`: Base interface for email access information.
- `SendGridAccessInfo`: Configuration class for SendGrid email client.
- `SmtpAccessInfo`: Configuration class for SMTP email client.
- `IMailAccessesInfo`: Interface for managing multiple email clients.
- `IEmailService`: Interface for sending emails.
- `ISmtpEmailService`: Specific interface for sending emails through SMTP.
- `ISendGridEmailService`: Specific interface for sending emails through SendGrid.


**Contributing**

Contributions are welcome! Please feel free to open an issue or to ask anything if needed.


**License**

This project is licensed under the MIT License.