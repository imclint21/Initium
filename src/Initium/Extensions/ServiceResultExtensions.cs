using Initium.Results;

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
	/// <param name="serviceResult">The base <see cref="ServiceResult"/> to which the data will be added.</param>
	/// <param name="data">The data to attach to the result.</param>
	/// <returns>
	/// A new <see cref="ServiceResult{TData}"/> instance containing the same status, message, and status code as the original result,
	/// but enriched with the specified data.
	/// </returns>
	public static ServiceResult<TData> WithData<TData>(this ServiceResult serviceResult, TData data) => new()
	{
		Success = serviceResult.Success,
		Message = serviceResult.Message,
		StatusCode = serviceResult.StatusCode,
		Data = data
	};

	/// <summary>
	/// Converts a <see cref="ServiceResult"/> into a typed <see cref="ServiceResult{TData}"/> with the data set to its default value.
	/// </summary>
	/// <typeparam name="TData">The type of the data for the result.</typeparam>
	/// <param name="serviceResult">The base <see cref="ServiceResult"/> to be converted.</param>
	/// <returns>
	/// A new <see cref="ServiceResult{TData}"/> instance that contains the same status, message, and status code as the original result,
	/// with the data initialized to the default value of the specified type.
	/// </returns>
	public static ServiceResult<TData> As<TData>(this ServiceResult serviceResult) => new()
	{
		Success = serviceResult.Success,
		Message = serviceResult.Message,
		StatusCode = serviceResult.StatusCode,
		Data = default
	};

	public static async Task<TData?> Unwrap<TData>(this Task<ServiceResult<TData>> task) =>
		(await task).Unwrap();

	public static async Task<TData> UnwrapOrThrow<TData>(this Task<ServiceResult<TData>> task, HttpStatusCode? statusCode = null, string? message = null) =>
		(await task).UnwrapOrThrow(statusCode, message);

	public static async Task<TData> UnwrapOr<TData>(this Task<ServiceResult<TData>> task, TData fallback) =>
		(await task).UnwrapOr(fallback);

	public static async Task<ServiceResult<TData>> WithData<TData>(this Task<ServiceResult> task, TData data) =>
		(await task).WithData(data);

	public static async Task<ServiceResult<TData>> As<TData>(this Task<ServiceResult> task) =>
		(await task).As<TData>();

	// Sync result -> async next (untyped)
	public static async Task<ServiceResult> ChainWith(this ServiceResult result, Func<Task<ServiceResult>> next) =>
		!result ? result : await next();

	// Sync result -> async next (typed)
	public static async Task<ServiceResult<TData>> ChainWith<TData>(this ServiceResult result, Func<Task<ServiceResult<TData>>> next) =>
		!result ? result.As<TData>() : await next();

	// Task result -> sync next (untyped)
	public static async Task<ServiceResult> ChainWith(this Task<ServiceResult> task, Func<ServiceResult> next) =>
		(await task).ChainWith(next);

	// Task result -> sync next (typed)
	public static async Task<ServiceResult<TData>> ChainWith<TData>(this Task<ServiceResult> task, Func<ServiceResult<TData>> next) =>
		(await task).ChainWith(next);

	// Task result -> async next (untyped)
	public static async Task<ServiceResult> ChainWith(this Task<ServiceResult> task, Func<Task<ServiceResult>> next)
	{
		var result = await task;
		return !result ? result : await next();
	}

	// Task result -> async next (typed)
	public static async Task<ServiceResult<TData>> ChainWith<TData>(this Task<ServiceResult> task, Func<Task<ServiceResult<TData>>> next)
	{
		var result = await task;
		return !result ? result.As<TData>() : await next();
	}
}
