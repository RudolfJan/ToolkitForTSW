using System;
using System.Diagnostics; // needed for Process class
using System.IO;
using Logging.Library;
using Utilities.Library;
using Utilities.Library.TextHelpers;

// static class to invoke external applications
namespace ToolkitForTSW
  {

  public static class CApps
    {

    public static String EditXmlFile(String XmlFile)
      {
      return EditGenericTextFile(XmlFile,
          false,
          TSWOptions.XmlEditor,
          Log.Trace(
              "XMLFile edited\r\nWARNING: the contents of this screen may be invalid until you finished editing the XML File! You may need to refresh the contents manually using the refresh button",
              LogEventType.Message)
      );
      }

    public static String EditTextFile(String Filepath)
      {
      return EditGenericTextFile(Filepath, true, TSWOptions.TextEditor);
      }

    private static String EditGenericTextFile(String Filepath, bool create_if_not_exists, String Editor, String ReturnMsg = "")
      {
      if (!File.Exists(Filepath))
        {
        if (!create_if_not_exists)
          {
          Log.Trace("Error editing File " + Filepath + " because file is not found", LogEventType.Error);
          return "Error editing File " + Filepath + " because file is not found";
          }
        else
          {
          FileHelpers.CreateEmptyFile(Filepath);
          }
        }
      using (Process TextfileEditProcess = new Process())
        {
        try
          {
          string FileName;
          if (Editor.Length > 0)
            {
            FileName = TSWOptions.XmlEditor;
            }
          else
            {
            FileName = "Notepad.exe";
            }
          ProcessHelper.RunProcess(FileName, Arguments: TextHelper.QuoteFilename(Filepath), WindowStyle: ProcessWindowStyle.Maximized);
          }
        catch (Exception E)
          {
            {
            return Log.Trace("Error editing file " + Filepath + ". Reason: " + E.Message, LogEventType.Error);
            }
          }
        return ReturnMsg;
        }
      }


    public static String ExecuteFile(FileInfo Filepath, String Arguments = "")
      {
      return ExecuteFile(Filepath.FullName, Arguments);
      }

    public static String ExecuteFile(String Filepath, String Arguments = "")
      {
      return ProcessHelper.RunProcess(Filepath, Arguments, CreateNoWindow: false);
      }

    // Start FileCompare Tool
    public static String FileCompareTool(String InputFile, String OutputFile, String InputDescription,
        String OutputDescription)
      {
      var Result = String.Empty;
      if (TSWOptions.FileCompare.Length == 0)
        {
        return Log.Trace("File Compare Tool not set in Options", LogEventType.Error);
        }
      try
        {
        string process_arguments = TextHelper.QuoteFilename(InputFile) + " " + TextHelper.QuoteFilename(OutputFile) + " /dl \"" + InputDescription +
            "\" /dr \"" + OutputDescription + "\"";
        ProcessHelper.RunProcess(TSWOptions.FileCompare, process_arguments, WindowStyle: ProcessWindowStyle.Maximized);
        }
      catch (Exception E)
        {
        return Log.Trace("Error using File Compare Tool " + E.Message + "\r\n" + Result, LogEventType.Error);
        }
      return Result;
      }

     public static String UnPack(String InputFile, String OutputDirectory)
      {
      String Result = String.Empty;
      try
        {
        string FileName = TSWOptions.Unpacker;
        string Arguments = "\"" + InputFile + "\" -extract " + TextHelper.QuoteFilename(OutputDirectory);
        Result += ProcessHelper.RunProcess(FileName, Arguments, WaitForExit: true, ContinuousOutput: true, WindowStyle: ProcessWindowStyle.Hidden, CreateNoWindow: true, RedirectStandardOutput: true);
        return "";
        }
      catch (Exception E)
        {
        return "Error using Unpack " + InputFile + " " + E.Message + "\r\n";
        }
      }

    public static String UnPackAsset(String CommandLine)
      {
      var Result = String.Empty;
      if (TSWOptions.UAssetUnpacker.Length == 0)
        {
        Result += Log.Trace("UAsset Unpacker location is not set in options", LogEventType.Error);
        return Result;
        }
      try
        {
        string FileName = TSWOptions.UAssetUnpacker;
        return ProcessHelper.RunProcess(FileName, CommandLine, WaitForExit: true, WindowStyle: ProcessWindowStyle.Hidden, CreateNoWindow: true, RedirectStandardOutput: true);
        }
      catch (Exception E)
        {
        Result += Log.Trace("Error using UAssetUnpacker " + CommandLine + " " + E.Message,
            LogEventType.Error);
        return Result;
        }
      }

     public static String LaunchUrl(String Filepath, Boolean IsMinimised)
      {
      var OpenFileProcess = new Process();
      try
        {
        OpenFileProcess.StartInfo.FileName = "explorer.exe";
        OpenFileProcess.StartInfo.Arguments = TextHelper.QuoteFilename(Filepath);
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
        return Log.Trace("Cannot open file " + Filepath + " reason: " + E.Message +
                          "\r\nMake sure to install it at the correct location");
        }
      }

     }
  }
