namespace Initium.Attributes;

/// <summary>
/// Adds a custom HTTP header to the response. Can be applied multiple times on a class or method.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class CustomHeaderAttribute(string headerName, string headerValue) : Attribute
{
	/// <summary>
	/// Gets the header name.
	/// </summary>
	public string HeaderName { get; } = headerName;

	/// <summary>
	/// Gets the header value.
	/// </summary>
	public string HeaderValue { get; } = headerValue;
}