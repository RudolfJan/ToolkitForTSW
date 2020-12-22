namespace ToolkitForTSW.Models
  {
  public class LiveryModel
    {
    public int Id { get; set; }
    public string LiveryName { get; set; }
    public string FileName { get; set; }
    public string LiveryDescription { get; set; }
    public string  LiveryImage { get; set; }
    public string LiverySource { get; set; }
    public LiveryTypesEnum LiveryType { get; set; }
    public int RouteId { get; set; }
    }
  }
