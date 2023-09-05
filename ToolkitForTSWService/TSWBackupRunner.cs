using Serilog;

namespace ToolkitForTSWService
  {
  public class TSWBackupRunner
    {
    //private readonly ILogger _logger;

    public TSWBackupRunner()
      {
      Log.Information("Program started");
      }

    private readonly TSWBackup tswBackup = new TSWBackup();


    public void ExecuteAsync(CancellationToken stoppingToken)
      {
      while (!stoppingToken.IsCancellationRequested)
        {
        Log.Information("Backup process started");
        TSWServiceOptions.ReadFromRegistry();
        tswBackup.MakeDailyBackup();
        Thread.Sleep(20000);
        // await Task.Delay(24 * 60 * 60 * 1000, stoppingToken);
        }
      }
    }
  }