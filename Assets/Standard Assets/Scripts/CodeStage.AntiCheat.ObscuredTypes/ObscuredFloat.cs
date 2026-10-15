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
	public struct ObscuredFloat : IEquatable<ObscuredFloat>, IFormattable
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct FloatIntBytesUnion
		{
			[FieldOffset(0)]
			public float f;

			[FieldOffset(0)]
			public int i;

			[FieldOffset(0)]
			public ACTkByte4 b4;
		}

		private static int cryptoKey = 230887;

		[SerializeField]
		private int currentCryptoKey;

		[SerializeField]
		private ACTkByte4 hiddenValue;

		[FormerlySerializedAs("hiddenValue"), SerializeField]
		private byte[] hiddenValueOld;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private float fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		private ObscuredFloat(float value)
		{
			this.currentCryptoKey = ObscuredFloat.cryptoKey;
			this.hiddenValue = ObscuredFloat.InternalEncrypt(value);
			this.hiddenValueOld = null;
			bool isRunning = ObscuredCheatingDetector.IsRunning;
			this.fakeValue = ((!isRunning) ? 0f : value);
			this.fakeValueActive = isRunning;
			this.inited = true;
		}

		public static void SetNewCryptoKey(int newKey)
		{
			ObscuredFloat.cryptoKey = newKey;
		}

		public static int Encrypt(float value)
		{
			return ObscuredFloat.Encrypt(value, ObscuredFloat.cryptoKey);
		}

		public static int Encrypt(float value, int key)
		{
			ObscuredFloat.FloatIntBytesUnion floatIntBytesUnion = default(ObscuredFloat.FloatIntBytesUnion);
			floatIntBytesUnion.f = value;
			floatIntBytesUnion.i ^= key;
			return floatIntBytesUnion.i;
		}

		private static ACTkByte4 InternalEncrypt(float value)
		{
			return ObscuredFloat.InternalEncrypt(value, 0);
		}

		private static ACTkByte4 InternalEncrypt(float value, int key)
		{
			int num = key;
			if (num == 0)
			{
				num = ObscuredFloat.cryptoKey;
			}
			ObscuredFloat.FloatIntBytesUnion floatIntBytesUnion = default(ObscuredFloat.FloatIntBytesUnion);
			floatIntBytesUnion.f = value;
			floatIntBytesUnion.i ^= num;
			return floatIntBytesUnion.b4;
		}

		public static float Decrypt(int value)
		{
			return ObscuredFloat.Decrypt(value, ObscuredFloat.cryptoKey);
		}

		public static float Decrypt(int value, int key)
		{
			ObscuredFloat.FloatIntBytesUnion floatIntBytesUnion = default(ObscuredFloat.FloatIntBytesUnion);
			floatIntBytesUnion.i = (value ^ key);
			return floatIntBytesUnion.f;
		}

		public void ApplyNewCryptoKey()
		{
			if (this.currentCryptoKey != ObscuredFloat.cryptoKey)
			{
				this.hiddenValue = ObscuredFloat.InternalEncrypt(this.InternalDecrypt(), ObscuredFloat.cryptoKey);
				this.currentCryptoKey = ObscuredFloat.cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			float value = this.InternalDecrypt();
			do
			{
				this.currentCryptoKey = UnityEngine.Random.Range(-2147483648, 2147483647);
			}
			while (this.currentCryptoKey == 0);
			this.hiddenValue = ObscuredFloat.InternalEncrypt(value, this.currentCryptoKey);
		}

		public int GetEncrypted()
		{
			this.ApplyNewCryptoKey();
			ObscuredFloat.FloatIntBytesUnion floatIntBytesUnion = default(ObscuredFloat.FloatIntBytesUnion);
			floatIntBytesUnion.b4 = this.hiddenValue;
			return floatIntBytesUnion.i;
		}

		public void SetEncrypted(int encrypted)
		{
			this.inited = true;
			ObscuredFloat.FloatIntBytesUnion floatIntBytesUnion = default(ObscuredFloat.FloatIntBytesUnion);
			floatIntBytesUnion.i = encrypted;
			this.hiddenValue = floatIntBytesUnion.b4;
			if (this.currentCryptoKey == 0)
			{
				this.currentCryptoKey = ObscuredFloat.cryptoKey;
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
		}

		public float GetDecrypted()
		{
			return this.InternalDecrypt();
		}

		private float InternalDecrypt()
		{
			if (!this.inited)
			{
				this.currentCryptoKey = ObscuredFloat.cryptoKey;
				this.hiddenValue = ObscuredFloat.InternalEncrypt(0f);
				this.fakeValue = 0f;
				this.fakeValueActive = false;
				this.inited = true;
				return 0f;
			}
			ObscuredFloat.FloatIntBytesUnion floatIntBytesUnion = default(ObscuredFloat.FloatIntBytesUnion);
			floatIntBytesUnion.b4 = this.hiddenValue;
			floatIntBytesUnion.i ^= this.currentCryptoKey;
			float f = floatIntBytesUnion.f;
			if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && Math.Abs(f - this.fakeValue) > ObscuredCheatingDetector.Instance.floatEpsilon)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return f;
		}

		public static implicit operator ObscuredFloat(float value)
		{
			return new ObscuredFloat(value);
		}

		public static implicit operator float(ObscuredFloat value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredFloat operator ++(ObscuredFloat input)
		{
			float value = input.InternalDecrypt() + 1f;
			input.hiddenValue = ObscuredFloat.InternalEncrypt(value, input.currentCryptoKey);
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

		public static ObscuredFloat operator --(ObscuredFloat input)
		{
			float value = input.InternalDecrypt() - 1f;
			input.hiddenValue = ObscuredFloat.InternalEncrypt(value, input.currentCryptoKey);
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

		public override bool Equals(object obj)
		{
			return obj is ObscuredFloat && this.Equals((ObscuredFloat)obj);
		}

		public bool Equals(ObscuredFloat obj)
		{
			double num = (double)obj.InternalDecrypt();
			double obj2 = (double)this.InternalDecrypt();
			return num.Equals(obj2);
		}

		public override int GetHashCode()
		{
			return this.InternalDecrypt().GetHashCode();
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
	}
}
