using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Initium.Validation;

/// <summary>
/// Validates that a value is a URL-friendly slug: lower-case ASCII letters and digits, with words separated by
/// single hyphens and no leading, trailing, or doubled hyphens (e.g. <c>acme-proxies</c>). This is exactly the
/// shape produced by <see cref="Extensions.SlugExtensions.ToSlug"/>. A null/empty/whitespace value passes (the
/// field is optional).
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class SlugAttribute : ValidationAttribute
{
	private static readonly Regex Pattern = new(
		@"^[a-z0-9]+(?:-[a-z0-9]+)*$",
		RegexOptions.CultureInvariant | RegexOptions.Compiled);

	/// <inheritdoc />
	public override bool IsValid(object? value) =>
		value is not string slug || string.IsNullOrWhiteSpace(slug) || Pattern.IsMatch(slug.Trim());

	/// <inheritdoc />
	public override string FormatErrorMessage(string name) =>
		"Enter a valid slug: lower-case letters, digits, and single hyphens (e.g. acme-proxies).";
}
