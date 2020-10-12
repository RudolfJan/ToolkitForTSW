using System;
using System.Collections.Generic;
using System.Text;

namespace SavCrackerTest.Models
	{
	public enum SavDataType
		{
		Undefined,
		Text,
		Guid,
		Code,
		Flag
		};

	public class SavElementModel
		{
		public int Position { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; } //excluding length! 
		public SavDataType DataType { get; set; }
		}
	}
