using GvasFormat.Serialization.UETypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Gvas.Library.Serialization.UETypes
  {
  [DebuggerDisplay("{Value}", Name = "{Name}")]
  public class UEQuatStructProperty : UEStructProperty
    {
    public UEQuatStructProperty() { }

    public UEQuatStructProperty(BinaryReader reader)
      {
      Value = reader.ReadBytes(12);
      
      }

    public byte[] Value= new byte[12];
    }
  }
