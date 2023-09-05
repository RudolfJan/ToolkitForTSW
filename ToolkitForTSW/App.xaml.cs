namespace ToolkitForTSW
  {
  public partial class App
    {
    public App()
      {
      Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Bootstrapper.Config["SycfusionLicense"]);
      }
    }
  }
