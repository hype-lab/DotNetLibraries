[![NuGet version (HypeLab.MailEngine)](https://img.shields.io/nuget/v/HypeLab.MailEngine.svg?style=flat-square)](https://www.nuget.org/packages/HypeLab.MailEngine/)
![Target Frameworks](https://img.shields.io/badge/targets-.NET%208.0%20%7C%20.NET%209.0-blue?style=flat-square)

# Hype-Lab Mail Engine
## Note: This library targets .NET 8 and .NET 9, so it's not compatible with lower versions of .NET.

HypeLab.MailEngine is a flexible and modular .NET library **that targets .NET 8 and .NET 9** for managing multiple email clients, including support for both SMTP and SendGrid for now.
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
    "ClientId": "Hype-Lab SendGridClient", // required
    "IsDefault": true, // optional (if not present, it will be set as true)
    "ApiKey": "dummyKey", // required
    "RequestHeaders": [ // optional
      {
        "Key": "Authorization", // required if RequestHeaders is present
        "Value": "Bearer SG" // required if RequestHeaders is present
      },
	  // etc.
      {
        "Key": "Content-Type",
        "Value": "application/json"
      }
    ],
    "Host": "https://api.sendgrid.com/v3/mail/send", // optional
    "Version": "v3", // optional
    "UrlPath": "/mail/send", // optional
    "Reliability": { // optional
      "MaximumNumberOfRetries": 3, // required if Reliability is present
      "MinimumBackOffInSeconds": 1, // required if Reliability is present
      "DeltaBackOffInSeconds": 1, // required if Reliability is present
      "MaximumBackOffInSeconds": 10 // required if Reliability is present
    },
    "Auth": { // optional
      "Scheme": "Bearer", // required if Auth is present
      "Parameter": "SG" // optional
    },
    "HttpErrorAsException": true // optional
  },
  "Smtp": {
    "ClientId": "Hype-Lab SmtpClient", // required
    "Server": "smtp.google.com", // required
    "Port": 587, // required
    "Email": "info@hype-lab.it", // required
    "Password": "dummyPassword", // required
    "EnableSsl": true, // required
    "IsDefault": false, // optional (if not present, it will be set as true)
    "UseDefaultCredentials": false, // optional
    "Domain": "hype-lab.it", // optional, passed to NetworkCredential constructor
    "Timeout": 10000, // optional
    "PickupDirectoryLocation": "C:\\inetpub\\mailroot\\Pickup", // optional
    "TargetName": "STARTTLS/smtp.google.com", // optional
    "DeliveryMethod": "Network", // optional, default is Network
    "DeliveryFormat": "International", // optional, default is International
    "ClientCertificates": [ // optional
      {
        "FileName": "C:\\inetpub\\mailroot\\Pickup\\hype-lab.it.pfx", // required if ClientCertificates is present
        "Password": "dummyPassword", // optional
        "KeyStorageFlags": "MachineKeySet" // optional
      }
    ]
  },
}
```
**NOTE**: The `IsDefault` property is used to set the default email client.
In case you intend to use a single email sender, the class constructors allow the non-presence of `IsDefault`, and set it as `true`, **so be careful when using multiple email senders**, the engine will throws an exception if more than one sender is set as default.

**Feel free to leave feedback about any outages or issues you may encounter with any of the properties.**

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
              List<IEmailAddressInfo> ccs =
              [
                  new EmailAddressInfo()
                  {
                      Email = "emailCc@n.1",
                      IsBcc = false,
                      IsCc = true,
                      IsTo = false,
                      Name = "Email CC 1"
                  }
              ];

              byte[] fileBytes = []; // your file bytes
              List<IAttachment> attachments =
              [
                  new SendGridAttachment(name: "testattachment.txt", type: "text/plain", content: fileBytes)
                  {
                      Disposition = "inline", // nullable; default is "attachment"
                      ContentId = Guid.NewGuid().ToString() // nullable
                  }
                  // OR if it's a smtp client attachment
                  new SmtpClientAttachment(name: "testattachment.txt", filePath: "C:\\tst\\testattachment.txt", contentType: "text/plain"/* primary MIME type*/)
                  {
                      MediaType = "text/plain", // nullable; optional, more specific MIME type
                      ContentId = Guid.NewGuid().ToString(), // nullable
                      NameEncoding = Encoding.UTF8, // nullable
                      TransferEncoding = TransferEncoding.Base64 // nullable
                  }
              ];

              CustomMailMessage mailMessage = CustomMailMessage.Create(
                  emailTo: "your_email@to",
                  emailSubject: "MailEngine .NET Library Email",
                  emailFrom: "info@hype-lab.it",
                  htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
                  plainTextContent: "Test email from MailEngine .NET Library",
                  emailToName: "Matt P",
                  emailFromName: "Info Hype-Lab"
                  ccs: ccs,
                  attachments: attachments);

              // or if you need to define multiple to addresses
              MultipleToesMailMessage mailMessage = MultipleToesMailMessage.Create(
                  emailToes: ["emailTo@n.1", "emailTo@n.2", "emailTo@n.3", "emailTo@n.4", "emailTo@n.5"],
                  emailSubject: "MailEngine .NET Library Email",
                  emailFrom: "noreply@hype-lab.it",
                  htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
                  plainTextContent: "Test email from MailEngine .NET Library",
                  emailToName: "Matt P",
                  emailFromName: "Hype-Lab NoReply"
                  ccs: ccs,
                  attachments: attachments);

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
              List<IEmailAddressInfo> ccs =
              [
                  new EmailAddressInfo()
                  {
                      Email = "emailCc@n.1",
                      IsBcc = false,
                      IsCc = true,
                      IsTo = false,
                      Name = "Email CC 1"
                  }
              ];

              byte[] fileBytes = []; // your file bytes
              List<IAttachment> attachments =
              [
                  new SendGridAttachment(name: "testattachment.txt", type: "text/plain", content: fileBytes)
                  {
                      Disposition = "inline", // nullable; default is "attachment"
                      ContentId = Guid.NewGuid().ToString() // nullable
                  }
                  // OR if it's a smtp client attachment
                  new SmtpClientAttachment(name: "testattachment.txt", filePath: "C:\\tst\\testattachment.txt", contentType: "text/plain"/* primary MIME type*/)
                  {
                      MediaType = "text/plain", // nullable; optional, more specific MIME type
                      ContentId = Guid.NewGuid().ToString(), // nullable
                      NameEncoding = Encoding.UTF8, // nullable
                      TransferEncoding = TransferEncoding.Base64 // nullable
                  }
              ];

              CustomMailMessage mailMessage = CustomMailMessage.Create(
                  emailTo: "your_email@to",
                  emailSubject: "MailEngine .NET Library Email",
                  emailFrom: "info@hype-lab.it",
                  htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
                  plainTextContent: "Test email from MailEngine .NET Library",
                  emailToName: "Matt P",
                  emailFromName: "Info Hype-Lab"
                  ccs: ccs,
                  attachments: attachments);

              // OR if you need to define multiple to addresses
              MultipleToesMailMessage mailMessage = MultipleToesMailMessage.Create(
                  emailToes: ["emailTo@n.1", "emailTo@n.2", "emailTo@n.3", "emailTo@n.4", "emailTo@n.5"],
                  emailSubject: "MailEngine .NET Library Email",
                  emailFrom: "noreply@hype-lab.it",
                  htmlMessage: "<h1>Test email from MailEngine .NET Library</h1>",
                  plainTextContent: "Test email from MailEngine .NET Library",
                  emailToName: "Matt P",
                  emailFromName: "Hype-Lab NoReply"
                  ccs: ccs,
                  attachments: attachments);

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
    // ...

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

**Contributing**

Feedbacks and and Contributions are welcome! Also feel free to open an issue or to ask anything if needed.


**License**

This project is licensed under the MIT License.