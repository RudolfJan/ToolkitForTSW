using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;
using Styles.Library.Helpers;

namespace ToolkitForTSW
    {
    public class CScreenshotManager : Notifier
        {
        #region Properties

/*
Collection of all screenshots
*/
        private ObservableCollection<CScreenshot> _ScreenshotList = null;

        public ObservableCollection<CScreenshot> ScreenshotList
            {
            get { return _ScreenshotList; }
            set
                {
                _ScreenshotList = value;
                OnPropertyChanged("ScreenshotList");
                }
            }

        private ObservableCollection<CScreenshot> _DisplayScreenshotList = null;

        public ObservableCollection<CScreenshot> DisplayScreenshotList
            {
            get { return _DisplayScreenshotList; }
            set
                {
                _DisplayScreenshotList = value;
                OnPropertyChanged("DisplayScreenshotList");
                }
            }

        private UInt32 _DisplayRangeOffset = 0;

        public UInt32 DisplayRangeOffset
            {
            get { return _DisplayRangeOffset; }
            set
                {
                _DisplayRangeOffset = value;
                OnPropertyChanged("DisplayRangeOffset");
                }
            }

        private UInt32 _DisplayRangeCount = 15;

        public UInt32 DisplayRangeCount
            {
            get { return _DisplayRangeCount; }
            set
                {
                _DisplayRangeCount = value;
                OnPropertyChanged("DisplayRangeCount");
                }
            }

        private UInt32 _ScreenshotCount = 0;

        public UInt32 ScreenshotCount
            {
            get { return _ScreenshotCount; }
            set
                {
                _ScreenshotCount = value;
                OnPropertyChanged("ScreenshotCount");
                }
            }

        private String _FormattedPage;
        public String FormattedPage
            {
            get { return _FormattedPage; }
            set
                {
                _FormattedPage = value;
                OnPropertyChanged("FormattedPage");
                }
            }

        private UInt32 _ScreenShotTotalPages;
        public UInt32 ScreenShotTotalPages
            {
            get { return _ScreenShotTotalPages; }
            set
                {
                _ScreenShotTotalPages = value;
                OnPropertyChanged("ScreenShotTotalPages");
                }
            }

        private UInt32 _SteamScreenshotCount = 0;

        public UInt32 SteamScreenshotCount
            {
            get { return _SteamScreenshotCount; }
            set
                {
                _SteamScreenshotCount = value;
                OnPropertyChanged("SteamScreenshotCount");
                }
            }

        private UInt32 _TotalScreenshotCount = 0;

        public UInt32 TotalScreenshotCount
            {
            get { return _TotalScreenshotCount; }
            set
                {
                _TotalScreenshotCount = value;
                OnPropertyChanged("TotalScreenshotCount");
                }
            }

        private String _SelectedImagePath = String.Empty;

        public String SelectedImagePath
            {
            get { return _SelectedImagePath; }
            set
                {
                _SelectedImagePath = value;
                OnPropertyChanged("SelectedImagePath");
                }
            }

        private String _Result = String.Empty;

        public String Result
            {
            get { return _Result; }
            set
                {
                _Result = value;
                OnPropertyChanged("Result");
                }
            }

        #endregion

        #region Constructors

        public CScreenshotManager()
            {
            ScreenshotList = new ObservableCollection<CScreenshot>();
            DisplayScreenshotList = new ObservableCollection<CScreenshot>();
            Result += GetScreenhots(CTSWOptions.SavedScreenshots, false, "png");
            Result += GetScreenhots(CTSWOptions.SavedSteamScreenshots, true, "jpg");
            GetDisplayRange();
            }

        #endregion

        public String GetScreenhots(String Path, Boolean SteamType, String Extension)
            {
            Result = String.Empty;
            if (String.IsNullOrEmpty(Path))
                {
                return Result += "Screenshot directory " + Path + " not found\r\n";
            }
            try
                {
                if (Directory.Exists(Path))
                    {
                    var Dir = new DirectoryInfo(Path);
                    FileInfo[] Files = Dir.GetFiles("*." + Extension);
                    foreach (var F in Files)
                        {
                        CScreenshot S = new CScreenshot(F, SteamType);
                        if (SteamType)
                            {
                            SteamScreenshotCount++;
                            }
                        else
                            {
                            ScreenshotCount++;
                            }

                        TotalScreenshotCount++;
                        ScreenshotList.Add(S);
                        }

                    ScreenShotTotalPages = TotalScreenshotCount / DisplayRangeCount;
                    GetFormattedPage();
                    }

                else
                    {
                    Result += "Screenshot directory " + Path + " not found\r\n";
                    }
                }
            catch (Exception E)
                {
                Result += "Cannot get screenshots: " + E.Message + "\r\n";
                return Result;
                }

            return Result;
            }

        public void GetFormattedPage()
            {
            FormattedPage= $"{DisplayRangeOffset/DisplayRangeCount+1}/{ScreenShotTotalPages+1}";
            }

        public void GetDisplayRange()
            {
            DisplayScreenshotList.Clear();
            for (UInt32 I = DisplayRangeOffset;
                I < Math.Min(DisplayRangeOffset + DisplayRangeCount, ScreenshotList.Count);
                I++)
                {
                var S = ScreenshotList[(Int32) I];
                S.CreateThumbNail();
                DisplayScreenshotList.Add(S);
                }
            GetFormattedPage();
            }
        }
    }
