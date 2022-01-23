using System;

namespace ToolkitForTSW
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
      var Result = String.Empty;
      KeyArea = KeyArea.Replace(" ", "");
      Char[] DelimiterChars = { ' ', ',', ')' };

      var Words = KeyArea.Split(DelimiterChars);
      Boolean B;
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
          var unused = Boolean.TryParse(Word.AsSpan(6), out B);
          Ctrl = B;
          continue;
          }
        if (Word.StartsWith("bShift"))
          {
          var unused1 = Boolean.TryParse(Word.AsSpan(7), out B);
          Shift = B;
          continue;
          }
        if (Word.StartsWith("bAlt"))
          {
          var unused2 = Boolean.TryParse(Word.AsSpan(5), out B);
          Alt = B;
          continue;
          }
        if (Word.StartsWith("bCmd"))
          {
          var unused3 = Boolean.TryParse(Word.AsSpan(5), out B);
          Cmd = B;
          continue;
          }
        }

      // Key=W, bShift=False,bCtrl=False,bAlt=False,bCmd=False

      return Result;
      }

    public String ParseGamePad(String KeyArea)
      {
      var Result = String.Empty;
      var Idx = KeyArea.IndexOf(")");
      if (Idx >= 0 && KeyArea.Length >= 4)
        {
        GamePadControl = KeyArea.Substring(4, Idx - 4);
        }
      return Result;
      }
    }
  }
