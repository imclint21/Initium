using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Initium.Attributes;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ApiResponseAttribute(HttpStatusCode statusCode, string message) : Attribute
{
	public HttpStatusCode StatusCode { get; } = statusCode;
	public string Message { get; } = message;

	public ApiResponseAttribute(int statusCode, string message) : this((HttpStatusCode)statusCode, message)
	{
	}
}
