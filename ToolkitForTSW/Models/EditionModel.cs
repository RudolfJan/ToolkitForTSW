namespace ToolkitForTSW.Models
  {
  public class EditionModel
    {
    public int Id { get; set; } = 0;
    public int EditionOrder { get; set; }
    public string EditionName { get; set; } = "";
    public string EditionLongName { get; set; } = "";
    public string SteamGameId { get; set; } = "";
    public string SteamProgramPath { get; set; } = "";
    public string EGSProgramPath { get; set; } = "";
    public int Selected { get; set; } = 0;
    public string Description { get; set; } = "";
    }
  }
