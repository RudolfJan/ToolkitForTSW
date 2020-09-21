using Styles.Library.Helpers;
using System;
using System.IO;


namespace TSWTools
	{
	public class CFilePresenter : Notifier
		{
		#region Properties
		private String _Name = String.Empty;
		public String Name
			{
			get => _Name;
			set
				{
				_Name = value;
				OnPropertyChanged("Name");
				}
			}

		private String _Extension = String.Empty;
		public String Extension
			{
			get => _Extension;
			set
				{
				_Extension = value;
				OnPropertyChanged("Extension");
				}
			}
		private String _FullName = String.Empty;
		public String FullName
			{
			get => _FullName;
			set
				{
				_FullName = value;
				OnPropertyChanged("FullName");
				}
			}
		private DateTimeOffset _CreationDate;
		public DateTimeOffset CreationDate
			{
			get => _CreationDate;
			set
				{
				_CreationDate = value;
				OnPropertyChanged("CreationDate");
				}
			}

		private Boolean _IsInstalled = false;
		public Boolean IsInstalled
			{
			get => _IsInstalled;
			set
				{
				_IsInstalled = value;
				OnPropertyChanged("IsInstalled");
				}
			}

		private Boolean _IsOutdated = false;
		public Boolean IsOutdated
			{
			get => _IsOutdated;
			set
				{
				_IsOutdated = value;
				OnPropertyChanged("IsOutdated");
				}
			}


		#endregion

		#region Constructors

		// Create using FullName for file, and optional creation Date/Time
		public CFilePresenter(String MyFullName, DateTimeOffset MyDate)
			{
			FullName = MyFullName;
			Name = Path.GetFileName(FullName);
			Extension = Path.GetExtension(FullName);
			CreationDate = MyDate;
			}

		public CFilePresenter(String MyFullName, String MyName, String MyExtension, DateTimeOffset MyDate)
			{
			FullName = MyFullName;
			Name = MyName;
			Extension = MyExtension;
			CreationDate = MyDate;
			}

		public CFilePresenter(String MyFullName, String MyName, DateTimeOffset MyDate)
			{
			FullName = MyFullName;
			Name = MyName;
			Extension = Path.GetExtension(FullName);
			CreationDate = MyDate;
			}

		public CFilePresenter()
			{
			}

		#endregion

		#region Converters

		public void Parse7ZLine(String Line)
			{
			if (Line.Length >= 53)
				{
				try
					{
					FullName = Line.Substring(53);
					Name = Path.GetFileName(FullName);
					Extension = Path.GetExtension(FullName);
					Int32 Year = Convert.ToInt32(Line.Substring(0, 4));
					Int32 Month = Convert.ToInt32(Line.Substring(5, 2));
					Int32 Day = Convert.ToInt32(Line.Substring(8, 2));
					Int32 Hours = Convert.ToInt32(Line.Substring(11, 2));
					Int32 Minutes = Convert.ToInt32(Line.Substring(14, 2));
					TimeSpan Offset = new TimeSpan();
					CreationDate = new DateTimeOffset(Year, Month, Day, Hours, Minutes, 0, Offset);
					if (Month > 12)
						{
						CLog.Trace("Conversion error", LogEventType.Debug);
						}
					}
				catch (Exception E)
					{
					CLog.Trace("Conversion error in line " + Line + " because " + E.Message, LogEventType.Debug);
					}
				}
			}

		#endregion

		#region Validators

		public Boolean CheckFile()
			{
			var CheckPath = CTSWOptions.LiveriesFolder + "\\" + FullName;
			if (CheckPath.EndsWith("//"))
				{
				return false; //Directory
				}
			if (File.Exists(CheckPath))
				{
				IsInstalled = true;
				}
			if (File.GetLastWriteTime(CheckPath) > CreationDate)
				{
				IsOutdated = true;
				}
			return true;
			}
		#endregion


		}
	}
