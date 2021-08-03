using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitForTSW
	{
	public class InputMapperModel
		{
		public String InputType { get; set; } = String.Empty;
		public String Identifier { get; set; } = String.Empty;
		public String Action { get; set; } = String.Empty;
		public CKey Key { get; set; } = null;
		public Boolean Found { get; set; } = false;

		public InputMapperModel()
			{
			}

		public String ParseInputLine(String InputLine, String MyInputType, String MyIdentifier)
			{
			String Result = String.Empty;
			// First step, determine mapping type
			// IncreaseInputs=((Key=A), (Key=Gamepad_RightTrigger)),
			// DecreaseInputs=((Key=D), (Key=Gamepad_RightShoulder)))
			Identifier = MyIdentifier;
			InputType = MyInputType;
			Int32 EndAction = InputLine.IndexOf("=", StringComparison.Ordinal);
			Action = InputLine.Substring(0, EndAction);
			Int32 KeyEnd = InputLine.IndexOf(")", StringComparison.Ordinal);
			if (InputLine.Length > KeyEnd)
				{
				if ((KeyEnd - EndAction - 3) > 0)
					{
					String KeyArea = InputLine.Substring(EndAction + 3, KeyEnd - EndAction - 3);
					Key = new CKey();
					Key.ParseKey(KeyArea);
					if (InputLine.Length > KeyEnd + 4)
						{
						String GamePadArea = InputLine.Substring(KeyEnd + 4);
						Key.ParseGamePad(GamePadArea);
						}
					else
						{
						Console.WriteLine("Length issue:" + (EndAction + KeyEnd).ToString() + "InputLine.length+" + InputLine);
						}
					}
				}
			return Result;
			}

		public String ParseActionMapping(String InputLine, String MyInputType, String MyIdentifier)
			{
			String Result = String.Empty;
			Identifier = MyIdentifier;
			InputType = MyInputType;
			Key = new CKey();
			Key.ParseKey(InputLine);
			return Result;
			}

		}

	}
