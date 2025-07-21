namespace Initium.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class CustomHeaderAttribute(string headerName, string headerValue) : Attribute
{
	public string HeaderName { get; } = headerName;
	public string HeaderValue { get; } = headerValue;
}