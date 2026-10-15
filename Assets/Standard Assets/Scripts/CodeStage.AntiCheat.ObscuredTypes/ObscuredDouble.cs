// Decompile from assembly: Assembly-CSharp-firstpass.dll
using CodeStage.AntiCheat.Common;
using CodeStage.AntiCheat.Detectors;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredDouble : IEquatable<ObscuredDouble>, IFormattable
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct DoubleLongBytesUnion
		{
			[FieldOffset(0)]
			public double d;

			[FieldOffset(0)]
			public long l;

			[FieldOffset(0)]
			public ACTkByte8 b8;
		}

		private static long cryptoKey = 210987L;

		[SerializeField]
		private long currentCryptoKey;

		[SerializeField]
		private ACTkByte8 hiddenValue;

		[FormerlySerializedAs("hiddenValue"), SerializeField]
		private byte[] hiddenValueOld;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private double fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		private ObscuredDouble(double value)
		{
			this.currentCryptoKey = ObscuredDouble.cryptoKey;
			this.hiddenValue = ObscuredDouble.InternalEncrypt(value);
			this.hiddenValueOld = null;
			bool isRunning = ObscuredCheatingDetector.IsRunning;
			this.fakeValue = ((!isRunning) ? 0.0 : value);
			this.fakeValueActive = isRunning;
			this.inited = true;
		}

		public static void SetNewCryptoKey(long newKey)
		{
			ObscuredDouble.cryptoKey = newKey;
		}

		public static long Encrypt(double value)
		{
			return ObscuredDouble.Encrypt(value, ObscuredDouble.cryptoKey);
		}

		public static long Encrypt(double value, long key)
		{
			ObscuredDouble.DoubleLongBytesUnion doubleLongBytesUnion = default(ObscuredDouble.DoubleLongBytesUnion);
			doubleLongBytesUnion.d = value;
			doubleLongBytesUnion.l ^= key;
			return doubleLongBytesUnion.l;
		}

		private static ACTkByte8 InternalEncrypt(double value)
		{
			return ObscuredDouble.InternalEncrypt(value, 0L);
		}

		private static ACTkByte8 InternalEncrypt(double value, long key)
		{
			long num = key;
			if (num == 0L)
			{
				num = ObscuredDouble.cryptoKey;
			}
			ObscuredDouble.DoubleLongBytesUnion doubleLongBytesUnion = default(ObscuredDouble.DoubleLongBytesUnion);
			doubleLongBytesUnion.d = value;
			doubleLongBytesUnion.l ^= num;
			return doubleLongBytesUnion.b8;
		}

		public static double Decrypt(long value)
		{
			return ObscuredDouble.Decrypt(value, ObscuredDouble.cryptoKey);
		}

		public static double Decrypt(long value, long key)
		{
			ObscuredDouble.DoubleLongBytesUnion doubleLongBytesUnion = default(ObscuredDouble.DoubleLongBytesUnion);
			doubleLongBytesUnion.l = (value ^ key);
			return doubleLongBytesUnion.d;
		}

		public void ApplyNewCryptoKey()
		{
			if (this.currentCryptoKey != ObscuredDouble.cryptoKey)
			{
				this.hiddenValue = ObscuredDouble.InternalEncrypt(this.InternalDecrypt(), ObscuredDouble.cryptoKey);
				this.currentCryptoKey = ObscuredDouble.cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			double value = this.InternalDecrypt();
			do
			{
				this.currentCryptoKey = (long)UnityEngine.Random.Range(-2147483648, 2147483647);
			}
			while (this.currentCryptoKey == 0L);
			this.hiddenValue = ObscuredDouble.InternalEncrypt(value, this.currentCryptoKey);
		}

		public long GetEncrypted()
		{
			this.ApplyNewCryptoKey();
			ObscuredDouble.DoubleLongBytesUnion doubleLongBytesUnion = default(ObscuredDouble.DoubleLongBytesUnion);
			doubleLongBytesUnion.b8 = this.hiddenValue;
			return doubleLongBytesUnion.l;
		}

		public void SetEncrypted(long encrypted)
		{
			this.inited = true;
			ObscuredDouble.DoubleLongBytesUnion doubleLongBytesUnion = default(ObscuredDouble.DoubleLongBytesUnion);
			doubleLongBytesUnion.l = encrypted;
			this.hiddenValue = doubleLongBytesUnion.b8;
			if (this.currentCryptoKey == 0L)
			{
				this.currentCryptoKey = ObscuredDouble.cryptoKey;
			}
			if (ObscuredCheatingDetector.IsRunning)
			{
				this.fakeValue = this.InternalDecrypt();
				this.fakeValueActive = true;
			}
			else
			{
				this.fakeValueActive = false;
			}
			if (this.currentCryptoKey == 0L)
			{
				this.currentCryptoKey = ObscuredDouble.cryptoKey;
			}
		}

		public double GetDecrypted()
		{
			return this.InternalDecrypt();
		}

		private double InternalDecrypt()
		{
			if (!this.inited)
			{
				this.currentCryptoKey = ObscuredDouble.cryptoKey;
				this.hiddenValue = ObscuredDouble.InternalEncrypt(0.0);
				this.fakeValue = 0.0;
				this.fakeValueActive = false;
				this.inited = true;
				return 0.0;
			}
			ObscuredDouble.DoubleLongBytesUnion doubleLongBytesUnion = default(ObscuredDouble.DoubleLongBytesUnion);
			doubleLongBytesUnion.b8 = this.hiddenValue;
			doubleLongBytesUnion.l ^= this.currentCryptoKey;
			double d = doubleLongBytesUnion.d;
			if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && Math.Abs(d - this.fakeValue) > 1E-06)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return d;
		}

		public static implicit operator ObscuredDouble(double value)
		{
			return new ObscuredDouble(value);
		}

		public static implicit operator double(ObscuredDouble value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredDouble operator ++(ObscuredDouble input)
		{
			double value = input.InternalDecrypt() + 1.0;
			input.hiddenValue = ObscuredDouble.InternalEncrypt(value, input.currentCryptoKey);
			if (ObscuredCheatingDetector.IsRunning)
			{
				input.fakeValue = value;
				input.fakeValueActive = true;
			}
			else
			{
				input.fakeValueActive = false;
			}
			return input;
		}

		public static ObscuredDouble operator --(ObscuredDouble input)
		{
			double value = input.InternalDecrypt() - 1.0;
			input.hiddenValue = ObscuredDouble.InternalEncrypt(value, input.currentCryptoKey);
			if (ObscuredCheatingDetector.IsRunning)
			{
				input.fakeValue = value;
				input.fakeValueActive = true;
			}
			else
			{
				input.fakeValueActive = false;
			}
			return input;
		}

		public override string ToString()
		{
			return this.InternalDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return this.InternalDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return this.InternalDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return this.InternalDecrypt().ToString(format, provider);
		}

		public override bool Equals(object obj)
		{
			return obj is ObscuredDouble && this.Equals((ObscuredDouble)obj);
		}

		public bool Equals(ObscuredDouble obj)
		{
			return obj.InternalDecrypt().Equals(this.InternalDecrypt());
		}

		public override int GetHashCode()
		{
			return this.InternalDecrypt().GetHashCode();
		}
	}
}
