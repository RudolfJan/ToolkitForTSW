using Styles.Library.Helpers;
using System;



namespace TSWTools
	{
	public class CLogFilter : Notifier
		{
		private Boolean _DebugChecked = true;
		public Boolean DebugChecked
			{
			get => _DebugChecked;
			set
				{
				_DebugChecked = value;
				OnPropertyChanged("DebugChecked");
				}
			}

		private Boolean _ErrorChecked = true;
		public Boolean ErrorChecked
			{
			get => _ErrorChecked;
			set
				{
				_ErrorChecked = value;
				OnPropertyChanged("ErrorChecked");
				}
			}

		private Boolean _MessageChecked = true;
		public Boolean MessageChecked
			{
			get => _MessageChecked;
			set
				{
				_MessageChecked = value;
				OnPropertyChanged("MessageChecked");
				}
			}

		private Boolean _EventChecked = true;
		public Boolean EventChecked
			{
			get => _EventChecked;
			set
				{
				_EventChecked = value;
				OnPropertyChanged("EventChecked");
				}
			}

		public CLogFilter(Boolean MyDebugChecked, Boolean MyErrorChecked, Boolean MyMessageChecked, Boolean MyEventChecked)
			{
			DebugChecked = MyDebugChecked;
			ErrorChecked = MyErrorChecked;
			MessageChecked = MyMessageChecked;
			EventChecked = MyEventChecked;
			}

		public void UpdateFilterSettings(Boolean MyDebugChecked, Boolean MyErrorChecked, Boolean MyMessageChecked, Boolean MyEventChecked)
			{
			DebugChecked = MyDebugChecked;
			ErrorChecked = MyErrorChecked;
			MessageChecked = MyMessageChecked;
			EventChecked = MyEventChecked;
			}


		public Boolean EventTypeFilter(Object Item)
			{
			var MyItem = (CLogEntry)Item;
			if (MyItem != null)
				{
				switch (MyItem.EventType)
					{
					case LogEventType.Error:
							{
							return ErrorChecked;
							}
					case LogEventType.Debug:
							{
							return DebugChecked;
							}
					case LogEventType.Message:
							{
							return MessageChecked;
							}
					case LogEventType.Event:
							{
							return EventChecked;
							}
					default:
							{
							return false;
							}
					}
				}
			return false;
			}
		}
	}
