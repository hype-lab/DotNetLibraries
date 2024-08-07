# HypeLab.RxPatternsResolver
Provides a class capable of solve collections of regex patterns given an input string. Also equipped with a default patterns set.
Also exposes methods that validate the email address format and optionally validates domain; also checks if an email address exists.

## (Optional) Register type

On startup:
```c#
builder.Services.AddRegexResolver();
```

**Using with DI**
```c#
public class Example
{
   private readonly RegexPatternsResolver _rxResolver;
	
   public Example(RegexPatternsResolver rxResolver)
   {
	_rxResolver = rxResolver;
   }
}
```

## Pattern resolver usage example
```c#
using HypeLab.RxPatternsResolver;
using HypeLab.RxPatternsResolver.Constants;

// ...

const string tst = @"Hi i do tes#TS s@ds a\a  b/b°?mlkm";

_rxResolver.AddPattern(RxResolverConst.DefaultBadCharsCollectionPattern1, string.Empty);
_rxResolver.AddPattern(@"[/\\]", " - ");
string output = _rxResolver.ResolveStringWithPatterns(tst);

Console.WriteLine($"Old string:{Environment.NewLine}{tst}" +
    Environment.NewLine + Environment.NewLine +
    $"New string:{Environment.NewLine}{output}");
	
// Old string:
// Hi i do tes#TS s@ds a\a  b/b°?mlkm

// New string:
// Hi i do tesTS sds a - a  b - b?mlkm
```

## Email address validation
```c#
EmailCheckerResponse resp = await _rxResolver.IsValidEmailAsync("john.doe@gmail.com", checkDomain: true);
Console.WriteLine($"{resp.Message} - Status: {resp.ResponseStatus}");
// OUTPUT: john.doe@gmail.com results as a valid email address - Status: EMAIL_VALID

EmailCheckerResponse resp2 = _rxResolver.IsValidEmail("john.doe@gmail.com", checkDomain: true);
Console.WriteLine($"{resp2.Message} - Status: {resp2.ResponseStatus}");
// OUTPUT: john.doe@gmail.com results as a valid email address - Status: EMAIL_VALID

EmailCheckerResponse resp3 = _rxResolver.IsValidEmail("john.doe@gmail.com");
Console.WriteLine($"{resp3.Message} - Status: {resp3.ResponseStatus}");
// OUTPUT: john.doe@gmail.com results as a valid email address - Status: EMAIL_VALID
```

## Email address existence check
```c#
EmailCheckerResponse resp = await _rxResolver.IsEmailExistingAsync("john.doe@gmail.com");
Console.WriteLine($"{resp.Message} - Status: {resp.ResponseStatus}");
// OUTPUT: john.doe@gmail.com exists - Status: EMAIL_VALID

EmailCheckerResponse resp2 = _rxResolver.IsEmailExisting("john.doe@gmail.com");
Console.WriteLine($"{resp2.Message} - Status: {resp2.ResponseStatus}");
// OUTPUT: john.doe@gmail.com exists - Status: EMAIL_VALID
```
