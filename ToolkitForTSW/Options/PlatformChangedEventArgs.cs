using System;
using System.Collections.Generic;
using System.Text;

namespace ToolkitForTSW.Options
  {
 public class PlatformChangedEventArgs: EventArgs
    {
    public PlatformEnum OldPlatform { get;set; }
    public PlatformEnum  NewPlatform { get;set;}

    public PlatformChangedEventArgs(PlatformEnum oldPlatform, PlatformEnum newPlatform)
      {
      OldPlatform= oldPlatform;
      NewPlatform= newPlatform;
      }
          
    }
  }
