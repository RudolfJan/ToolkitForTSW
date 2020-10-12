using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;



namespace ToolkitForTSW
{
	public class CLogEntry : Notifier

		{
		private String _LogEntry = String.Empty;
		public String LogEntry
			{
			get => _LogEntry;
			set
				{
				_LogEntry = value;
				OnPropertyChanged("LogEntry");
				}
			}

		private LogEventType _EventType;
		public LogEventType EventType
			{
			get => _EventType;
			set
				{
				_EventType = value;
				OnPropertyChanged("EventType");
				}
			}

		private String _Method = String.Empty;
		public String Method
			{
			get => _Method;
			set
				{
				_Method = value;
				OnPropertyChanged("Method");
				}
			}

		private String _FilePath = String.Empty;
		public String FilePath
			{
			get => _FilePath;
			set
				{
				_FilePath = value;
				OnPropertyChanged("FilePath");
				}
			}

		private Int32 _LineNumber = 0;
		public Int32 LineNumber
			{
			get => _LineNumber;
			set
				{
				_LineNumber = value;
				OnPropertyChanged("LineNumber");
				}
			}



		public CLogEntry(LogEventType T, String Text)
			{
			LogEntry = Text;
			EventType = T;
			}

		public CLogEntry(String MyMethod, String MyFilePath, Int32 MyLineNumber, LogEventType MyEventType, String MyText)
			{
			LogEntry = MyText;
			EventType = MyEventType;
			Method = MyMethod;
			FilePath = MyFilePath;
			LineNumber = MyLineNumber;
			}



		public override String ToString()
			{
			return EventType.ToString() + " " + LogEntry;
			}
		}


	public class CLog : Notifier
		{
		private ObservableCollection<CLogEntry> _LogManager = null;
		public ObservableCollection<CLogEntry> LogManager
			{
			get => _LogManager;
			set
				{
				_LogManager = value;
				OnPropertyChanged("LogManager");
				}
			}

		public CLog()
			{
			MainWindow.LogEventHandler.LogEvent += OnLogEvent;
			LogManager = new ObservableCollection<CLogEntry>();
			}

		private void OnLogEvent(Object Sender, CLogEventArgs E)
			{
			LogManager.Add(new CLogEntry(E.Method, E.FilePath, E.LineNumber, E.EventType, E.Text));
			}

		// see https://stackoverflow.com/questions/12556767/how-do-i-get-the-current-line-number
		public static String Trace(String Text, LogEventType EventType = LogEventType.Message, Boolean PlaySound = false,
																		[CallerMemberName] String CallingMethod = "",
																		[CallerFilePath] String CallingFilePath = "",
																		[CallerLineNumber] Int32 CallingFileLineNumber = 0)
			{
			var EventText = String.Format("{0}({1}:{2})\n{3}\r\n", CallingMethod, Path.GetFileName(CallingFilePath), CallingFileLineNumber, Text);
			CLogEventArgs Logevent = new CLogEventArgs(CallingMethod, Path.GetFileName(CallingFilePath), CallingFileLineNumber, Text, EventType, PlaySound);
			MainWindow.LogEventHandler.OnLogEvent(Logevent);
			return EventText;
			}
		}
	}
