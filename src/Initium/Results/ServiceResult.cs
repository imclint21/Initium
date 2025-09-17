using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Initium.Exceptions;
using Initium.Response;
using Microsoft.AspNetCore.Identity;
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
    /// Creates a successful <see cref="ServiceResult"/> with an implicit 200 (handled by filters).
    /// </summary>
    public static ServiceResult Ok() => new()
    {
        Success = true
    };

    /// <summary>
    /// Creates a successful <see cref="ServiceResult"/> with a message.
    /// </summary>
    public static ServiceResult Ok(string message) => new()
    {
        Success = true,
        Message = message
    };

    /// <summary>
    /// Creates a successful <see cref="ServiceResult"/> with a specified status code.
    /// </summary>
    public static ServiceResult Ok(HttpStatusCode statusCode) => new()
    {
        Success = true,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a successful <see cref="ServiceResult"/> with a message and status code.
    /// </summary>
    public static ServiceResult Ok(string message, HttpStatusCode statusCode) => new()
    {
        Success = true,
        Message = message,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with default status 500.
    /// </summary>
    public static ServiceResult Error() => new()
    {
        Success = false,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with message and default status 500.
    /// </summary>
    public static ServiceResult Error(string message) => new()
    {
        Success = false,
        Message = message,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with a specific status code.
    /// </summary>
    public static ServiceResult Error(HttpStatusCode statusCode) => new()
    {
        Success = false,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with message and status code.
    /// </summary>
    public static ServiceResult Error(string message, HttpStatusCode statusCode) => new()
    {
        Success = false,
        Message = message,
        StatusCode = statusCode
    };

    // Structured errors
    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with structured errors and default status 500.
    /// </summary>
    public static ServiceResult Error(IEnumerable<ApiError> errors) => new()
    {
        Success = false,
        Errors = errors,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with message and structured errors, default status 500.
    /// </summary>
    public static ServiceResult Error(string message, IEnumerable<ApiError> errors) => new()
    {
        Success = false,
        Message = message,
        Errors = errors,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with structured errors and a specific status code.
    /// </summary>
    public static ServiceResult Error(IEnumerable<ApiError> errors, HttpStatusCode statusCode) => new()
    {
        Success = false,
        Errors = errors,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with message, structured errors and a specific status code.
    /// </summary>
    public static ServiceResult Error(string message, IEnumerable<ApiError> errors, HttpStatusCode statusCode) => new()
    {
        Success = false,
        Message = message,
        Errors = errors,
        StatusCode = statusCode
    };

    
    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with an exception message.
    /// </summary>
    /// <param name="exception">The exception whose message will be used to describe the error.</param>
    /// <returns>A failed <see cref="ServiceResult"/> with an exception message.</returns>
    public static ServiceResult Error(Exception exception) => new()
    {
        Success = false,
        Message = exception.Message,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Chains the current result with another operation if the current result is successful.
    /// </summary>
    /// <param name="next">A function returning the next <see cref="ServiceResult"/>.</param>
    /// <returns>The result of the next operation if the current result is successful; otherwise, the current result.</returns>
    public ServiceResult ChainWith(Func<ServiceResult> next) => !this ? this : next();

    [Obsolete("The message does not look if the operation has failed or not.")]
    public ServiceResult WithMessage(string? message)
    {
        Message = message;
        return this;
    }
    
    /// <summary>
    /// Sets the message based on a selector function that receives the success flag.
    /// </summary>
    /// <param name="messageSelector">A function that selects a message depending on the success of the result.</param>
    /// <returns>The current <see cref="ServiceResult"/> instance with the updated message.</returns>
    public ServiceResult WithMessage(Func<bool, string> messageSelector)
    {
        Message = messageSelector(Success);
        return this;
    }

    /// <summary>
    /// Sets the status code for the result.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to assign.</param>
    /// <returns>The current <see cref="ServiceResult"/> instance with the updated status code.</returns>
    public ServiceResult WithStatusCode(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        return this;
    }
    
    /// <summary>
    /// Sets the status code for the result based on a resolver function that receives the success flag.
    /// </summary>
    /// <param name="statusCodeResolver">A function that determines the status code depending on the success of the result.</param>
    /// <returns>The current <see cref="ServiceResult"/> instance with the updated status code.</returns>
    public ServiceResult WithStatusCode(Func<bool, HttpStatusCode> statusCodeResolver)
    {
        StatusCode = statusCodeResolver(Success);
        return this;
    }
    
    /// <summary>
    /// Adds or updates a metadata entry for the result.
    /// </summary>
    /// <param name="key">The key of the metadata entry.</param>
    /// <param name="value">The value of the metadata entry.</param>
    /// <returns>The current <see cref="ServiceResult"/> instance with the updated metadata.</returns>
    public ServiceResult WithMetadata(string key, string value)
    {
        Metadata[key] = value;
        return this;
    }
    
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

    /// <summary>
    /// Implicitly converts an <see cref="HttpResponseMessage"/> to a <see cref="ServiceResult"/>.
    /// Returns a successful result if the HTTP response indicates success; otherwise, returns a failure.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message to convert.</param>
    /// <returns>A <see cref="ServiceResult"/> representing the success or failure of the HTTP response.</returns>
    public static implicit operator ServiceResult(HttpResponseMessage httpResponseMessage) => httpResponseMessage.IsSuccessStatusCode;
    
    /// <summary>
    /// Implicitly converts a <see cref="ServiceResult"/> to its associated <see cref="HttpStatusCode"/>, if any.
    /// </summary>
    /// <param name="result">The <see cref="ServiceResult"/> instance from which to retrieve the HTTP status code.</param>
    /// <returns>The HTTP status code associated with the result, or <c>null</c> if no status code is set.</returns>
    public static implicit operator HttpStatusCode?(ServiceResult result) => result.StatusCode;
    
    /// <summary>
    /// Converts a boolean value to a <see cref="ServiceResult"/>.
    /// </summary>
    /// <param name="success">
    /// A boolean indicating the result status: <c>true</c> for success, <c>false</c> for an error.
    /// </param>
    /// <returns>
    /// A <see cref="ServiceResult"/> instance representing the success or error state.
    /// </returns>
    public static implicit operator ServiceResult(bool success) => success ? Ok() : Error();
    
    public static implicit operator ServiceResult(IdentityResult identityResult) =>
        identityResult.Succeeded
            ? Ok()
            : Error(identityResult.Errors.Select(identityError => new ApiError(identityError.Code, identityError.Description)), HttpStatusCode.BadRequest);
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
    /// Creates a successful <see cref="ServiceResult{TData}"/> without any data, defaulting to 200 OK.
    /// </summary>
    /// <returns>A successful <see cref="ServiceResult{TData}"/> with no data.</returns>
    public new static ServiceResult<TData> Ok() => new()
    {
        Success = true,
        StatusCode = HttpStatusCode.OK
    };

    /// <summary>
    /// Creates a successful <see cref="ServiceResult{TData}"/> with the specified data, an optional message, and a status code.
    /// </summary>
    /// <param name="data">The data returned by the operation.</param>
    /// <param name="message">An optional message describing the result.</param>
    /// <param name="statusCode">The HTTP status code associated with the result. Defaults to <see cref="HttpStatusCode.OK"/>.</param>
    /// <returns>A successful <see cref="ServiceResult{TData}"/>.</returns>
    public static ServiceResult<TData> Ok(TData data, string? message = null, HttpStatusCode? statusCode = null) => new()
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
    public static ServiceResult<TData> Ok(TData data, HttpStatusCode? statusCode = null) => new()
    {
        Success = true,
        Data = data,
        StatusCode = statusCode ?? HttpStatusCode.OK
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with default status 500.
    /// </summary>
    public new static ServiceResult<TData> Error() => new()
    {
        Success = false,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with message and default status 500.
    /// </summary>
    public new static ServiceResult<TData> Error(string message) => new()
    {
        Success = false,
        Message = message,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with message and status code.
    /// </summary>
    public new static ServiceResult<TData> Error(string message, HttpStatusCode statusCode) => new()
    {
        Success = false,
        Message = message,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with structured errors and default status 500.
    /// </summary>
    public new static ServiceResult<TData> Error(IEnumerable<ApiError> errors) => new()
    {
        Success = false,
        Errors = errors,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with message and structured errors, default status 500.
    /// </summary>
    public new static ServiceResult<TData> Error(string message, IEnumerable<ApiError> errors) => new()
    {
        Success = false,
        Message = message,
        Errors = errors,
        StatusCode = HttpStatusCode.InternalServerError
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with structured errors and status code.
    /// </summary>
    public new static ServiceResult<TData> Error(IEnumerable<ApiError> errors, HttpStatusCode statusCode) => new()
    {
        Success = false,
        Errors = errors,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> with message, structured errors and status code.
    /// </summary>
    public new static ServiceResult<TData> Error(string message, IEnumerable<ApiError> errors, HttpStatusCode statusCode) => new()
    {
        Success = false,
        Message = message,
        Errors = errors,
        StatusCode = statusCode
    };

    /// <summary>
    /// Creates a failed <see cref="ServiceResult{TData}"/> from an exception with default status 500.
    /// </summary>
    public new static ServiceResult<TData> Error(Exception exception) => new()
    {
        Success = false,
        Message = exception.Message,
        StatusCode = HttpStatusCode.InternalServerError
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
    public static implicit operator ServiceResult<TData>(TData? data) => new()
    {
        Success = true,
        Data = data
    };
    
    /// <summary>
    /// Implicitly converts an <see cref="HttpResponseMessage"/> to a <see cref="ServiceResult{TData}"/>.
    /// If the HTTP response indicates success, reads and assigns the response content as <typeparamref name="TData"/>.
    /// Otherwise, sets the <c>Data</c> to <c>default</c>.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message to convert.</param>
    /// <returns>
    /// A <see cref="ServiceResult{TData}"/> representing the success or failure of the HTTP response,
    /// with data deserialized from the response body if successful.
    /// </returns>
    public static implicit operator ServiceResult<TData>(HttpResponseMessage httpResponseMessage) => new()
    {
        Success = httpResponseMessage.IsSuccessStatusCode,
        Data = httpResponseMessage.Content.ReadFromJsonAsync<TData>().Result
    };

    /// <summary>
    /// Implicitly converts an <see cref="IdentityResult"/> into a <see cref="ServiceResult{TData}"/>.
    /// Returns <see cref="Ok()"/> if the identity result succeeded, otherwise returns an <see cref="Error(IEnumerable{ApiError}, HttpStatusCode)"/>.
    /// </summary>
    /// <param name="identityResult">The <see cref="IdentityResult"/> to convert.</param>
    /// <returns>A <see cref="ServiceResult{TData}"/> representing the identity operation outcome.</returns>
    public static implicit operator ServiceResult<TData>(IdentityResult identityResult) =>
        identityResult.Succeeded
            ? Ok()
            : Error(identityResult.Errors.Select(identityError => new ApiError(identityError.Code, identityError.Description)), HttpStatusCode.BadRequest);
    
    /// <summary>
    /// Tries to unwrap the data contained in the result, returning null if the data is null.
    /// </summary> 
    /// <returns>The unwrapped data or null.</returns>
    public TData? Unwrap() => Data;

    /// <summary>
    /// Unwraps the data contained in the result, or throws an <see cref="ApiException"/> if the data is null.
    /// This method is useful for enforcing the presence of data in a service result, with customizable error details.
    /// </summary>
    /// <typeparam name="TData">The type of data contained in the result.</typeparam>
    /// <param name="statusCode">
    /// The HTTP status code to use in the exception if the data is null.
    /// Defaults to <see cref="HttpStatusCode.InternalServerError"/> if not specified and no default status code exists.
    /// </param>
    /// <param name="message">
    /// A custom error message to include in the exception.
    /// If not provided, the method will use the predefined message associated with the result, or a generic message.
    /// </param>
    /// <returns>The unwrapped data of type <typeparamref name="TData"/>.</returns>
    /// <exception cref="ApiException">
    /// Thrown if the data is null. The exception includes the provided or default HTTP status code and message.
    /// </exception>
    public TData UnwrapOrThrow(HttpStatusCode? statusCode = null, string? message = null) => 
        Data ?? throw new ApiException(statusCode ?? StatusCode ?? HttpStatusCode.InternalServerError, message ?? Message);

    /// <summary>
    /// Always throws a new exception of type <typeparamref name="TException"/>.
    /// This method never returns and is typically used to enforce a failure path with a specific exception type.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw.</typeparam>
    /// <returns>This method never returns.</returns>
    /// <exception cref="Exception">Always thrown as the concrete type specified by <typeparamref name="TException"/>.</exception>
    public TData UnwrapOrThrow<TException>() where TException : Exception, new()
    {
        var exception = new TException();
        throw exception switch
        {
            ArgumentException => new ArgumentException("LOL"),
            _ => exception
        };
    }

    /// <summary>
    /// Throws an <see cref="ApiException"/> if the result indicates failure.
    /// </summary>
    /// <param name="statusCode">Optional custom status code to use in the exception.</param>
    /// <param name="message">Optional custom message to use in the exception.</param>
    /// <exception cref="ApiException">Thrown when the result has failed.</exception>
    [Obsolete("Use UnwrapOrThrow or explicit error handling instead.")]
    public void ThrowIfFailed(HttpStatusCode? statusCode = null, string? message = null)
    {
        if (Failed)
        {
            throw new ApiException(statusCode ?? StatusCode ?? HttpStatusCode.InternalServerError, message ?? Message);
        }
    }
}
