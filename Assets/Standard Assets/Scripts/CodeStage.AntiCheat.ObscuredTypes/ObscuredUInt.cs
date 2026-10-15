// Decompile from assembly: Assembly-CSharp-firstpass.dll
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredUInt : IEquatable<ObscuredUInt>, IFormattable
	{
		private static uint cryptoKey = 240513u;

		[SerializeField]
		private uint currentCryptoKey;

		[SerializeField]
		private uint hiddenValue;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private uint fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		private ObscuredUInt(uint value)
		{
			this.currentCryptoKey = ObscuredUInt.cryptoKey;
			this.hiddenValue = ObscuredUInt.Encrypt(value);
			bool isRunning = ObscuredCheatingDetector.IsRunning;
			this.fakeValue = ((!isRunning) ? 0u : value);
			this.fakeValueActive = isRunning;
			this.inited = true;
		}

		public static void SetNewCryptoKey(uint newKey)
		{
			ObscuredUInt.cryptoKey = newKey;
		}

		public static uint Encrypt(uint value)
		{
			return ObscuredUInt.Encrypt(value, 0u);
		}

		public static uint Decrypt(uint value)
		{
			return ObscuredUInt.Decrypt(value, 0u);
		}

		public static uint Encrypt(uint value, uint key)
		{
			if (key == 0u)
			{
				return value ^ ObscuredUInt.cryptoKey;
			}
			return value ^ key;
		}

		public static uint Decrypt(uint value, uint key)
		{
			if (key == 0u)
			{
				return value ^ ObscuredUInt.cryptoKey;
			}
			return value ^ key;
		}

		public void ApplyNewCryptoKey()
		{
			if (this.currentCryptoKey != ObscuredUInt.cryptoKey)
			{
				this.hiddenValue = ObscuredUInt.Encrypt(this.InternalDecrypt(), ObscuredUInt.cryptoKey);
				this.currentCryptoKey = ObscuredUInt.cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			uint value = this.InternalDecrypt();
			this.currentCryptoKey = (uint)UnityEngine.Random.Range(1, 2147483647);
			this.hiddenValue = ObscuredUInt.Encrypt(value, this.currentCryptoKey);
		}

		public uint GetEncrypted()
		{
			this.ApplyNewCryptoKey();
			return this.hiddenValue;
		}

		public void SetEncrypted(uint encrypted)
		{
			this.inited = true;
			this.hiddenValue = encrypted;
			if (this.currentCryptoKey == 0u)
			{
				this.currentCryptoKey = ObscuredUInt.cryptoKey;
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

		public uint GetDecrypted()
		{
			return this.InternalDecrypt();
		}

		private uint InternalDecrypt()
		{
			if (!this.inited)
			{
				this.currentCryptoKey = ObscuredUInt.cryptoKey;
				this.hiddenValue = ObscuredUInt.Encrypt(0u);
				this.fakeValue = 0u;
				this.fakeValueActive = false;
				this.inited = true;
				return 0u;
			}
			uint num = ObscuredUInt.Decrypt(this.hiddenValue, this.currentCryptoKey);
			if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && num != this.fakeValue)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return num;
		}

		public static implicit operator ObscuredUInt(uint value)
		{
			return new ObscuredUInt(value);
		}

		public static implicit operator uint(ObscuredUInt value)
		{
			return value.InternalDecrypt();
		}

		public static explicit operator ObscuredInt(ObscuredUInt value)
		{
			return (int)value.InternalDecrypt();
		}

		public static ObscuredUInt operator ++(ObscuredUInt input)
		{
			uint value = input.InternalDecrypt() + 1u;
			input.hiddenValue = ObscuredUInt.Encrypt(value, input.currentCryptoKey);
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

		public static ObscuredUInt operator --(ObscuredUInt input)
		{
			uint value = input.InternalDecrypt() - 1u;
			input.hiddenValue = ObscuredUInt.Encrypt(value, input.currentCryptoKey);
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
			return obj is ObscuredUInt && this.Equals((ObscuredUInt)obj);
		}

		public bool Equals(ObscuredUInt obj)
		{
			if (this.currentCryptoKey == obj.currentCryptoKey)
			{
				return this.hiddenValue == obj.hiddenValue;
			}
			return ObscuredUInt.Decrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredUInt.Decrypt(obj.hiddenValue, obj.currentCryptoKey);
		}

		public override string ToString()
		{
			return this.InternalDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return this.InternalDecrypt().ToString(format);
		}

		public override int GetHashCode()
		{
			return this.InternalDecrypt().GetHashCode();
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
