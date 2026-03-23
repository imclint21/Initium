using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Initium.Infrastructure.Transformers;

/// <summary>
/// Transforms PascalCase route parameters into kebab-case (e.g., <c>ProductCategories</c> becomes <c>product-categories</c>).
/// </summary>
internal partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
	[GeneratedRegex("([a-z])([A-Z])")]
	private static partial Regex SlugRegex();

	/// <inheritdoc />
	public string TransformOutbound(object? value) =>
        SlugRegex().Replace(value?.ToString() ?? string.Empty, "$1-$2").ToLower();
}