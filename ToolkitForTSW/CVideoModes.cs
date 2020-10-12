using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// https://stackoverflow.com/questions/744541/how-to-list-available-video-modes-using-c

namespace ToolkitForTSW
	{
	public class VideoMode
		{
		public Int32 Width { get; set; }
		public Int32 Height { get; set; }
		public Int32 Freq { get; set; }
		public Int32 Color { get; set; }

		public override String ToString()
			{
			return Width.ToString() + "x" + Height.ToString() + " " + Freq.ToString() + "Hz";
			}
		}

	public class CVideoModes
		{
		private readonly ObservableCollection<VideoMode> _VideoModesList;
		public ObservableCollection<VideoMode> VideoModesList => _VideoModesList;

		public CVideoModes()
			{
			_VideoModesList = new ObservableCollection<VideoMode>();
			_VideoModesList = ListVideoModes();
			}

		[DllImport("user32.dll")]
		private static extern Boolean EnumDisplaySettings(
												String DeviceName, Int32 ModeNum, ref Devmode DevMode);

		private const Int32 EnumCurrentSettings = -1;

		private const Int32 EnumRegistrySettings = -2;

		[StructLayout(LayoutKind.Sequential)]
		public struct Devmode
			{
			private const Int32 Cchdevicename = 0x20;
			private const Int32 Cchformname = 0x20;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
			public String dmDeviceName;

			public Int16 dmSpecVersion;
			public Int16 dmDriverVersion;
			public Int16 dmSize;
			public Int16 dmDriverExtra;
			public Int32 dmFields;
			public Int32 dmPositionX;
			public Int32 dmPositionY;
			public ScreenOrientation dmDisplayOrientation;
			public Int32 dmDisplayFixedOutput;
			public Int16 dmColor;
			public Int16 dmDuplex;
			public Int16 dmYResolution;
			public Int16 dmTTOption;
			public Int16 dmCollate;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
			public String dmFormName;

			public Int16 dmLogPixels;
			public Int32 dmBitsPerPel;
			public Int32 dmPelsWidth;
			public Int32 dmPelsHeight;
			public Int32 dmDisplayFlags;
			public Int32 dmDisplayFrequency;
			public Int32 dmICMMethod;
			public Int32 dmICMIntent;
			public Int32 dmMediaType;
			public Int32 dmDitherType;
			public Int32 dmReserved1;
			public Int32 dmReserved2;
			public Int32 dmPanningWidth;
			public Int32 dmPanningHeight;
			}



		public static ObservableCollection<VideoMode> ListVideoModes()
			{
			var VDevMode = new Devmode();
			Int32 I = 0;
			var VideoModesList = new ObservableCollection<VideoMode>();

			while (EnumDisplaySettings(null, I, ref VDevMode))
				{
				var VideoModeVar = new VideoMode
					{
					Width = VDevMode.dmPelsWidth,
					Height = VDevMode.dmPelsHeight,
					Color = 1 << VDevMode.dmBitsPerPel,
					Freq = VDevMode.dmDisplayFrequency
					};
				VideoModesList.Add(VideoModeVar);
				I++;
				}
			return VideoModesList;
			}
		}
	}
