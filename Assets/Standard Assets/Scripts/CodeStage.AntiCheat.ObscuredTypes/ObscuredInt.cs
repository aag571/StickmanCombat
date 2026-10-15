// Decompile from assembly: Assembly-CSharp-firstpass.dll
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredInt : IEquatable<ObscuredInt>, IFormattable
	{
		private static int cryptoKey = 444444;

		[SerializeField]
		private int currentCryptoKey;

		[SerializeField]
		private int hiddenValue;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private int fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		private ObscuredInt(int value)
		{
			this.currentCryptoKey = ObscuredInt.cryptoKey;
			this.hiddenValue = ObscuredInt.Encrypt(value);
			bool isRunning = ObscuredCheatingDetector.IsRunning;
			this.fakeValue = ((!isRunning) ? 0 : value);
			this.fakeValueActive = isRunning;
			this.inited = true;
		}

		public static void SetNewCryptoKey(int newKey)
		{
			ObscuredInt.cryptoKey = newKey;
		}

		public static int Encrypt(int value)
		{
			return ObscuredInt.Encrypt(value, 0);
		}

		public static int Encrypt(int value, int key)
		{
			if (key == 0)
			{
				return value ^ ObscuredInt.cryptoKey;
			}
			return value ^ key;
		}

		public static int Decrypt(int value)
		{
			return ObscuredInt.Decrypt(value, 0);
		}

		public static int Decrypt(int value, int key)
		{
			if (key == 0)
			{
				return value ^ ObscuredInt.cryptoKey;
			}
			return value ^ key;
		}

		public void ApplyNewCryptoKey()
		{
			if (this.currentCryptoKey != ObscuredInt.cryptoKey)
			{
				this.hiddenValue = ObscuredInt.Encrypt(this.InternalDecrypt(), ObscuredInt.cryptoKey);
				this.currentCryptoKey = ObscuredInt.cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			this.hiddenValue = this.InternalDecrypt();
			do
			{
				this.currentCryptoKey = UnityEngine.Random.Range(-2147483648, 2147483647);
			}
			while (this.currentCryptoKey == 0);
			this.hiddenValue = ObscuredInt.Encrypt(this.hiddenValue, this.currentCryptoKey);
		}

		public int GetEncrypted()
		{
			this.ApplyNewCryptoKey();
			return this.hiddenValue;
		}

		public void SetEncrypted(int encrypted)
		{
			this.inited = true;
			this.hiddenValue = encrypted;
			if (this.currentCryptoKey == 0)
			{
				this.currentCryptoKey = ObscuredInt.cryptoKey;
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

		public int GetDecrypted()
		{
			return this.InternalDecrypt();
		}

		private int InternalDecrypt()
		{
			if (!this.inited)
			{
				this.currentCryptoKey = ObscuredInt.cryptoKey;
				this.hiddenValue = ObscuredInt.Encrypt(0);
				this.fakeValue = 0;
				this.fakeValueActive = false;
				this.inited = true;
				return 0;
			}
			int num = ObscuredInt.Decrypt(this.hiddenValue, this.currentCryptoKey);
			if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && num != this.fakeValue)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return num;
		}

		public static implicit operator ObscuredInt(int value)
		{
			return new ObscuredInt(value);
		}

		public static implicit operator int(ObscuredInt value)
		{
			return value.InternalDecrypt();
		}

		public static implicit operator ObscuredFloat(ObscuredInt value)
		{
			return (float)value.InternalDecrypt();
		}

		public static implicit operator ObscuredDouble(ObscuredInt value)
		{
			return (double)value.InternalDecrypt();
		}

		public static explicit operator ObscuredUInt(ObscuredInt value)
		{
			return (uint)value.InternalDecrypt();
		}

		public static ObscuredInt operator ++(ObscuredInt input)
		{
			int value = input.InternalDecrypt() + 1;
			input.hiddenValue = ObscuredInt.Encrypt(value, input.currentCryptoKey);
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

		public static ObscuredInt operator --(ObscuredInt input)
		{
			int value = input.InternalDecrypt() - 1;
			input.hiddenValue = ObscuredInt.Encrypt(value, input.currentCryptoKey);
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
			return obj is ObscuredInt && this.Equals((ObscuredInt)obj);
		}

		public bool Equals(ObscuredInt obj)
		{
			if (this.currentCryptoKey == obj.currentCryptoKey)
			{
				return this.hiddenValue == obj.hiddenValue;
			}
			return ObscuredInt.Decrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredInt.Decrypt(obj.hiddenValue, obj.currentCryptoKey);
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
