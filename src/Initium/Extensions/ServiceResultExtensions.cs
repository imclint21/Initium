namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for enhancing the functionality of <see cref="ServiceResult"/>.
/// </summary>
public static class ServiceResultExtensions
{
	/// <summary>
	/// Converts a <see cref="ServiceResult"/> into a <see cref="ServiceResult{TData}"/> by attaching the specified data.
	/// </summary>
	/// <typeparam name="TData">The type of the data to attach to the result.</typeparam>
	/// <param name="baseResult">The base <see cref="ServiceResult"/> to which the data will be added.</param>
	/// <param name="data">The data to attach to the result.</param>
	/// <returns>
	/// A new <see cref="ServiceResult{TData}"/> instance containing the same status, message, and status code as the original result,
	/// but enriched with the specified data.
	/// </returns>
	public static ServiceResult<TData> WithData<TData>(this ServiceResult baseResult, TData data) => new()
	{
		Success = baseResult.Success,
		Message = baseResult.Message,
		StatusCode = baseResult.StatusCode,
		Data = data
	};
}