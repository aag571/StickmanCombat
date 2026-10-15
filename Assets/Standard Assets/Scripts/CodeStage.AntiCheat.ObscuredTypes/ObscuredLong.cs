// Decompile from assembly: Assembly-CSharp-firstpass.dll
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredLong : IEquatable<ObscuredLong>, IFormattable
	{
		private static long cryptoKey = 444442L;

		[SerializeField]
		private long currentCryptoKey;

		[SerializeField]
		private long hiddenValue;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private long fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		private ObscuredLong(long value)
		{
			this.currentCryptoKey = ObscuredLong.cryptoKey;
			this.hiddenValue = ObscuredLong.Encrypt(value);
			bool isRunning = ObscuredCheatingDetector.IsRunning;
			this.fakeValue = ((!isRunning) ? 0L : value);
			this.fakeValueActive = isRunning;
			this.inited = true;
		}

		public static void SetNewCryptoKey(long newKey)
		{
			ObscuredLong.cryptoKey = newKey;
		}

		public static long Encrypt(long value)
		{
			return ObscuredLong.Encrypt(value, 0L);
		}

		public static long Decrypt(long value)
		{
			return ObscuredLong.Decrypt(value, 0L);
		}

		public static long Encrypt(long value, long key)
		{
			if (key == 0L)
			{
				return value ^ ObscuredLong.cryptoKey;
			}
			return value ^ key;
		}

		public static long Decrypt(long value, long key)
		{
			if (key == 0L)
			{
				return value ^ ObscuredLong.cryptoKey;
			}
			return value ^ key;
		}

		public void ApplyNewCryptoKey()
		{
			if (this.currentCryptoKey != ObscuredLong.cryptoKey)
			{
				this.hiddenValue = ObscuredLong.Encrypt(this.InternalDecrypt(), ObscuredLong.cryptoKey);
				this.currentCryptoKey = ObscuredLong.cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			long value = this.InternalDecrypt();
			do
			{
				this.currentCryptoKey = (long)UnityEngine.Random.Range(-2147483648, 2147483647);
			}
			while (this.currentCryptoKey == 0L);
			this.hiddenValue = ObscuredLong.Encrypt(value, this.currentCryptoKey);
		}

		public long GetEncrypted()
		{
			this.ApplyNewCryptoKey();
			return this.hiddenValue;
		}

		public void SetEncrypted(long encrypted)
		{
			this.inited = true;
			this.hiddenValue = encrypted;
			if (this.currentCryptoKey == 0L)
			{
				this.currentCryptoKey = ObscuredLong.cryptoKey;
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

		public long GetDecrypted()
		{
			return this.InternalDecrypt();
		}

		private long InternalDecrypt()
		{
			if (!this.inited)
			{
				this.currentCryptoKey = ObscuredLong.cryptoKey;
				this.hiddenValue = ObscuredLong.Encrypt(0L);
				this.fakeValue = 0L;
				this.fakeValueActive = false;
				this.inited = true;
				return 0L;
			}
			long num = ObscuredLong.Decrypt(this.hiddenValue, this.currentCryptoKey);
			if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && num != this.fakeValue)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return num;
		}

		public static implicit operator ObscuredLong(long value)
		{
			return new ObscuredLong(value);
		}

		public static implicit operator long(ObscuredLong value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredLong operator ++(ObscuredLong input)
		{
			long value = input.InternalDecrypt() + 1L;
			input.hiddenValue = ObscuredLong.Encrypt(value, input.currentCryptoKey);
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

		public static ObscuredLong operator --(ObscuredLong input)
		{
			long value = input.InternalDecrypt() - 1L;
			input.hiddenValue = ObscuredLong.Encrypt(value, input.currentCryptoKey);
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
			return obj is ObscuredLong && this.Equals((ObscuredLong)obj);
		}

		public bool Equals(ObscuredLong obj)
		{
			if (this.currentCryptoKey == obj.currentCryptoKey)
			{
				return this.hiddenValue == obj.hiddenValue;
			}
			return ObscuredLong.Decrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredLong.Decrypt(obj.hiddenValue, obj.currentCryptoKey);
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
