using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Initium.Attributes;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ApiResponseAttribute : Attribute
{
	public int StatusCode { get; }
	public string Message { get; }

	public ApiResponseAttribute(HttpStatusCode statusCode, string message)
	{
		StatusCode = (int)statusCode;
		Message = message;
	}

	public ApiResponseAttribute(int statusCode, string message)
	{
		StatusCode = statusCode;
		Message = message;
	}
}
