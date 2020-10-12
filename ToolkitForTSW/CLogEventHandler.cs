using System;

namespace ToolkitForTSW
	{
	public enum LogEventType { Debug, Error, Message, Event, Undefined };

	public class CLogEventArgs : EventArgs
		{
		public String Text;
		public LogEventType EventType;
		public String Method;
		public String FilePath;
		public Int32 LineNumber;
		public Boolean PlaySound;

		public CLogEventArgs(String MyMethod, String MyFilePath, Int32 MyLineNumber, String MyText, LogEventType MyEventType = LogEventType.Message, Boolean MyPlaySound = false)
			{
			Method = MyMethod;
			FilePath = MyFilePath;
			LineNumber = MyLineNumber;
			Text = MyText;
			EventType = MyEventType;
			PlaySound = MyPlaySound;
			}

		public CLogEventArgs(String MyText, LogEventType MyEventType = LogEventType.Message, Boolean MyPlaySound = false)
			{
			Method = "";
			FilePath = "";
			LineNumber = 0;
			Text = MyText;
			EventType = MyEventType;
			PlaySound = MyPlaySound;
			}
		}

	public class CLogEventHandler
		{
		public delegate void CLogEvent(Object Sender, CLogEventArgs E);
		public event CLogEvent LogEvent;
		public void OnLogEvent(CLogEventArgs E)
			{
			LogEvent?.Invoke(this, E);
			}
		}
	}
