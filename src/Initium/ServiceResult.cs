using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Initium.Exceptions;
using Initium.Infrastructure;

namespace Initium;

public class ServiceResult : BaseResult
{
	[JsonIgnore]
	public bool Failed => !Success;
	
	public static ServiceResult Ok(string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK) => new()
	{
		Success = true,
		Message = message,
		StatusCode = statusCode
	};

	public static ServiceResult Ok(HttpStatusCode statusCode) => new()
	{
		Success = true,
		StatusCode = statusCode
	};

	public static ServiceResult Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
	{
		Success = false,
		Message = message,
		StatusCode = statusCode
	};

	public static ServiceResult Error(HttpStatusCode statusCode) => new()
	{
		Success = false,
		StatusCode = statusCode
	};
	
	public ServiceResult ChainWith(Func<ServiceResult> next) => !this ? this : next();

	public static implicit operator ActionResult(ServiceResult result) => new ObjectResult(result);
	public static implicit operator bool(ServiceResult result) => result.Success;
}

public class ServiceResult<TData> : ServiceResult
{
	public TData? Data { get; set; }

	public static ServiceResult<TData> Ok(TData data, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK) => new()
	{
		Success = true,
		Data = data,
		Message = message,
		StatusCode = statusCode
	};

	public static ServiceResult<TData> Ok(TData data, HttpStatusCode statusCode) => new()
	{
		Success = true,
		Data = data,
		StatusCode = statusCode
	};

	public new static ServiceResult<TData> Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
	{
		Success = false,
		Message = message,
		StatusCode = statusCode
	};

	public new static ServiceResult<TData> Error(HttpStatusCode statusCode) => new()
	{
		Success = false,
		StatusCode = statusCode
	};
	
	public static implicit operator TData?(ServiceResult<TData> baseResult) => baseResult.Data;
	public static implicit operator ServiceResult<TData>(TData? data) => new() { Data = data };
	
	public TData Unwrap() => Data ?? throw new ApiException(HttpStatusCode.InternalServerError, "Unwrapping failed, data is null.");
}
