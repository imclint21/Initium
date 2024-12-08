using System.Net;
using Microsoft.AspNetCore.Mvc;
using Initium.Exceptions;
using Initium.Infrastructure;

namespace Initium;

public class ServiceResult : BaseResult
{
	public bool Failed => !Success;
	
	public static ServiceResult Ok(string? message = null) => new()
	{
		Success = true,
		Message = message,
		StatusCode = HttpStatusCode.OK
	};

	public static ServiceResult Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
	{
		Success = false, 
		Message = message,
		StatusCode = statusCode
	};
	
	public ServiceResult ChainWith(Func<ServiceResult> next) => !this ? this : next();

	public static implicit operator ActionResult(ServiceResult result) => new ObjectResult(result);
	public static implicit operator bool(ServiceResult result) => result.Success;
}

public class ServiceResult<TData> : ServiceResult
{
	public TData? Data { get; set; }

	public static ServiceResult<TData> Ok(TData data, string? message = null) => new()
	{
		Success = true,
		Message = message,
		StatusCode = HttpStatusCode.OK,
		Data = data
	};
	
	public static ServiceResult<TData> Error(string? message = null, HttpStatusCode? statusCode = HttpStatusCode.BadRequest) => new()
	{
		Success = false,
		StatusCode =  statusCode,
		Message = message
	};
	
	public TData Unwrap() => Data ?? throw new ApiException(HttpStatusCode.InternalServerError, "Unwrapping failed, data is null.");
}