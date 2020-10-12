using System;
using System.Collections.Generic;
using System.Diagnostics; // needed for Process class
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Xml;
using System.Xml.Linq;

// static class to invoke external applications
namespace ToolkitForTSW
{
	public static class CApps
		{
    public static String ExecuteFile(FileInfo Filepath, String Arguments = "")
      {
      return ExecuteFile(Filepath.FullName, Arguments);
      }

    public static String ExecuteFile(String Filepath, String Arguments="")
			{
			using (var ExecuteProcess = new Process())
				{
				try
					{
					ExecuteProcess.StartInfo.FileName = Filepath;
					ExecuteProcess.StartInfo.Arguments = Arguments;
          ExecuteProcess.StartInfo.CreateNoWindow = true;
          ExecuteProcess.StartInfo.UseShellExecute = false;
					ExecuteProcess.StartInfo.RedirectStandardOutput = false;
					ExecuteProcess.Start();
					}
				catch (Exception E)
					{
					return CLog.Trace("Error executing .exe " + E.Message, LogEventType.Error);
					}

				return String.Empty;
				}
			}

		public static String EditXmlFile(String XmlFile)
			{
			if (!File.Exists(XmlFile))
				{
				CLog.Trace("Error editing XMLFile " + XmlFile + " because file is not found", LogEventType.Error);
				return "Error editing XMLFile " + XmlFile + " because file is not found";
				}

			using (Process EditScenarioPropertiesProcess = new Process())
				{
				try
					{
					if (CTSWOptions.XmlEditor.Length > 0)
						{
						EditScenarioPropertiesProcess.StartInfo.FileName = CTSWOptions.XmlEditor;
						}
					else
						{
						EditScenarioPropertiesProcess.StartInfo.FileName = "Notepad.exe";
						}

					EditScenarioPropertiesProcess.StartInfo.Arguments = QuoteFilename(XmlFile);
					EditScenarioPropertiesProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
					EditScenarioPropertiesProcess.StartInfo.RedirectStandardOutput = false;
					EditScenarioPropertiesProcess.Start();
					}
				catch (Exception E)
					{
					return CLog.Trace("Error editing XMLFile " + XmlFile + " because " + E.Message, LogEventType.Error);
					}

				return CLog.Trace(
					"XMLFile edited\r\nWARNING: the contents of this screen may be invalid until you finished editing the XML File! You may need to refresh the contents manually using the refresh button",
					LogEventType.Message);
				}
			}



		// Start FileCompare Tool
		public static String FileCompareTool(String InputFile, String OutputFile, String InputDescription,
			String OutputDescription)
			{
			using (Process FileCompareProcess = new Process())
				{
				var Result = String.Empty;
				if (CTSWOptions.FileCompare.Length == 0)
					{
					return CLog.Trace("File Compare Tool not set in Options", LogEventType.Error);
					}

				try
					{
					FileCompareProcess.StartInfo.FileName = CTSWOptions.FileCompare;
					FileCompareProcess.StartInfo.Arguments = QuoteFilename(InputFile) + " " + QuoteFilename(OutputFile) + " /dl \"" + InputDescription +
					                                         "\" /dr \"" + OutputDescription + "\"";
					FileCompareProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
					FileCompareProcess.StartInfo.UseShellExecute = false;
					FileCompareProcess.StartInfo.RedirectStandardOutput = false;
					FileCompareProcess.StartInfo.RedirectStandardError = false;
					FileCompareProcess.Start();
					}
				catch (Exception E)
					{
					return CLog.Trace("Error using File Compare Tool " + E.Message + "\r\n" + Result, LogEventType.Error);
					}

				return Result;
				}
			}



		public static String CompileInstallerScript(String InputFile, String InstallerOptions = "")
			{
			using (Process Installer = new Process())
				{
				var InstallerName = CTSWOptions.Installer;
				if (!File.Exists(InstallerName))
					{
					return CLog.Trace("Inno Setup not found, expected " + InstallerName, LogEventType.Message);
					}

				try
					{
					Installer.StartInfo.FileName = InstallerName;
					Installer.StartInfo.Arguments = InstallerOptions + " \"" + InputFile + "\"";
					Installer.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					Installer.StartInfo.CreateNoWindow = true;
					Installer.StartInfo.UseShellExecute = false;
					Installer.StartInfo.RedirectStandardOutput = true;
					Installer.StartInfo.RedirectStandardError = true;
					Installer.Start();
					var R = Installer.StandardOutput.ReadToEnd();
					R += Installer.StandardError.ReadToEnd();
					Installer.WaitForExit();
					return CLog.Trace(R + "\r\nInstaller created " + InstallerName);
					}
				catch (Exception E)
					{
					return CLog.Trace("Error creating installer " + E.Message, LogEventType.Error);
					}
				}
			}

		public static String UnPack(String InputFile, String OutputDirectory)
			{
			String Result = String.Empty;
			try
				{
				using (Process UnpackProcess = new Process())
					{
					UnpackProcess.StartInfo.FileName = CTSWOptions.Unpacker;
					UnpackProcess.StartInfo.Arguments = "\"" + InputFile + "\" -extract " + QuoteFilename(OutputDirectory);
					UnpackProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					UnpackProcess.StartInfo.CreateNoWindow = true;
					UnpackProcess.StartInfo.UseShellExecute = false;
					UnpackProcess.StartInfo.RedirectStandardOutput = true;
					UnpackProcess.Start();
		
					while (!UnpackProcess.StandardOutput.EndOfStream)
						{
						Console.WriteLine(UnpackProcess.StandardOutput.ReadLine());
						}
					UnpackProcess.WaitForExit();
					Result += UnpackProcess.StandardOutput.ReadToEnd();
					}
				return "";
				}
			catch (Exception E)
				{
				return "Error using Unpack " + InputFile+ " " +E.Message +"\r\n";
				}
			}

		public static String UnPackAsset(String CommandLine)
			{
			var Result = String.Empty;
			if (CTSWOptions.UAssetUnpacker.Length == 0)
				{
				Result += CLog.Trace("UAssetUnpacker location is not set in options", LogEventType.Error);
				return Result;
				}
			try
				{
				using (Process UnpackProcess = new Process())
					{
					UnpackProcess.StartInfo.FileName = CTSWOptions.UAssetUnpacker;
					UnpackProcess.StartInfo.Arguments = CommandLine;
					UnpackProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					UnpackProcess.StartInfo.CreateNoWindow = true;
					UnpackProcess.StartInfo.UseShellExecute = false;
					UnpackProcess.StartInfo.RedirectStandardOutput = true;
					UnpackProcess.Start();

					while (!UnpackProcess.StandardOutput.EndOfStream)
						{
						Result+= UnpackProcess.StandardOutput.ReadLine()+"\r\n";
						}
					UnpackProcess.WaitForExit();
					Result += UnpackProcess.StandardOutput.ReadToEnd();
					}
				return Result;
				}
			catch (Exception E)
				{
				Result += CLog.Trace("Error using UAssetUnpacker " + CommandLine + " " + E.Message,
					LogEventType.Error);
				return Result;
				}
			}

 public static String OpenGenericFile(String Filepath)
			{
			try
				{
				if (Filepath.Contains("\'"))
					{
					//TODO fix this work around
					return CLog.Trace("Cannot open file " + Filepath + " because it contains single quotes. Remove the qute from the file path", LogEventType.Error);
					}

				if (File.Exists(Filepath))
					{
					using (var OpenFileProcess = new Process())
						{
						OpenFileProcess.StartInfo.FileName = "explorer.exe";
						OpenFileProcess.StartInfo.Arguments = QuoteFilename(Filepath);
						OpenFileProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
						OpenFileProcess.StartInfo.RedirectStandardOutput = false;
						OpenFileProcess.Start();
						return String.Empty;
						}
					}
				}
			catch (Exception E)
				{
				return CLog.Trace("Cannot open file " + Filepath + " reason: " + E.Message, LogEventType.Error);
				}

			return CLog.Trace("Cannot find file " + Filepath + " \r\nMake sure to install it at the correct location");
			}

		// Open folder from the application
		public static String OpenFolder(String FolderPath)
			{
			using (Process EditScriptProcess = new Process())
				{
				if (!Directory.Exists(FolderPath))
					{
					return "Directory does not exist " + FolderPath;
					}

				try
					{
					EditScriptProcess.StartInfo.FileName = "explorer.exe";
					EditScriptProcess.StartInfo.Arguments = QuoteFilename(FolderPath);
					EditScriptProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
					EditScriptProcess.StartInfo.RedirectStandardOutput = false;
					EditScriptProcess.Start();
					return String.Empty;
					}
				catch (Exception E)
					{
					return CLog.Trace("Cannot open directory " + FolderPath + " reason: " + E.Message, LogEventType.Error);
					}
				}
			}

		// Safe way to delete a single file
		public static String DeleteSingleFile(String FilePath)
			{
			if (File.Exists(FilePath))
				{
				// Use a try block to catch IOExceptions, to
				// handle the case of the file already being
				// opened by another process.
				try
					{
					File.Delete(FilePath);
					return String.Empty;
					}
				catch (IOException E)
					{
					Console.WriteLine(E.Message);
					return CLog.Trace("Cannot delete" + FilePath + "because " + E.Message, LogEventType.Message);
					}
				}

			return String.Empty;
			}

		public static void CreateEmptyFile(String Filename)
			{
			File.Create(Filename).Dispose();
			}

		public static String EditTextFile(String Filepath)
			{
			if (!File.Exists(Filepath))
				{
				CreateEmptyFile(Filepath);
				}

			using (Process EditScriptProcess = new Process())
				{
				try
					{
					if (CTSWOptions.TextEditor.Length > 0)
						{
						EditScriptProcess.StartInfo.FileName = CTSWOptions.TextEditor;
						}
					else
						{
						EditScriptProcess.StartInfo.FileName = "Notepad.exe";
						}

					EditScriptProcess.StartInfo.Arguments = QuoteFilename(Filepath);
					EditScriptProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
					EditScriptProcess.StartInfo.RedirectStandardOutput = false;
					EditScriptProcess.Start();
					}
				catch (Exception E)
					{
					return CLog.Trace("Error editing file " + Filepath + " " + E.Message, LogEventType.Error);
					}

				return String.Empty;
				}
			}

		public static String OpenZipFile(String FilePath)
			{
			if (File.Exists(FilePath))
				{
				using (var EditScriptProcess = new Process())
					{
					try
						{
						EditScriptProcess.StartInfo.FileName = "7zFM.exe";
						EditScriptProcess.StartInfo.Arguments = QuoteFilename(FilePath);
						EditScriptProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
						EditScriptProcess.StartInfo.RedirectStandardOutput = false;
						EditScriptProcess.Start();
						return String.Empty;
						}
					catch (Exception E)
						{
						return CLog.Trace("Cannot open zip file " + FilePath + " reason: " + E.Message, LogEventType.Error);
						}
					}
				}

			return String.Empty;
			}

		// This extraction will NOT take into account the filepath
		public static String ExtractZipFile(String Archive, String ArchiveEntry, String OutputDirectory)
			{
			if (File.Exists(Archive))
				{
				using (var MyProcess = new Process())
					{
					try
						{
						MyProcess.StartInfo.FileName = CTSWOptions.SevenZip;
						MyProcess.StartInfo.Arguments =
							"-y e \"" + Archive + "\" -o\"" + OutputDirectory + "\" \"" + ArchiveEntry + "\"";
						MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						MyProcess.StartInfo.CreateNoWindow = true;
						MyProcess.StartInfo.RedirectStandardOutput = true;
						MyProcess.StartInfo.RedirectStandardError = true;
						MyProcess.StartInfo.UseShellExecute = false;
						MyProcess.Start();
						// ReSharper disable once UnusedVariable
						var Stdout = MyProcess.StandardOutput.ReadToEnd();
						var Stderr = MyProcess.StandardError.ReadToEnd();
						MyProcess.WaitForExit();
						if (Stderr.Length > 0)
							{
							return CLog.Trace("Error extracting compressed file " + Archive + " message: " + Stderr,
								LogEventType.Message);
							}

						return String.Empty;
						}
					catch (Exception E)
						{
						return CLog.Trace("Cannot extract comppresse file " + Archive + " reason: " + E.Message,
							LogEventType.Error);
						}
					}
				}

			return String.Empty;
			}

		// Extract all files, take filepath into account
		public static String SevenZipExtractAll(String Archive, String OutputDirectory, String Filter = "*",
			Boolean Recursive = false)
			{
			var Stdout = String.Empty;
			var RecursiveOption = String.Empty;
			if (Recursive)
				{
				RecursiveOption = " -r";
				}

			if (File.Exists(Archive))
				{
				using (var MyProcess = new Process())
					{
					try
						{
						MyProcess.StartInfo.FileName = CTSWOptions.SevenZip;
						MyProcess.StartInfo.Arguments =
							"-y x \"" + Archive + "\" -o\"" + OutputDirectory + "\" " + Filter + RecursiveOption;
						MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						MyProcess.StartInfo.CreateNoWindow = true;
						MyProcess.StartInfo.RedirectStandardOutput = true;
						MyProcess.StartInfo.RedirectStandardError = true;
						MyProcess.StartInfo.UseShellExecute = false;
						MyProcess.Start();
						Stdout = MyProcess.StandardOutput.ReadToEnd();
						var Stderr = MyProcess.StandardError.ReadToEnd();
						MyProcess.WaitForExit();
						if (Stderr.Length > 0)
							{
							return CLog.Trace("Error extracting compressed files " + Archive + " message: " + Stderr,
								LogEventType.Message);
							}

						return Stdout;
						}
					catch (Exception E)
						{
						return CLog.Trace("Cannot extract comppressed filse " + Archive + " reason: " + E.Message,
							LogEventType.Error);
						}
					}
				}

			return Stdout;
			}

		// Add all files from a directory to a zip archive, optionally do this recursively, input directory must end with backslash!
		public static String SevenZipAddAll(String Archive, String InputDirectory, Boolean Recursive = false)
			{
			using (var MyProcess = new Process())
				{
				try
					{
					MyProcess.StartInfo.FileName = CTSWOptions.SevenZip;
					if (Recursive)
						{
						MyProcess.StartInfo.Arguments = "a \"" + Archive + "\" -r \"" + QuoteFilename(InputDirectory);
						}
					else
						{
						MyProcess.StartInfo.Arguments = "a \"" + InputDirectory+"\"";
						}

					MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					MyProcess.StartInfo.CreateNoWindow = true;
					MyProcess.StartInfo.RedirectStandardOutput = true;
					MyProcess.StartInfo.RedirectStandardError = true;
					MyProcess.StartInfo.UseShellExecute = false;
					MyProcess.Start();
					var Stdout = MyProcess.StandardOutput.ReadToEnd();
					var Stderr = MyProcess.StandardError.ReadToEnd();
					MyProcess.WaitForExit();
					if (Stderr.Length > 0)
						{
						return CLog.Trace("Error extracting compressed files " + Archive + " message: " + Stderr,
							LogEventType.Message);
						}

					return Stdout;
					}
				catch (Exception E)
					{
					return CLog.Trace("Cannot extract compressed file " + Archive + " reason: " + E.Message, LogEventType.Error);
					}
				}
			}

		// Add all files from a directory to a .7z archive, use TSW directory as working directory, so it saves with TS as base directory
		public static String SevenZipAddFiles(String Archive, String InputFiles, Boolean Recursive = false)
			{
			using (var MyProcess = new Process())
				{
				try
					{
					MyProcess.StartInfo.FileName = CTSWOptions.SevenZip;
					if (Recursive)
						{
						MyProcess.StartInfo.Arguments = "a \"" + Archive + "\" -r \"" + InputFiles;
						}
					else
						{
						MyProcess.StartInfo.Arguments = "a \"" + InputFiles+ "\"";
						}
					MyProcess.StartInfo.WorkingDirectory=CTSWOptions.TrainSimWorldDirectory;
					MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					MyProcess.StartInfo.CreateNoWindow = true;
					MyProcess.StartInfo.RedirectStandardOutput = true;
					MyProcess.StartInfo.RedirectStandardError = true;
					MyProcess.StartInfo.UseShellExecute = false;
					MyProcess.Start();
					var Stdout = MyProcess.StandardOutput.ReadToEnd();
					var Stderr = MyProcess.StandardError.ReadToEnd();
					MyProcess.WaitForExit();
					if (Stderr.Length > 0)
						{
						return CLog.Trace("Error compressing files " + Archive + " message: " + Stderr,
							LogEventType.Message);
						}

					return Stdout;
					}
				catch (Exception E)
					{
					return CLog.Trace("Cannot compress files " + Archive + " reason: " + E.Message, LogEventType.Error);
					}
				}
			}
		// Extract a single file, take filepath into account
		public static String SevenZipExtractSingle(String Archive, String OutputDirectory, String FullName)
			{
			var Stdout = String.Empty;
			if (File.Exists(Archive))
				{
				using (var MyProcess = new Process())
					{
					try
						{
						MyProcess.StartInfo.FileName = CTSWOptions.SevenZip;
						MyProcess.StartInfo.Arguments = "-y x \"" + Archive + "\" -o\"" + OutputDirectory + "\" \"" + FullName+"\"";
						MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						MyProcess.StartInfo.CreateNoWindow = true;
						MyProcess.StartInfo.RedirectStandardOutput = true;
						MyProcess.StartInfo.RedirectStandardError = true;
						MyProcess.StartInfo.UseShellExecute = false;
						MyProcess.Start();
						Stdout = MyProcess.StandardOutput.ReadToEnd();
						var Stderr = MyProcess.StandardError.ReadToEnd();
						MyProcess.WaitForExit();
						if (Stderr.Length > 0)
							{
							return CLog.Trace("Error extracting compressed files " + Archive + " message: " + Stderr,
								LogEventType.Message);
							}

						return Stdout;
						}
					catch (Exception E)
						{
						return CLog.Trace("Cannot extract compressed file " + Archive + " reason: " + E.Message,
							LogEventType.Error);
						}
					}
				}

			return Stdout;
			}

		// Extract a single file, take filepath into account
		public static String SevenZipExtractAll(String Archive, String OutputDirectory)
			{
			var Stdout = String.Empty;
			if (File.Exists(Archive))
				{
				using (var MyProcess = new Process())
					{
					try
						{
						MyProcess.StartInfo.FileName = CTSWOptions.SevenZip;
						MyProcess.StartInfo.Arguments = "-y -r -aoa x \"" + Archive + "\" -o\"" + OutputDirectory + "\" *";
						MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						MyProcess.StartInfo.CreateNoWindow = true;
						MyProcess.StartInfo.RedirectStandardOutput = true;
						MyProcess.StartInfo.RedirectStandardError = true;
						MyProcess.StartInfo.UseShellExecute = false;
						MyProcess.Start();
						Stdout = MyProcess.StandardOutput.ReadToEnd();
						var Stderr = MyProcess.StandardError.ReadToEnd();
						MyProcess.WaitForExit();
						if (Stderr.Length > 0)
							{
							return CLog.Trace("Error extracting compressed files " + Archive + " message: " + Stderr,
								LogEventType.Message);
							}

						return Stdout;
						}
					catch (Exception E)
						{
						return CLog.Trace("Cannot extract compressed files from " + Archive + " reason: " + E.Message,
							LogEventType.Error);
						}
					}
				}
			return Stdout;
			}

		// Extract all files at a given directory path. Note: you need to specify the wildcardp pattern as well, e.g. * for all files
		public static String SevenZipExtractDirectory(String Archive, String InputDirectory, String OutputDirectory)
			{
			var Stdout = String.Empty;
			if (File.Exists(Archive))
				{
				using (var MyProcess = new Process())
					{
					try
						{
						MyProcess.StartInfo.FileName = CTSWOptions.SevenZip;
						MyProcess.StartInfo.Arguments = "-y -r -aoa x \"" + Archive + "\" -o\"" + OutputDirectory + "\" "+ InputDirectory;
						MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						MyProcess.StartInfo.CreateNoWindow = true;
						MyProcess.StartInfo.RedirectStandardOutput = true;
						MyProcess.StartInfo.RedirectStandardError = true;
						MyProcess.StartInfo.UseShellExecute = false;
						MyProcess.Start();
						Stdout = MyProcess.StandardOutput.ReadToEnd();
						var Stderr = MyProcess.StandardError.ReadToEnd();
						MyProcess.WaitForExit();
						if (Stderr.Length > 0)
							{
							return CLog.Trace("Error extracting compressed files " + Archive + " message: " + Stderr,
								LogEventType.Message);
							}

						return Stdout;
						}
					catch (Exception E)
						{
						return CLog.Trace("Cannot extract compressed files from " + Archive + " reason: " + E.Message,
							LogEventType.Error);
						}
					}
				}
			return Stdout;
			}


		public static String ListZipFiles(String FilePath, out String Stdout)
			{
			Stdout = String.Empty;
			if (File.Exists(FilePath))
				{
				using (var MyProcess = new Process())
					{
					try
						{
						MyProcess.StartInfo.FileName = CTSWOptions.SevenZip;
						MyProcess.StartInfo.Arguments = "-r l \"" + FilePath + "\"";
						MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						MyProcess.StartInfo.CreateNoWindow = true;
						MyProcess.StartInfo.RedirectStandardOutput = true;
						MyProcess.StartInfo.RedirectStandardError = true;
						MyProcess.StartInfo.UseShellExecute = false;
						MyProcess.Start();
						Stdout = MyProcess.StandardOutput.ReadToEnd();
						var Stderr = MyProcess.StandardError.ReadToEnd();
						MyProcess.WaitForExit();
						if (Stderr.Length > 0)
							{
							return CLog.Trace("Error list zip file " + FilePath + " message: " + Stderr, LogEventType.Message);
							}

						return String.Empty;
						}
					catch (Exception E)
						{
						return CLog.Trace("Cannot list zip file " + FilePath + " reason: " + E.Message, LogEventType.Error);
						}
					}
				}

			return String.Empty;
			}


		public static String LaunchUrl(String Filepath, Boolean IsMinimised)
			{
			var OpenFileProcess = new Process();
			try
				{
				OpenFileProcess.StartInfo.FileName = "explorer.exe";
				OpenFileProcess.StartInfo.Arguments = QuoteFilename(Filepath);
				if (IsMinimised)
					{
					OpenFileProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
					}
				else
					{
					OpenFileProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
					}

				OpenFileProcess.StartInfo.RedirectStandardOutput = false;
				OpenFileProcess.Start();
				OpenFileProcess.Dispose();
				return String.Empty;
				}
			catch (Exception E)
				{
				OpenFileProcess.Dispose();
				return CLog.Trace("Cannot open file " + Filepath + " reason: " + E.Message +
				                  "\r\nMake sure to install it at the correct location");
				}
			}

		public static void EmptyDirectory(String Directory)
			{
			// First delete all the files, making sure they are not readonly
			var StackA = new Stack<DirectoryInfo>();
			StackA.Push(new DirectoryInfo(Directory));

			var StackB = new Stack<DirectoryInfo>();
			while (StackA.Any())
				{
				var Dir = StackA.Pop();
				foreach (var File in Dir.GetFiles())
					{
					File.IsReadOnly = false;
					File.Delete();
					}

				foreach (var SubDir in Dir.GetDirectories())
					{
					SetPermissions(SubDir.FullName);
					StackA.Push(SubDir);
					StackB.Push(SubDir);
					}

				System.IO.Directory.Delete(Directory, false);
				}

			// Then delete the sub directories depth first
			while (StackB.Any())
				{
				StackB.Pop().Delete();
				}
			}

		private static void SetPermissions(String DirPath)
			{
			var Info = new DirectoryInfo(DirPath);
			var Self = System.Security.Principal.WindowsIdentity.GetCurrent();
			var Ds = Info.GetAccessControl();
			Ds.AddAccessRule(new FileSystemAccessRule(Self.Name,
				FileSystemRights.FullControl,
				InheritanceFlags.ObjectInherit |
				InheritanceFlags.ContainerInherit,
				PropagationFlags.None,
				AccessControlType.Allow));
			Info.SetAccessControl(Ds);
			}

		/*
		 * Prepare a string to include it in XML
	&lt; < less than
	&gt; > greater than
	&amp; & ampersand
	&apos; ' apostrophe
	&quot; " quotation mark
	*/

		public static String AddEntities(String S)
			{
			// Replace & must be in first position
			return S.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\'", "&apos;")
				.Replace("\"", "&quot;");
			}

		// Utility function to put a string in quotes, maybe needed in XML constructs, e.g. Inpt string output "string"
		public static String AddQuotes(String S)
			{
			// first, we add backslashes before each quote to escape the quote

			return "\"" + S.Replace("\"", "\\\"").Replace("\'", "\\\'") + "\"";
			}

		// Remove all white space from a string
		// http://stackoverflow.com/questions/6219454/efficient-way-to-remove-all-whitespace-from-string/37368176#37368176
		// This is very fast and safe
		public static String Trim(String Str)
			{
			var Len = Str.Length;
			var Src = Str.ToCharArray();
			var DstIdx = 0;
			for (Int32 I = 0; I < Len; I++)
				{
				var Ch = Src[I];
				switch (Ch)
					{
						case '\u0020':
						case '\u00A0':
						case '\u1680':
						case '\u2000':
						case '\u2001':
						case '\u2002':
						case '\u2003':
						case '\u2004':
						case '\u2005':
						case '\u2006':
						case '\u2007':
						case '\u2008':
						case '\u2009':
						case '\u200A':
						case '\u202F':
						case '\u205F':
						case '\u3000':
						case '\u2028':
						case '\u2029':
						case '\u0009':
						case '\u000A':
						case '\u000B':
						case '\u000C':
						case '\u000D':
						case '\u0085':
							continue;
						default:
							Src[DstIdx++] = Ch;
							break;
					}
				}

			return new String(Src, 0, DstIdx);
			}

		public static UInt64 GetUuid()
			{
			var G = Guid.NewGuid();
			var Bytes = G.ToByteArray();
			return BitConverter.ToUInt64(Bytes, 0);
			}

		public static String GetUuidString()
			{
			return GetUuid().ToString().Substring(0, 10);
			}

		// Saves an XDocument while preserving indentation and removing the Byte Order marker (BOM)
		// you need to use this because the DTG files do not support a byte order marker
		// but at the same time you like to preserve indentation and new lines.
		public static void Save(XDocument Doc, String Targetfile)
			{
			var Settings = new XmlWriterSettings
				{
				Encoding = new UTF8Encoding(false), // The false means, do not emit the BOM.
				Indent = true
				};
			using (XmlWriter W = XmlWriter.Create(Targetfile, Settings))
				{
				Doc.Save(W);
				}
			}

		// Convert integer time in seconds to h:mm:ss formatted string
		public static String TimeToString(UInt32 Time)
			{
			UInt32 Hours, Minutes, Seconds;
			Hours = Time / 3600;
			Minutes = (Time - Hours * 3600) / 60;
			Seconds = Time - Hours * 3600 - Minutes * 60;
			return String.Format("{0,1:d2}:{1,1:d2}:{2,1:d2}", Hours, Minutes, Seconds);
			}

		public static String SetFileType(String FileName, String Extension)
			{
			var Tmp = Path.GetExtension(FileName);
			Int32 Length = 0;
			if (Tmp != null)
				{
				Length = Tmp.Length;
				}
			var Tmp2 = FileName.Substring(0, FileName.Length - Length);
			return Tmp2 + "." + Extension;
			}


		// Unpack all files in the directory InputDirectory including path to the output directory.
		// This function is useful if you want to change the last part of the path, as we do when cloning a scenario
		public static String UnpackDirectory(String ArchivePath, String InputDirectory, String OutputDirectory)

			{
			var Result = String.Empty;
	
			if (!File.Exists(ArchivePath))
				{
				Result += CLog.Trace("Archive " + ArchivePath + " not found",LogEventType.Message); // Cannot open archive
				return Result;
				}
			try
				{
				using (ZipArchive Archive = ZipFile.OpenRead(ArchivePath))
					{
					var Entries= Archive.Entries;
					foreach(var Entry in Entries)
						{
						if(Entry.FullName.StartsWith(InputDirectory))
							{
							var StrippedPath= Entry.FullName.Substring(InputDirectory.Length);
							var TargetPath= OutputDirectory+ StrippedPath;
							if(StrippedPath.Length>0)
								{
								var TargetDir=Path.GetDirectoryName(TargetPath);
								if (TargetDir != null)
									{
									Directory.CreateDirectory(TargetDir);
									}
								if(!StrippedPath.EndsWith("/")) // Do not try to create an entry for directories
									{
									Entry.ExtractToFile(TargetPath,true);
									}
								}
							}
						}
					}
				}
			catch (Exception E)
				{
				CLog.Trace("Problem extracting files " + InputDirectory + " because " + E.Message, LogEventType.Error);
				return Result;
				}
			return Result;
			}

		// https://stackoverflow.com/questions/18996330/copying-files-and-subdirectories-to-another-directory-with-existing-files
		public static String CopyDir(String FromFolder, String ToFolder, Boolean Overwrite = false)
			{
			try
				{
				Directory
					.EnumerateFiles(FromFolder, "*.*", SearchOption.AllDirectories)
					.AsParallel()
					.ForAll(From =>
						{
						var To = From.Replace(FromFolder, ToFolder);
						// Create directories if needed
						var ToSubFolder = Path.GetDirectoryName(To);
						if (!String.IsNullOrWhiteSpace(ToSubFolder))
							{
							Directory.CreateDirectory(ToSubFolder);
							}

						File.Copy(From, To, Overwrite);
						});
				return String.Empty;
				}

			catch (Exception E)
				{
				return CLog.Trace("Failed to copy directories because " + E.Message, LogEventType.Error);
				}
			}

    // Doubles all quotes in a string, needed to write the string to an SQL table, if input is null, will return null
    public static String DoubleQuotes(String Input)
      {
      return Input?.Replace("\"","\"\"").Replace("\'","\'\'");
      }

// Add quotes to a filename in case it contains spaces. If the filepath is already quoted, don't do it again 
    public static string QuoteFilename(string s)
      {
      if(s.StartsWith("\"") && s.EndsWith("\""))
        {
        return s; // already quoted
        }
      else
        return $"\"{s}\"";
      }


		}
	}