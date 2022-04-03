using Caliburn.Micro;
using Filter.Library.Filters.DataAccess;
using Logging.Library;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToolkitForTSW.GameSave.DataAccess;
using ToolkitForTSW.GameSave.Models;
using Utilities.Library;
using Utilities.Library.Filters.Models;

namespace ToolkitForTSW.GameSave.ViewModels
  {
  public class GameSaveViewModel : Screen
    {

    #region properties
    private bool _activeSaveGame;

    public bool ActiveSaveGame
      {
      get { return _activeSaveGame; }
      set
        {
        _activeSaveGame = value;
        NotifyOfPropertyChange(nameof(ActiveSaveGame));
        NotifyOfPropertyChange(nameof(CanSaveActive));
        NotifyOfPropertyChange(nameof(CanSaveAuto));
        NotifyOfPropertyChange(nameof(CanSaveCheckpoint));
        }
      }

    private bool _autoSaveGame;

    public bool AutoSaveGame
      {
      get { return _autoSaveGame; }
      set
        {
        _autoSaveGame = value;
        NotifyOfPropertyChange(nameof(AutoSaveGame));
        NotifyOfPropertyChange(nameof(CanUseAutoSave));
        }
      }

    private bool _checkpointSaveGame;

    public bool CheckpointSaveGame
      {
      get { return _checkpointSaveGame; }
      set
        {
        _checkpointSaveGame = value;
        NotifyOfPropertyChange(nameof(CheckpointSaveGame));
        NotifyOfPropertyChange(nameof(CanUseCheckpointSave));
        }
      }

    private BindableCollection<string> _profileList = new BindableCollection<string>();

    public BindableCollection<string> ProfileList
      {
      get { return _profileList; }
      set { _profileList = value; }
      }
    private string _selectedProfile;

    public string SelectedProfile
      {
      get { return _selectedProfile; }
      set
        {
        _selectedProfile = value;
        GetSaveGameFiles();
        NotifyOfPropertyChange(nameof(CanSaveActive));
        NotifyOfPropertyChange(nameof(CanSaveAuto));
        NotifyOfPropertyChange(nameof(CanSaveCheckpoint));
        NotifyOfPropertyChange(nameof(CanUseArchivedGameSave));
        }
      }

    public List<GameSaveModel> GameSaveList { get; set; } = new List<GameSaveModel>();

    private BindableCollection<GameSaveModel> _filteredGameSaveList;

    public BindableCollection<GameSaveModel> FilteredGameSaveList
      {
      get { return _filteredGameSaveList; }
      set { _filteredGameSaveList = value; }
      }

    private GameSaveModel _selectedGameSaveModel;

    public GameSaveModel SelectedGameSave
      {
      get { return _selectedGameSaveModel; }
      set
        {
        _selectedGameSaveModel = value;
        NotifyOfPropertyChange(nameof(CanUseArchivedGameSave));
        NotifyOfPropertyChange(nameof(CanDeleteGameSave));
        }
      }

    private string _saveName = string.Empty;

    public string SaveName
      {
      get { return _saveName; }
      set
        {
        _saveName = value;
        NotifyOfPropertyChange(nameof(SaveName));
        NotifyOfPropertyChange(nameof(CanSaveActive));
        NotifyOfPropertyChange(nameof(CanSaveAuto));
        NotifyOfPropertyChange(nameof(CanSaveCheckpoint));
        }
      }

    private string _description = string.Empty;

    public string Description
      {
      get { return _description; }
      set
        {
        _description = value;
        NotifyOfPropertyChange(nameof(Description));
        NotifyOfPropertyChange(nameof(CanSaveActive));
        NotifyOfPropertyChange(nameof(CanSaveAuto));
        NotifyOfPropertyChange(nameof(CanSaveCheckpoint));
        }
      }

    private string _TagPattern = string.Empty;
    public string TagPattern
      {
      get { return _TagPattern; }
      set
        {
        _TagPattern = value;
        NotifyOfPropertyChange(nameof(TagPattern));
        }
      }

    private TagCategoriesExtendedModel _SelectedRouteTag;
    public TagCategoriesExtendedModel SelectedRouteTag
      {
      get { return _SelectedRouteTag; }
      set
        {
        _SelectedRouteTag = value;
        NotifyOfPropertyChange(nameof(SelectedRouteTag));
        NotifyOfPropertyChange(nameof(CanSaveActive));
        NotifyOfPropertyChange(nameof(CanSaveAuto));
        NotifyOfPropertyChange(nameof(CanSaveCheckpoint));
        }
      }

    private BindableCollection<TagCategoriesExtendedModel> _FilteredRouteTagList;
    public BindableCollection<TagCategoriesExtendedModel> FilteredRouteTagList
      {
      get { return _FilteredRouteTagList; }
      set
        {
        _FilteredRouteTagList = value;
        NotifyOfPropertyChange(nameof(FilteredRouteTagList));
        }
      }

    private TagCategoriesExtendedModel _selectedActivityTag;
    public TagCategoriesExtendedModel SelectedActivityTag
      {
      get { return _selectedActivityTag; }
      set
        {
        _selectedActivityTag = value;
        NotifyOfPropertyChange(nameof(SelectedActivityTag));
        NotifyOfPropertyChange(nameof(CanSaveActive));
        NotifyOfPropertyChange(nameof(CanSaveAuto));
        NotifyOfPropertyChange(nameof(CanSaveCheckpoint));
        }
      }

    private BindableCollection<TagCategoriesExtendedModel> _activityTagList;
    public BindableCollection<TagCategoriesExtendedModel> ActivityTagList
      {
      get { return _activityTagList; }
      set
        {
        _activityTagList = value;
        NotifyOfPropertyChange(nameof(ActivityTagList));
        }
      }
    #endregion

    public GameSaveViewModel()
      {
      Init();
      }

    public void Init()
      {
      GetProfiles();
      GetTags();
      GameSaveList = (List<GameSaveModel>)GameSaveDataAccess.GetAllGameSaves();
      FilteredGameSaveList = new BindableCollection<GameSaveModel>(GameSaveList
          .OrderBy(x => x.RouteAbbreviation));
      }

    public void GetProfiles()
      {
      var dir = new DirectoryInfo($"{TSWOptions.GameSaveLocation}Saved\\SaveGames\\");
      var files = dir.GetFiles("PP_*.sav");

      foreach (var file in files)
        {
        var profile = file.Name.Substring(3, file.Name.Length - 7);
        ProfileList.Add(profile);
        }
      if (ProfileList.Count == 1)
        {
        SelectedProfile = ProfileList[0];
        }
      }

    public void GetTags()
      {
      ActivityTagList = new BindableCollection<TagCategoriesExtendedModel>(TagCategoriesExtendedDataAccess
         .GetFilteredTagsAndCategories("Activity", "*")
         .OrderBy(x => x.TagName));
      FilteredRouteTagList = new BindableCollection<TagCategoriesExtendedModel>(TagCategoriesExtendedDataAccess
         .GetFilteredTagsAndCategories("Route", "*")
         .OrderBy(x => x.TagName));
      }

    public void GetSaveGameFiles()
      {
      var dir = new DirectoryInfo($"{TSWOptions.GameSaveLocation}Saved\\SaveGames\\");
      var files = dir.GetFiles($"TSWSaveGame_{SelectedProfile}.sav");
      ActiveSaveGame = files.Length == 1;
      files = dir.GetFiles($"TSWAutoSaveGame_{SelectedProfile}.sav");
      AutoSaveGame = files.Length == 1;
      files = dir.GetFiles($"TSWCheckpointSaveGame_{SelectedProfile}.sav");
      CheckpointSaveGame = files.Length == 1;
      }

    public bool CanUseAutoSave
      {
      get
        {
        return AutoSaveGame && SelectedProfile != null;
        }
      }

    public void UseAutoSave()
      {
      var baseDir = $"{TSWOptions.GameSaveLocation}Saved\\SaveGames\\";
      FileHelpers.DeleteSingleFile($"{baseDir}TSWSaveGame_{SelectedProfile}.sav");
      File.Copy($"{baseDir}TSWAutoSaveGame_{SelectedProfile}.sav", $"{baseDir}TSWSaveGame_{SelectedProfile}.sav");
      FileHelpers.DeleteSingleFile($"{baseDir}TSWAutoSaveGame_{SelectedProfile}.sav");
      GetSaveGameFiles();
      }

    public bool CanUseCheckpointSave
      {
      get
        {
        return CheckpointSaveGame && SelectedProfile != null;
        }
      }

    public void UseCheckpointSave()
      {
      var baseDir = $"{TSWOptions.GameSaveLocation}Saved\\SaveGames\\";
      FileHelpers.DeleteSingleFile($"{baseDir}TSWSaveGame_{SelectedProfile}.sav");
      File.Copy($"{baseDir}TSWCheckpointSaveGame_{SelectedProfile}.sav", $"{baseDir}TSWSaveGame_{SelectedProfile}.sav");
      FileHelpers.DeleteSingleFile($"{baseDir}TSWCheckpointSaveGame_{SelectedProfile}.sav");
      GetSaveGameFiles();
      }

    public bool CanUseArchivedGameSave
      {
      get
        {
        return SelectedGameSave != null && SelectedProfile != null;
        }
      }

    public void UseArchivedGameSave()
      {
      var destination = $"{TSWOptions.GameSaveLocation}Saved\\SaveGames\\TSWSaveGame_{SelectedProfile}.sav";
      var source = $"{TSWOptions.SaveGameArchiveFolder}{SelectedGameSave.RouteAbbreviation}-{SelectedGameSave.SaveName}-{SelectedGameSave.Activity}.sav";
      File.Copy(source, destination, true);
      Log.Trace($"Activated game save as requested using {SelectedGameSave.SaveName}", LogEventType.InformUser);
      GetSaveGameFiles();
      }

    public bool CanDeleteGameSave
      {
      get
        {
        return SelectedGameSave != null;
        }
      }
    public void DeleteGameSave()
      {
      var path = $"{TSWOptions.SaveGameArchiveFolder}{SelectedGameSave.RouteAbbreviation}-{SelectedGameSave.SaveName}-{SelectedGameSave.Activity}.sav";
      FileHelpers.DeleteSingleFile(path);
      GameSaveDataAccess.DeleteGameSave(SelectedGameSave.Id);
      GameSaveList.Remove(SelectedGameSave);
      FilteredGameSaveList = new BindableCollection<GameSaveModel>(GameSaveList.OrderBy(x => x.RouteAbbreviation));
      NotifyOfPropertyChange(nameof(FilteredGameSaveList));
      SelectedGameSave = null;
      }


    public bool CanSaveActive
      {
      get
        {
        return SelectedActivityTag != null
        && SelectedRouteTag != null
        && SaveName?.Length > 1
        && ActiveSaveGame
        && SelectedProfile != null;
        }
      }


    public void SaveActive()
      {
      Save("TSWSaveGame");
      }

    public bool CanSaveAuto
      {
      get
        {
        return SelectedActivityTag != null
        && SelectedRouteTag != null
        && SaveName?.Length > 1
        && AutoSaveGame
        && SelectedProfile != null;
        }
      }

    public void SaveAuto()
      {
      Save("TSWAutoSaveGame");
      }



    public bool CanSaveCheckpoint
      {
      get
        {
        return SelectedActivityTag != null
        && SelectedRouteTag != null
        && SaveName?.Length > 1
        && CheckpointSaveGame
        && SelectedProfile != null;
        }
      }

    public void SaveCheckpoint()
      {
      Save("TSWCheckpointSaveGame");
      }


    private void Save(string saveType)
      {
      var source = $"{TSWOptions.GameSaveLocation}Saved\\SaveGames\\{saveType}_{SelectedProfile}.sav";
      var destination = $"{TSWOptions.SaveGameArchiveFolder}{SelectedRouteTag.TagName}-{SaveName}-{SelectedActivityTag.TagName}.sav";
      var newGameSave = new GameSaveModel();
      newGameSave.SaveName = SaveName;
      newGameSave.Description = Description;
      newGameSave.RouteAbbreviation = SelectedRouteTag.TagName;
      newGameSave.Activity = SelectedActivityTag.TagName;
      newGameSave.Id = GameSaveDataAccess.InsertGameSave(newGameSave);
      File.Copy(source, destination, true);
      GameSaveList.Add(newGameSave);
      FilteredGameSaveList = new BindableCollection<GameSaveModel>(GameSaveList.OrderBy(x => x.RouteAbbreviation));
      NotifyOfPropertyChange(nameof(FilteredGameSaveList));
      GetSaveGameFiles();
      }


    public void ClearGameSave()
      {
      SaveName = "";
      Description = "";
      SelectedRouteTag = null;
      SelectedActivityTag = null;
      }

    public Task CloseForm()
      {
      return TryCloseAsync();
      }
    }
  }
