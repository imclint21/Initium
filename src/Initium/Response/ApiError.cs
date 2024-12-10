using Tapper;

namespace Initium.Response;

[TranspilationSource]
public class ApiError
{
	public string? Code { get; set; }
	public string? Description { get; set; }

	public ApiError()
	{
	}

	public ApiError(string code, string description)
	{
		Code = code;
		Description = description;
	}
}