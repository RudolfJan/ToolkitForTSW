using Styles.Library.Helpers;
using System;
using System.Windows;

namespace ToolkitForTSW
	{
	public class CMain : Notifier
		{
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

		public CMain()
			{
			while (!CTSWOptions.GetNotFirstRun())
				{
				MessageBox.Show(@"Please complete the configuration before you proceed", @"Set configuration",
					MessageBoxButton.OK,MessageBoxImage.Asterisk);

				CTSWOptions.ReadFromRegistry();

				var InitialInstallDirectory = CTSWOptions.TSWToolsFolder;
				var Form = new FormOptions();

				Form.ShowDialog();
				if (Form.DialogResult == true)
					{
					CTSWOptions.WriteToRegistry();
					CTSWOptions.UpdateTSWToolsDirectory(InitialInstallDirectory);
					}
				}
			if (CTSWOptions.TSWToolsFolder.Length > 1)
				{
				CTSWOptions.CreateDirectories();
				CTSWOptions.MoveManuals();
				}
			}

		public void OpenManual()
			{
			Result += CApps.OpenGenericFile(CTSWOptions.ManualsFolder + "ToolkitForTSW Manual "+ CTSWOptions.TSWToolsManualVersion+".pdf");
			}

		public void OpenStartersGuide()
			{
			Result += CApps.OpenGenericFile(CTSWOptions.ManualsFolder + "TSW2 Starters guide.pdf");
			}
		}
	}