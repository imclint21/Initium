using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Initium.Attributes;

namespace Initium.Helpers;

internal static class ApiResponseHelper
{
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
}
