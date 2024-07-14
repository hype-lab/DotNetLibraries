using HypeLab.RxPatternsResolver.Exceptions;
using HypeLab.RxPatternsResolver.Helpers;
using HypeLab.RxPatternsResolver.Interfaces;
using HypeLab.RxPatternsResolver.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver
{
	/// <summary>
	/// Class capable of solve collections of regex patterns given an input string. Also equipped with a default patterns set.
	/// </summary>
	public class RegexPatternsResolver : IEmailValidable
	{
		private Stack<RegexPatternInstance>? _patterns;

		private readonly RegexOptions RegexOption = RegexOptions.None;

		/// <summary>
		/// Class initialization without parameters. Must add patterns before resolve a string.
		/// </summary>
		public RegexPatternsResolver() { }

		/// <summary>
		/// Class initialization with essential parameters.
		/// </summary>
		public RegexPatternsResolver(string pattern, string replacement)
		{
			AddPattern(pattern, replacement);
		}

		/// <summary>
		/// Class initialization with all parameters.
		/// </summary>
		public RegexPatternsResolver(string pattern, string replacement, RegexOptions regexOption)
		{
			RegexOption = regexOption;
			AddPattern(pattern, replacement, RegexOption);
		}

		/// <summary>
		/// Adds a new Regex pattern into patterns collection.
		/// Throws exception if pattern is null.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		public void AddPattern(string pattern, string replacement, RegexOptions? regexOption = null)
		{
			if (string.IsNullOrWhiteSpace(pattern))
				throw new ArgumentException("Input string cannot be null or empty", nameof(pattern));

			_patterns ??= new Stack<RegexPatternInstance>();

			_patterns!.Push(new RegexPatternInstance()
			{
				Pattern = pattern, Replacement = replacement, RegexOption = regexOption ?? RegexOption
			});
		}

		/// <summary>
		/// Returns input string replaced using patterns previously added.
		/// </summary>
		/// <returns>
		/// Throws exception if patterns collection is null.
		/// Returns just input string if patterns collection is empty.
		/// Otherwise returns the elaborated string.
		/// </returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="Exception"></exception>
		public string ResolveStringWithPatterns(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentException("String to replace cannot be null or empty", nameof(input));

			if (_patterns == null)
				throw new InvalidOperationException("Patterns collection is null. Do you have added some patterns before resolve?");

			string resolvedString = input;
			try
			{
				if (_patterns!.Count > 0)
				{
					if (_patterns!.Count == 1)
					{
						RegexPatternInstance inst = _patterns!.Peek();
						Regex codeTitleRegex = new Regex(inst.Pattern, inst.RegexOption);
						resolvedString = codeTitleRegex.Replace(resolvedString, inst.Replacement ?? string.Empty);
					}
					else
					{
						foreach (RegexPatternInstance pattern in _patterns!)
						{
							Regex codeTitleRegex = new Regex(pattern.Pattern, pattern.RegexOption);
							resolvedString = codeTitleRegex.Replace(resolvedString, pattern.Replacement ?? string.Empty);
						}
					}
				}

				return resolvedString;
			}
			catch (ArgumentException argumentException)
			{
				throw new ArgumentException($"[Param: {argumentException.ParamName} - Source: {argumentException.Source}] - {argumentException.Message}", argumentException.InnerException);
			}
			catch (Exception ex)
			{
				throw new RxPatternResolverException(ex.Message, ex.InnerException);
			}
		}

		/// <summary>
		/// Determines whether the email format is valid for an email address.
		/// Also offers the possibility to check email domain sending a request to the google dns to verify if domain is valid.
		/// see: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
		/// see: https://developers.google.com/speed/public-dns/docs/doh
		/// </summary>
		/// <param name="email">The provided email address</param>
		/// <param name="checkDomain">If true checks for domain validity</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="HttpRequestException"></exception>
		/// <exception cref="RegexMatchTimeoutException"></exception>
		/// <exception cref="Exception"></exception>
		public async Task<EmailCheckerResponse> IsValidEmailAsync(string email, bool checkDomain = false)
        {
			if (string.IsNullOrWhiteSpace(email))
                return new EmailCheckerResponse("Input string is null or empty", EmailCheckerResponseStatus.INPUT_NULL_OR_EMPTY);

            try
			{
				EmailChecker emailChecker = new EmailChecker();

				// Normalize the domain
                if (emailChecker.IsValidEmailAddress(email.NormalizeEmailDomain()))
                {
					if (checkDomain)
                    {
						EmailCheckerResponseStatus domainStatus =
                            await emailChecker.IsDomainValidAsync(EmailHelper.RetrieveRequestUrlWithGivenDomain(email.GetDomain())).ConfigureAwait(false);

						if (domainStatus == EmailCheckerResponseStatus.DOMAIN_NOT_VALID)
							return new EmailCheckerResponse($"Domain \"{email.GetDomain()}\" is not valid.", domainStatus);
					}

					return new EmailCheckerResponse($"{email} results as a valid email address");
                }
                else
                {
                    return new EmailCheckerResponse("Email address is not valid", EmailCheckerResponseStatus.EMAIL_NOT_VALID);
                }
			}
			catch (ArgumentNullException e)
			{
				throw new ArgumentNullException(e.Message, e.InnerException);
			}
			catch (HttpRequestException e)
            {
				throw new HttpRequestException(e.Message, e.InnerException);
            }
			catch (RegexMatchTimeoutException e)
			{
				throw new RegexMatchTimeoutException(e.Message, e.InnerException);
			}
			catch (Exception e)
			{
				throw new RxPatternResolverException(e.Message, e.InnerException);
			}
        }

		/// <summary>
		/// Determines whether the email format is valid for an email address.
		/// see: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
		/// see: https://developers.google.com/speed/public-dns/docs/doh
		/// </summary>
		/// <param name="email">The provided email address</param>
		/// <exception cref="RegexMatchTimeoutException"></exception>
		/// <exception cref="Exception"></exception>
		public EmailCheckerResponse IsValidEmail(string email)
        {
			if (string.IsNullOrWhiteSpace(email))
				return new EmailCheckerResponse("Input string is null or empty", EmailCheckerResponseStatus.INPUT_NULL_OR_EMPTY);

			try
			{
				EmailChecker emailChecker = new EmailChecker();

				// Normalize the domain
				if (emailChecker.IsValidEmailAddress(email.NormalizeEmailDomain()))
					return new EmailCheckerResponse($"{email} results as a valid email address");
				else
					return new EmailCheckerResponse("Email address is not valid", EmailCheckerResponseStatus.EMAIL_NOT_VALID);
			}
			catch (RegexMatchTimeoutException e)
			{
				throw new RegexMatchTimeoutException(e.Message, e.InnerException);
			}
			catch (Exception e)
			{
				throw new RxPatternResolverException(e.Message, e.InnerException);
			}
		}
    }
}
