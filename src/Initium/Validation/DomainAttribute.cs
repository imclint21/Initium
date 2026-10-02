using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Initium.Validation;

/// <summary>
/// Validates that a value looks like a bare domain or sub-domain (e.g. <c>acme.com</c>,
/// <c>gw.acme-proxies.com</c>): labels of letters/digits/hyphens separated by dots, with a TLD of 2+ letters,
/// and no scheme, path, or port. A null/empty/whitespace value passes (the field is optional). This checks the
/// shape only — it does not verify that the domain actually resolves.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class DomainAttribute : ValidationAttribute
{
	private static readonly Regex Pattern = new(
		@"^(?=.{1,253}$)([a-z0-9](-?[a-z0-9])*\.)+[a-z]{2,}$",
		RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

	/// <inheritdoc />
	public override bool IsValid(object? value) =>
		value is not string domain || string.IsNullOrWhiteSpace(domain) || Pattern.IsMatch(domain.Trim());

	/// <inheritdoc />
	public override string FormatErrorMessage(string name) =>
		"Enter a valid domain, without http://, a path, or a port.";
}
