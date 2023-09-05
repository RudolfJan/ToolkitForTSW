using Serilog;

namespace ToolkitForTSWService
  {

  public class Program
    {

    private static void Main(string[] args)
      {
      Log.Logger = new LoggerConfiguration()
         .WriteTo.Console()
         .WriteTo.File(@"D:\ToolkitForTSWData\logging.txt")
         .CreateLogger();

      Log.Information("Program started");
      TSWServiceOptions.ReadFromRegistry();

      var backupRunner = new TSWBackupRunner();
      backupRunner.ExecuteAsync(new CancellationToken());
      }
    }
  }
