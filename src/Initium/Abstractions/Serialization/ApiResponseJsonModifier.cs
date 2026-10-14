using System.Text.Json.Serialization.Metadata;
using Initium.Response;

namespace Initium.Abstractions.Serialization;

/// <summary>
/// Contract resolver modifier that omits null properties of <see cref="ApiResponse"/> from the JSON body,
/// scoped to that type so other payloads keep their explicit nulls. Applied at serialization time rather than
/// with per-property attributes, which would also drop the properties from the generated client types.
/// </summary>
internal static class ApiResponseJsonModifier
{
	public static void OmitNullProperties(JsonTypeInfo typeInfo)
	{
		if (typeInfo.Type != typeof(ApiResponse)) return;

		foreach (var property in typeInfo.Properties)
			property.ShouldSerialize = (_, value) => value is not null;
	}
}
