# HypeLab.RxPatternsResolver
Provides a class capable of solve collections of regex patterns given an input string. Also equipped with a default patterns set.
Also exposes a method that validateS the email address format and optionally validates domain.

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
	
	public Class1(RegexPatternsResolver rxResolver)
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

RegexPatternsResolver resolver = new();
resolver.AddPattern(RxResolverConst.DefaultBadCharsCollectionPattern1, string.Empty);
resolver.AddPattern(@"[/\\]", " - ");
string output = resolver.ResolveStringWithPatterns(tst);

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
    RegexPatternsResolver resolver = new();
    EmailCheckerResponse resp = await resolver.IsValidEmailAsync("john.doe@gmail.com", checkDomain: true).ConfigureAwait(false);

    Console.WriteLine($"{resp.Message} - Status: {resp.ResponseStatus}");
    // OUTPUT: john.doe@gmail.com results as a valid email address - Status: EMAIL_VALID

    EmailCheckerResponse resp2 = resolver.IsValidEmail("john.doe@gmail.com");
    Console.WriteLine($"{resp2.Message} - Status: {resp2.ResponseStatus}");
    // OUTPUT: john.doe@gmail.com results as a valid email address - Status: EMAIL_VALID
```
