﻿using Caliburn.Micro;
using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace ToolkitForTSW.ViewModels
{
	public class KeyBindingViewModel : Screen
		{
		private BindableCollection<InputMapperModel> _InputMapperList;
		public BindableCollection<InputMapperModel> InputMapperList
			{
			get { return _InputMapperList; }
			set
				{
				_InputMapperList = value;
				NotifyOfPropertyChange(()=>InputMapperList);
				}
			}

		private String _Result;
		public String Result
			{
			get { return _Result; }
			set
				{
				_Result = value;
				NotifyOfPropertyChange(()=>Result);
				}
			}


		public KeyBindingViewModel()
			{
			InputMapperList=new BindableCollection<InputMapperModel>();
			var InputMapperFile = TSWOptions.UnpackFolder +
			                      "TS2Prototype-WindowsNoEditor.pak\\TS2Prototype\\Config\\DefaultInput.ini";
			if (!File.Exists(InputMapperFile))
				{
				Result += Log.Trace("Cannot open Input mapper file. Did you unpack the core game? " +
				                     InputMapperFile,LogEventType.Error);
				return;
				}

			GetInputs(InputMapperFile);
			}

		public String GetInputs(String InputMapperFile)
			{
			String CurrentLine = String.Empty;

			if (!File.Exists(InputMapperFile))
				{
				Result += "Cannot open InputMapper file " + InputMapperFile + "\r\n";
				return Result;
				}
			try
				{
				StreamReader Reader = new StreamReader(InputMapperFile);

				while (!Reader.EndOfStream)
					{
					CurrentLine = Reader.ReadLine();
					ParseInputLine(CurrentLine);
					}
				Reader.Close();
				}
			catch (Exception E)
				{
				Result += "Fatal error reading InputMapper " + E.Message + "line " + CurrentLine + "\r\n";
				return Result;
				}

			return Result;
			}

		public String ParseInputLine(String InputLine)
			{
			// First step, determine mapping type
			// +StandardInputs=(Identifier="Power",IncreaseInputs=((Key=A), (Key=Gamepad_RightTrigger)),DecreaseInputs=((Key=D), (Key=Gamepad_RightShoulder)))
			if (InputLine.StartsWith("+StandardInputs="))
				{
				String InputType = "StandardInputs";
				Int32 IdentifierEnd = InputLine.IndexOf(",", StringComparison.Ordinal);
				String Identifier = InputLine.Substring(29, IdentifierEnd - 30);
				Int32 IncInputsEnd = InputLine.IndexOf("DecreaseInputs", StringComparison.Ordinal);
				if (IncInputsEnd > 0)
					{
					String IncreaseInputs = InputLine.Substring(IdentifierEnd + 1, IncInputsEnd - IdentifierEnd - 2);
					String DecreaseInputs = InputLine.Substring(IncInputsEnd);
					InputMapperModel Inc = new InputMapperModel();
					Inc.ParseInputLine(IncreaseInputs, InputType, Identifier);
					InputMapperModel Dec = new InputMapperModel();
					Dec.ParseInputLine(DecreaseInputs, InputType, Identifier);
					InputMapperList.Add(Inc);
					InputMapperList.Add(Dec);
					}
				else
					{
					String IncreaseInputs = InputLine.Substring(IdentifierEnd + 1);
					InputMapperModel Inc = new InputMapperModel();
					Inc.ParseInputLine(IncreaseInputs, InputType, Identifier);
					InputMapperList.Add(Inc);
					}
				}
			//+ActionMappings = (ActionName = "ActivateScenarioTrigger", Key = E, bShift = False, bCtrl = False, bAlt = False, bCmd = False)
			//+ ActionMappings = (ActionName = "ActivateScenarioTrigger", Key = Gamepad_FaceButton_Bottom, bShift = False, bCtrl = False, bAlt = False, bCmd = False)
			//+ ActionMappings = (ActionName = "Camera_ExteriorBoomMoveBackward", Key = Right, bShift = False, bCtrl = True, bAlt = False, bCmd = False)
			if (InputLine.StartsWith("+ActionMappings="))
				{
				String InputType = "ActionMappings";
				Int32 IdentifierEnd = InputLine.IndexOf(",", StringComparison.Ordinal);
				String Identifier = InputLine.Substring(29, IdentifierEnd - 30);
				String ActionPart = InputLine.Substring(IdentifierEnd+1);
				InputMapperModel Inc = new InputMapperModel();
				Inc.ParseActionMapping(ActionPart, InputType, Identifier);
				InputMapperList.Add(Inc);
				}
			return Result;
			}

		public Task CloseForm()
      {
			return TryCloseAsync();
      }
		}
	}
