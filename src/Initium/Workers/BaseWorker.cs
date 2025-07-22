using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;

namespace Initium.Workers;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedParameter.Global")]
public abstract class BaseWorker(TimeSpan? cycleDelay = null) : BackgroundService
{
	private CancellationTokenSource _restartCts = new();
	protected TimeSpan CycleDelay { get; set; } = cycleDelay ?? TimeSpan.FromSeconds(30);
	public bool[]? LaunchConditions { get; set; }

	protected abstract Task DoWork(CancellationToken stoppingToken);
	
	protected virtual bool IsEligibleAsync() => LaunchConditions == null || LaunchConditions.All(condition => condition);

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// if (IsEligibleAsync()) 
		// 	logger.LogInformation("[{Worker}] Starting worker...", GetType().Name);

		while (!stoppingToken.IsCancellationRequested)
		{
			using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _restartCts.Token);

			try
			{
				if (IsEligibleAsync())
				{
					await DoWork(linkedCts.Token);
				}
				await Task.Delay(CycleDelay, linkedCts.Token);
			}
			catch (OperationCanceledException)
			{
				// logger.LogTrace("Service execution canceled.");
			}
			catch (Exception ex)
			{
				// logger.LogError(ex, "Unexpected error occurred.");
			}
		}
	}

	protected void Restart()
	{
		_restartCts.Cancel();
		_restartCts.Dispose();
		_restartCts = new CancellationTokenSource();
	}
}