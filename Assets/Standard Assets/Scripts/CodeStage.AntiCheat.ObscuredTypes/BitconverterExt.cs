// Decompile from assembly: Assembly-CSharp-firstpass.dll
using System;
using System.Collections.Generic;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	internal class BitconverterExt
	{
		public static byte[] GetBytes(decimal dec)
		{
			int[] bits = decimal.GetBits(dec);
			List<byte> list = new List<byte>();
			int[] array = bits;
			for (int i = 0; i < array.Length; i++)
			{
				int value = array[i];
				list.AddRange(BitConverter.GetBytes(value));
			}
			return list.ToArray();
		}

		public static decimal ToDecimal(byte[] bytes)
		{
			if (bytes.Length != 16)
			{
				throw new Exception("[ACTk] A decimal must be created from exactly 16 bytes");
			}
			int[] array = new int[4];
			for (int i = 0; i <= 15; i += 4)
			{
				array[i / 4] = BitConverter.ToInt32(bytes, i);
			}
			return new decimal(array);
		}
	}
}
