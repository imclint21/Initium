using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Initium.Attributes;

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
    public static string GetDefaultMessageForStatusCode(int statusCode) => statusCode switch
    {
        200 => "The operation completed successfully.",
        201 => "The resource was created successfully.",
        204 => "The request was successful, but there is no content to return.",
        400 => "One or more validation errors occurred.",
        401 => "Access is denied due to invalid credentials",
        403 => "Access is forbidden, you do not have the necessary permissions to perform this operation.",
        404 => "The requested resource could not be found.",
        409 => "A conflict occurred, the request could not be completed due to conflicting changes.",
        503 => "The service is currently unavailable.",
        _ => "An internal server error occurred."
    };
}
