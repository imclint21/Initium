using System.Net;

namespace Initium.Results;

/// <summary>
/// Provides extension methods for <see cref="ServiceResult"/> and <see cref="ServiceResult{TData}"/>,
/// including async variants for unwrapping, converting, and chaining results.
/// </summary>
public static class ServiceResultExtensions
{
	/// <summary>
	/// Converts a <see cref="ServiceResult"/> into a <see cref="ServiceResult{TData}"/> by attaching the specified data.
	/// </summary>
	/// <typeparam name="TData">The type of the data to attach.</typeparam>
	/// <param name="serviceResult">The base result.</param>
	/// <param name="data">The data to attach.</param>
	/// <returns>A new <see cref="ServiceResult{TData}"/> with the original status and the specified data.</returns>
	public static ServiceResult<TData> WithData<TData>(this ServiceResult serviceResult, TData data) => new()
	{
		Success = serviceResult.Success,
		Message = serviceResult.Message,
		StatusCode = serviceResult.StatusCode,
		Data = data
	};

	/// <summary>
	/// Converts a <see cref="ServiceResult"/> into a typed <see cref="ServiceResult{TData}"/> with default data.
	/// </summary>
	/// <typeparam name="TData">The target data type.</typeparam>
	/// <param name="serviceResult">The base result to convert.</param>
	/// <returns>A new <see cref="ServiceResult{TData}"/> preserving the original status, message, and status code.</returns>
	public static ServiceResult<TData> As<TData>(this ServiceResult serviceResult) => new()
	{
		Success = serviceResult.Success,
		Message = serviceResult.Message,
		StatusCode = serviceResult.StatusCode,
		Data = default
	};

	/// <summary>
	/// Asynchronously unwraps the data from a <see cref="ServiceResult{TData}"/>, returning null if absent.
	/// </summary>
	public static async Task<TData?> Unwrap<TData>(this Task<ServiceResult<TData>> task) =>
		(await task).Unwrap();

	/// <summary>
	/// Asynchronously unwraps the data or throws an <see cref="Exceptions.ApiException"/> if null.
	/// </summary>
	public static async Task<TData> UnwrapOrThrow<TData>(this Task<ServiceResult<TData>> task, HttpStatusCode? statusCode = null, string? message = null) =>
		(await task).UnwrapOrThrow(statusCode, message);

	/// <summary>
	/// Asynchronously unwraps the data or returns the specified fallback value if null.
	/// </summary>
	public static async Task<TData> UnwrapOr<TData>(this Task<ServiceResult<TData>> task, TData fallback) =>
		(await task).UnwrapOr(fallback);

	/// <summary>
	/// Asynchronously converts a <see cref="ServiceResult"/> into a <see cref="ServiceResult{TData}"/> by attaching data.
	/// </summary>
	public static async Task<ServiceResult<TData>> WithData<TData>(this Task<ServiceResult> task, TData data) =>
		(await task).WithData(data);

	/// <summary>
	/// Asynchronously converts a <see cref="ServiceResult"/> into a typed <see cref="ServiceResult{TData}"/> with default data.
	/// </summary>
	public static async Task<ServiceResult<TData>> As<TData>(this Task<ServiceResult> task) =>
		(await task).As<TData>();

	/// <summary>
	/// Chains a <see cref="ServiceResult"/> with an async operation. Stops if the current result has failed.
	/// </summary>
	public static async Task<ServiceResult> ChainWith(this ServiceResult result, Func<Task<ServiceResult>> next) =>
		!result ? result : await next();

	/// <summary>
	/// Chains a <see cref="ServiceResult"/> with a typed async operation. Stops if the current result has failed.
	/// </summary>
	public static async Task<ServiceResult<TData>> ChainWith<TData>(this ServiceResult result, Func<Task<ServiceResult<TData>>> next) =>
		!result ? result.As<TData>() : await next();

	/// <summary>
	/// Chains a <see cref="Task{ServiceResult}"/> with a sync operation. Stops if the awaited result has failed.
	/// </summary>
	public static async Task<ServiceResult> ChainWith(this Task<ServiceResult> task, Func<ServiceResult> next) =>
		(await task).ChainWith(next);

	/// <summary>
	/// Chains a <see cref="Task{ServiceResult}"/> with a typed sync operation. Stops if the awaited result has failed.
	/// </summary>
	public static async Task<ServiceResult<TData>> ChainWith<TData>(this Task<ServiceResult> task, Func<ServiceResult<TData>> next) =>
		(await task).ChainWith(next);

	/// <summary>
	/// Chains a <see cref="Task{ServiceResult}"/> with an async operation. Stops if the awaited result has failed.
	/// </summary>
	public static async Task<ServiceResult> ChainWith(this Task<ServiceResult> task, Func<Task<ServiceResult>> next)
	{
		var result = await task;
		return !result ? result : await next();
	}

	/// <summary>
	/// Chains a <see cref="Task{ServiceResult}"/> with a typed async operation. Stops if the awaited result has failed.
	/// </summary>
	public static async Task<ServiceResult<TData>> ChainWith<TData>(this Task<ServiceResult> task, Func<Task<ServiceResult<TData>>> next)
	{
		var result = await task;
		return !result ? result.As<TData>() : await next();
	}
}
