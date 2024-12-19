using System.Net;

namespace Initium.Extensions;

public static class HttpStatusCodeExtensions
{
	public static bool IsSuccess(this HttpStatusCode statusCode) => 
		statusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices;
}