using Microsoft.Win32;
using Styles.Library.Helpers;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using ThumbnailSharp;


namespace ToolkitForTSW
	{
	public class CScreenshot : Notifier
		{
/*
Path to the screenshot
*/
		private FileInfo _Path;

		public FileInfo Path
			{
			get { return _Path; }
			set
				{
				_Path = value;
				OnPropertyChanged("Path");
				}
			}

/*
If a steam screenshot, value is true, if TSW screenshot value is false
*/
		private Boolean _SteamType;

		public Boolean SteamType
			{
			get { return _SteamType; }
			set
				{
				_SteamType = value;
				OnPropertyChanged("SteamType");
				}
			}

/*
ThumbNail generated for this screenshot
*/
		private BitmapSource _ThumbNail;

		public BitmapSource ThumbNail
			{
			get { return _ThumbNail; }
			set
				{
				_ThumbNail = value;
				OnPropertyChanged("ThumbNail");
				}
			}

		public CScreenshot(FileInfo MyPath, Boolean MySteamType)
			{
			Path = MyPath;
			SteamType = MySteamType;
			}

		public Boolean Exists()
			{
			return File.Exists(Path.FullName);
			}

		public void CreateThumbNail()
			{
			if (ThumbNail != null)
				{
				return;
				}

			var MyFormat = Format.Png;
			if (SteamType)
				{
				MyFormat = Format.Jpeg;
				}

			Byte[] ResultBytes = new ThumbnailCreator().CreateThumbnailBytes(
				thumbnailSize: 180,
				imageFileLocation: Path.FullName,
				imageFormat: MyFormat);

			ThumbNail = ByteArraytoBitmap(ResultBytes);
			}

		public static BitmapImage ByteArraytoBitmap(Byte[] ByteArray)
			{
			MemoryStream Stream = new MemoryStream(ByteArray);

			BitmapImage BitmapImage = new BitmapImage();
			BitmapImage.BeginInit();
			BitmapImage.StreamSource = Stream;
			BitmapImage.EndInit();

			return BitmapImage;
			}

		override public String ToString()
			{
			String FileName = Path.Name;
			if (SteamType)
				{
				return "Steam " + FileName;
				}
			else
				{
				return "Game " + FileName;
				}
			}

		public static void SaveScreenShotAs(String ScreenShotPath, String InitialDirectory)
			{
			var Form = new SaveFileDialog();
			Form.InitialDirectory = InitialDirectory;
			Form.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
			Form.FileName = System.IO.Path.GetFileName(ScreenShotPath) ?? String.Empty;
			Form.Title = "Save screenshot with a new name";
			if (Form.ShowDialog() == true)
				{
				File.Copy(ScreenShotPath, Form.FileName, true);
				}
			}
		}
	}
