using System.Net;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Initium.Attributes;
using Microsoft.AspNetCore.Http;

namespace Initium.Helpers;

/// <summary>
/// Provides helper methods for managing API responses and extracting attributes related to API responses.
/// </summary>
internal static class ApiResponseHelper
{
    /// <summary>
    /// Retrieves all <see cref="ApiResponseAttribute"/> instances applied to the given action or its controller.
    /// </summary>
    /// <param name="actionDescriptor">The action descriptor representing the current action.</param>
    /// <returns>
    /// A collection of <see cref="ApiResponseAttribute"/> instances applied to the action and its controller.
    /// If no attributes are found, an empty collection is returned.
    /// </returns>
    public static IEnumerable<ApiResponseAttribute> GetApiResponseAttributes(ActionDescriptor actionDescriptor)
    {
        if (actionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
            return [];

        var methodAttributes = controllerActionDescriptor.MethodInfo
            .GetCustomAttributes(typeof(ApiResponseAttribute), inherit: true)
            .Cast<ApiResponseAttribute>();

        var controllerAttributes = controllerActionDescriptor.ControllerTypeInfo
            .GetCustomAttributes(typeof(ApiResponseAttribute), inherit: true)
            .Cast<ApiResponseAttribute>();

        return methodAttributes.Concat(controllerAttributes);
    }

    /// <summary>
    /// Provides a default message for a given HTTP status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code for which a default message is requested.</param>
    /// <returns>
    /// A string containing the default message associated with the status code.
    /// If the status code is unrecognized, a generic internal server error message is returned.
    /// </returns>
    public static string GetDefaultMessageForStatusCode(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.OK => "The operation completed successfully.",
        HttpStatusCode.Created => "The resource was created successfully.",
        HttpStatusCode.NoContent => "The request was successful, but there is no content to return.",
        HttpStatusCode.BadRequest => "One or more validation errors occurred.",
        HttpStatusCode.Unauthorized => "Access is denied due to invalid credentials.",
        HttpStatusCode.Forbidden => "Access is forbidden, you do not have the necessary permissions to perform this operation.",
        HttpStatusCode.NotFound => "The requested resource could not be found.",
        HttpStatusCode.Conflict => "A conflict occurred, the request could not be completed due to conflicting changes.",
        HttpStatusCode.ServiceUnavailable => "The service is currently unavailable.",
        _ => "An internal server error occurred."
    };
    
    public static HttpStatusCode GetDefaultStatusCode(HttpContext context) => context.Request.Method.ToUpper() switch
    {
        "POST" => HttpStatusCode.Created,
        "DELETE" => HttpStatusCode.NoContent,
        _ => (HttpStatusCode)context.Response.StatusCode
    };
}
