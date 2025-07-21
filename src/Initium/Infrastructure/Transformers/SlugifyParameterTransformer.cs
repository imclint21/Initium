using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Initium.Infrastructure.Transformers;

internal partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
	[GeneratedRegex("([a-z])([A-Z])")]
	private static partial Regex SlugRegex();
	
	public string TransformOutbound(object? value) =>
        SlugRegex().Replace(value?.ToString() ?? string.Empty, "$1-$2").ToLower();
}