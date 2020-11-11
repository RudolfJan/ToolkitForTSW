using Styles.Library.Helpers;
using System;
using System.IO;
using System.Text;

namespace ToolkitForTSW
  {
  public class CCloneScenario : Notifier
    {
    private CScenario _scenario;
    public CScenario Scenario
      {
      get
        {
        return _scenario;
        }
      set
        {
        _scenario = value;
        OnPropertyChanged("Scenario");
        }
      }

    private string _ClonedScenarioName;
    public string ClonedScenarioName
      {
      get { return _ClonedScenarioName; }
      set
        {
        _ClonedScenarioName = value;
        OnPropertyChanged("ClonedScenarioName");
        }
      }

    private Guid _ClonedScenarioGuid = Guid.NewGuid();
    public Guid ClonedScenarioGuid
      {
      get { return _ClonedScenarioGuid; }
      set
        {
        _ClonedScenarioGuid = value;
        OnPropertyChanged("ClonedScenarioGuid");
        }
      }

    public void Clone()
      {
      string fileName = GetClonedScenarioFileName();
      byte[] guidBytes = ClonedScenarioGuid.ToByteArray();
      byte[] clonedScenarioNameBytes = GetScenarioNameByteString(ClonedScenarioName);
      var sourceDataLength = Scenario.Cracker.Data.GetLength(0);
      // correct output length for the length of the scenario
      var sourceScenarioNameLength = Scenario.Cracker.SavDataList[1].Position -
                               Scenario.Cracker.SavDataList[0].Position - 37;
      var destinationScenarioNameLength = clonedScenarioNameBytes.GetLength(0);
      var destinationDataLength = sourceDataLength - sourceScenarioNameLength + destinationScenarioNameLength;
      var output = new byte[destinationDataLength];

      var destinationScenarioNamePosition = Scenario.Cracker.SavDataList[0].Position + 37; // tricky there is an additional null byte after the index
      int destinationOffset = destinationScenarioNameLength - sourceScenarioNameLength;
      int startIndexInData = Scenario.Cracker.SavDataList[1].Position;
      int destinationRemainderStartPositionScenario = Scenario.Cracker.SavDataList[1].Position + destinationOffset;
      int bytesToCopy = destinationDataLength - destinationRemainderStartPositionScenario;

      // copy part till ScenarioName
      Buffer.BlockCopy(Scenario.Cracker.Data, 0, output, 0, destinationScenarioNamePosition);
      // insert new scenario name
      Buffer.BlockCopy(clonedScenarioNameBytes, 0, output, destinationScenarioNamePosition,
        destinationScenarioNameLength);
      // copy rest of data
      Buffer.BlockCopy(Scenario.Cracker.Data,
        startIndexInData, output, startIndexInData + destinationOffset, bytesToCopy);

      // Update ScenarioGuid
      int destinationGuidLocation = Scenario.Cracker.SavDataList[1].Position + 74 + destinationOffset; // 74 is the offset till the actual Guid value
      Buffer.BlockCopy(guidBytes,
        0, output, destinationGuidLocation, 16);
      WriteClonedScenarioToFile(fileName, output);
      }

    private static void WriteClonedScenarioToFile(string fileName, byte[] clonedScenarioBytes)
      {
      var destinationDataLength = clonedScenarioBytes.GetLength(0);
      try
        {
        using (var s = new FileStream(fileName, FileMode.Create))
          {
          using (var writer = new BinaryWriter(s))
            {
            writer.Write(clonedScenarioBytes, 0, destinationDataLength);
            }
          }
        }
      catch (Exception ex)
        {
        Console.WriteLine("Exception during reading data "); //TODO: use logging
        return;
        }
      }

    // public static void BlockCopy (Array src, int srcOffset, Array dst, int dstOffset, int count);
    private byte[] GetScenarioNameByteString(string clonedScenarioName)
      {
      int length = ClonedScenarioName.Length + 1; // Must include terminating zero and filler byte
      int payloadLength = length + 4;
      int byteSequenceLength = payloadLength + 8+1; // add space for payloadLength and index and filler byte
      var output = new byte[byteSequenceLength];
      var payLoadLengthPart = ConvertIntToBytes(payloadLength);
      Buffer.BlockCopy(payLoadLengthPart, 0, output, 0, 4);
      var stringLengthBytes = ConvertIntToBytes(length);
      Buffer.BlockCopy(stringLengthBytes, 0, output, 9, 4);
      var stringAsBytes = Encoding.Default.GetBytes(ClonedScenarioName);
      Buffer.BlockCopy(stringAsBytes, 0, output, 13, length - 1);
      return output;
      }


    private byte[] ConvertIntToBytes(int inp)
      {
      byte[] intBytes = BitConverter.GetBytes(inp);
      if (!BitConverter.IsLittleEndian)
        Array.Reverse(intBytes);
      byte[] result = intBytes;
      return result;
      }


    private string GetClonedScenarioFileName()
      {
      return $"{CTSWOptions.GameSaveLocation}Saved\\SaveGames\\USD_{ClonedScenarioGuid.ToString().ToUpper()}.sav";
      }
    }
  }
