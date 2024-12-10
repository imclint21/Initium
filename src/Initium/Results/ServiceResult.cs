using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;
using Initium.Exceptions;
using Initium.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Initium.Results;

/// <summary>
/// Represents a result for a service operation, indicating success or failure with an optional message and status code.
/// </summary>
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class ServiceResult : BaseResult
{
    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    [JsonIgnore]
    public bool Failed => !Success;

    /// <summary>
    /// Creates a successful <see cref="ServiceResult"/> with an optional message and status code.
    /// </summary>
    /// <param name="message">An optional message describing the result.</param>
    /// <param name="statusCode">The HTTP status code associated with the result. Defaults to <see cref="HttpStatusCode.OK"/>.</param>
    /// <returns>A successful <see cref="ServiceResult"/>.</returns>
    public static ServiceResult Ok(string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK) => new()
    {
        Success = true,
        Message = message,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a successful <see cref="ServiceResult"/> with a specified status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the result.</param>
    /// <returns>A successful <see cref="ServiceResult"/>.</returns>
    public static ServiceResult Ok(HttpStatusCode statusCode) => new()
    {
        Success = true,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with an optional message and status code.
    /// </summary>
    /// <param name="message">An optional message describing the error.</param>
    /// <param name="statusCode">The HTTP status code associated with the error. Defaults to <see cref="HttpStatusCode.BadRequest"/>.</param>
    /// <returns>A failed <see cref="ServiceResult"/>.</returns>
    public static ServiceResult Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
    {
        Success = false,
        Message = message,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with a specified status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the error.</param>
    /// <returns>A failed <see cref="ServiceResult"/>.</returns>
    public static ServiceResult Error(HttpStatusCode statusCode) => new()
    {
        Success = false,
        StatusCode = statusCode
    };

    /// <summary>
    /// Chains the current result with another operation if the current result is successful.
    /// </summary>
    /// <param name="next">A function returning the next <see cref="ServiceResult"/>.</param>
    /// <returns>The result of the next operation if the current result is successful; otherwise, the current result.</returns>
    public ServiceResult ChainWith(Func<ServiceResult> next) => !this ? this : next();

    /// <summary>
    /// Converts the current <see cref="ServiceResult"/> into an <see cref="ActionResult"/> for use in ASP.NET Core.
    /// </summary>
    /// <param name="result">The <see cref="ServiceResult"/> to convert.</param>
    public static implicit operator ActionResult(ServiceResult result) => new ObjectResult(result);

    /// <summary>
    /// Implicitly converts a <see cref="ServiceResult"/> to a <see cref="bool"/>, returning <c>true</c> if the result is successful.
    /// </summary>
    /// <param name="result">The <see cref="ServiceResult"/> to convert.</param>
    public static implicit operator bool(ServiceResult result) => result.Success;
}

/// <summary>
/// Represents a result for a service operation that returns data, indicating success or failure with an optional message and status code.
/// </summary>
/// <typeparam name="TData">The type of the data returned in the result.</typeparam>
public class ServiceResult<TData> : ServiceResult
{
    /// <summary>
    /// Gets or sets the data returned by the service operation.
    /// </summary>
    public TData? Data { get; set; }

    /// <summary>
    /// Creates a successful <see cref="ServiceResult{TData}"/> with the specified data, an optional message, and a status code.
    /// </summary>
    /// <param name="data">The data returned by the operation.</param>
    /// <param name="message">An optional message describing the result.</param>
    /// <param name="statusCode">The HTTP status code associated with the result. Defaults to <see cref="HttpStatusCode.OK"/>.</param>
    /// <returns>A successful <see cref="ServiceResult{TData}"/>.</returns>
    public static ServiceResult<TData> Ok(TData data, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK) => new()
    {
        Success = true,
        Data = data,
        Message = message,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a successful <see cref="ServiceResult{TData}"/> with the specified data and status code.
    /// </summary>
    /// <param name="data">The data returned by the operation.</param>
    /// <param name="statusCode">The HTTP status code associated with the result.</param>
    /// <returns>A successful <see cref="ServiceResult{TData}"/>.</returns>
    public static ServiceResult<TData> Ok(TData data, HttpStatusCode statusCode) => new()
    {
        Success = true,
        Data = data,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with an optional message and status code.
    /// </summary>
    /// <param name="message">An optional message describing the error.</param>
    /// <param name="statusCode">The HTTP status code associated with the error. Defaults to <see cref="HttpStatusCode.BadRequest"/>.</param>
    /// <returns>A failed <see cref="ServiceResult{TData}"/>.</returns>
    public new static ServiceResult<TData> Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
    {
        Success = false,
        Message = message,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with a specified status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the error.</param>
    /// <returns>A failed <see cref="ServiceResult{TData}"/>.</returns>
    public new static ServiceResult<TData> Error(HttpStatusCode statusCode) => new()
    {
        Success = false,
        StatusCode = statusCode
    };

    /// <summary>
    /// Implicitly converts a <see cref="ServiceResult{TData}"/> to its contained data if the result is successful.
    /// </summary>
    /// <param name="baseResult">The <see cref="ServiceResult{TData}"/> to convert.</param>
    public static implicit operator TData?(ServiceResult<TData> baseResult) => baseResult.Data;

    /// <summary>
    /// Implicitly converts data of type <typeparamref name="TData"/> into a <see cref="ServiceResult{TData}"/> representing a successful result.
    /// </summary>
    /// <param name="data">The data to wrap in a <see cref="ServiceResult{TData}"/>.</param>
    public static implicit operator ServiceResult<TData>(TData? data) => new() { Data = data };

    /// <summary>
    /// Unwraps the data contained in the result, throwing an exception if the result is unsuccessful or the data is null.
    /// </summary>
    /// <returns>The unwrapped data.</returns>
    /// <exception cref="ApiException">Thrown if the result is unsuccessful or the data is null.</exception>
    public TData Unwrap() => Data ?? throw new ApiException(HttpStatusCode.InternalServerError, "Unwrapping failed, data is null.");
}
