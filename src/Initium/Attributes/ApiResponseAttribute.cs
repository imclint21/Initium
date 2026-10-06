using System.Diagnostics.CodeAnalysis;
using System.Net;
using Initium.Response;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Initium.Attributes;

/// <summary>
/// Documents a possible API response for an endpoint, specifying a status code and description message.
/// It also feeds the ApiExplorer (Swagger/OpenAPI): a failure status (4xx/5xx) is advertised as returning an
/// <see cref="ApiResponse"/> body, so there is no need to repeat <c>[ProducesResponseType&lt;ApiResponse&gt;]</c>.
/// A success status (2xx) advertises only the status code, leaving the body type to the developer's own
/// <c>[ProducesResponseType&lt;T&gt;]</c> (success returns the bare entity, not an envelope).
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ApiResponseAttribute(HttpStatusCode statusCode, string message) : Attribute, IApiResponseMetadataProvider
{
	/// <summary>
	/// Gets the HTTP status code for this response.
	/// </summary>
	public HttpStatusCode StatusCode { get; } = statusCode;

	/// <summary>
	/// Gets the description message for this response.
	/// </summary>
	public string Message { get; } = message;

	/// <summary>
	/// Initializes a new instance of <see cref="ApiResponseAttribute"/> with an integer status code.
	/// </summary>
	/// <param name="statusCode">The HTTP status code as an integer.</param>
	/// <param name="message">The description message.</param>
	public ApiResponseAttribute(int statusCode, string message) : this((HttpStatusCode)statusCode, message)
	{
	}

	/// <inheritdoc />
	int IApiResponseMetadataProvider.StatusCode => (int)StatusCode;

	/// <summary>
	/// The response body type reported to the ApiExplorer: <see cref="ApiResponse"/> for a failure status
	/// (4xx/5xx), and <see cref="void"/> for a success status so the developer's own
	/// <c>[ProducesResponseType&lt;T&gt;]</c> describes the success body.
	/// </summary>
	Type? IApiResponseMetadataProvider.Type => (int)StatusCode >= 400 ? typeof(ApiResponse) : typeof(void);

	/// <inheritdoc />
	void IApiResponseMetadataProvider.SetContentTypes(MediaTypeCollection contentTypes)
	{
		// Only failures carry a JSON ApiResponse body; leave success untouched so it isn't forced to JSON here.
		if ((int)StatusCode >= 400)
		{
			contentTypes.Clear();
			contentTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
		}
	}
}
