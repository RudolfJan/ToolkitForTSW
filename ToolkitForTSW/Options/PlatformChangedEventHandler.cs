using System;
using System.Collections.Generic;
using System.Text;

namespace ToolkitForTSW.Options
  {
  public class PlatformChangedEventHandler
    {
    public delegate void PlatformChangedEvent(Object Sender, PlatformChangedEventArgs e);
    public static event PlatformChangedEvent PlatformChanged;
    public static void SetPlatformChangedEvent(PlatformChangedEventArgs e)
      {
      PlatformChanged?.Invoke(null, e);
      }
    }
  }
