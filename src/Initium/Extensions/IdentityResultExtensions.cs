using Microsoft.AspNetCore.Identity;

namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IdentityResult"/>.
/// </summary>
public static class IdentityResultExtensions
{
	/// <summary>
	/// Gets a value indicating whether the identity operation has failed.
	/// Equivalent to <c>!result.Succeeded</c>.
	/// </summary>
	/// <param name="result">The identity result.</param>
	/// <returns><c>true</c> if the operation failed; otherwise, <c>false</c>.</returns>
	public static bool HasFailed(this IdentityResult result) => !result.Succeeded;
}
