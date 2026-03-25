using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Initium.Workers;

/// <summary>
/// Provides a base class for recurring background workers with configurable cycle delays and launch conditions.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedParameter.Global")]
public abstract class BaseWorker(ILogger? logger = null, TimeSpan? cycleDelay = null) : BackgroundService
{
	private CancellationTokenSource _restartCts = new();

	/// <summary>
	/// Gets or sets the delay between each execution cycle. Defaults to 30 seconds.
	/// </summary>
	protected TimeSpan CycleDelay { get; set; } = cycleDelay ?? TimeSpan.FromSeconds(30);

	/// <summary>
	/// Gets or sets optional conditions that must all be true for the worker to execute.
	/// </summary>
	public bool[]? LaunchConditions { get; set; }

	/// <summary>
	/// Performs the worker's main logic. Called on each cycle when the worker is eligible.
	/// </summary>
	/// <param name="stoppingToken">A token to observe for cancellation requests.</param>
	protected abstract Task DoWork(CancellationToken stoppingToken);

	/// <summary>
	/// Determines whether the worker is eligible to execute based on <see cref="LaunchConditions"/>.
	/// </summary>
	/// <returns><c>true</c> if all conditions are met or no conditions are set; otherwise, <c>false</c>.</returns>
	protected virtual bool IsEligible() => LaunchConditions == null || LaunchConditions.All(condition => condition);

	/// <inheritdoc />
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _restartCts.Token);

			try
			{
				if (IsEligible())
				{
					await DoWork(linkedCts.Token);
				}
				await Task.Delay(CycleDelay, linkedCts.Token);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				OnError(ex);
			}
		}
	}

	/// <summary>
	/// Called when an unhandled exception occurs during <see cref="DoWork"/>. Logs the error by default.
	/// Override to customize error handling (e.g. alerting, stopping the worker).
	/// </summary>
	/// <param name="exception">The exception that occurred.</param>
	protected virtual void OnError(Exception exception) =>
		logger?.LogError(exception, "Unhandled exception in {Worker}", GetType().Name);

	/// <summary>
	/// Restarts the worker cycle by cancelling the current delay and starting a new one.
	/// </summary>
	protected void Restart()
	{
		_restartCts.Cancel();
		_restartCts.Dispose();
		_restartCts = new CancellationTokenSource();
	}
}