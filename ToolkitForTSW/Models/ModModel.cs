namespace ToolkitForTSW.Models
  {
  public class ModModel
    {
    public int Id { get; set; }
    public string ModName { get; set; }=string.Empty;
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string ModDescription { get; set; } = string.Empty;
    public string DLCName { get; set; } = string.Empty;
    public string  ModImage { get; set; } = string.Empty;
    public string ModSource { get; set; } = string.Empty;
    public bool IsInstalled { get; set; }
    public ModTypesEnum ModType { get; set; } = ModTypesEnum.Undefined;
    }
  }
