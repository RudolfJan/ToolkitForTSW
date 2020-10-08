using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWTools
	{
	public class CKey
		{
		public String Value { get; set; } = String.Empty;
		public Boolean Shift { get; set; } = false;
		public Boolean Ctrl { get; set; } = false;
		public Boolean Alt { get; set; } = false;
		public Boolean Cmd { get; set; } = false;
		public String GamePadControl { get; set; } = String.Empty;

		public CKey()
			{

			}

		public String ParseKey(String KeyArea)
			{
			String Result = String.Empty;
			KeyArea = KeyArea.Replace(" ", "");
			Char[] DelimiterChars = { ' ', ',', ')' };

			String[] Words = KeyArea.Split(DelimiterChars);
			Boolean B = false;
			foreach (var Word in Words)
				{
				if (Word.StartsWith("Key"))
					{
					Value = Word.Substring(4);
					if (Value.StartsWith("Gamepad_"))
						{
						GamePadControl = Value;
						Value = String.Empty;
						}
					continue;
					}
				if (Word.StartsWith("bCtrl"))
					{
					Boolean.TryParse(Word.Substring(6), out B);
					Ctrl = B;
					continue;
					}
				if (Word.StartsWith("bShift"))
					{
					Boolean.TryParse(Word.Substring(7), out B);
					Shift = B;
					continue;
					}
				if (Word.StartsWith("bAlt"))
					{
					Boolean.TryParse(Word.Substring(5), out B);
					Alt = B;
					continue;
					}
				if (Word.StartsWith("bCmd"))
					{
					Boolean.TryParse(Word.Substring(5), out B);
					Cmd = B;
					continue;
					}
				}

			// Key=W, bShift=False,bCtrl=False,bAlt=False,bCmd=False

			return Result;
			}

		public String ParseGamePad(String KeyArea)
			{
			String Result = String.Empty;
			Int32 Idx = KeyArea.IndexOf(")");
			if (Idx >= 0 && KeyArea.Length>=4)
				{
				GamePadControl = KeyArea.Substring(4, Idx - 4);
				}
			return Result;
			}


		}
	}
