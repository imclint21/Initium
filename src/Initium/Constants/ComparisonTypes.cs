using System.Text.Json.Serialization;
using Tapper;

namespace Initium.Constants;

[TranspilationSource]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComparisonTypes
{
	Equals,
	StartsWith,
	Contains
}