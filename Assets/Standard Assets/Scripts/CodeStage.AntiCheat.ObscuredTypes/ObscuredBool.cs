// Decompile from assembly: Assembly-CSharp-firstpass.dll
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredBool : IEquatable<ObscuredBool>
	{
		private static byte cryptoKey = 215;

		[SerializeField]
		private byte currentCryptoKey;

		[SerializeField]
		private int hiddenValue;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private bool fakeValue;

		[FormerlySerializedAs("fakeValueChanged"), SerializeField]
		private bool fakeValueActive;

		private ObscuredBool(bool value)
		{
			this.currentCryptoKey = ObscuredBool.cryptoKey;
			this.hiddenValue = ObscuredBool.Encrypt(value);
			bool isRunning = ObscuredCheatingDetector.IsRunning;
			this.fakeValue = (isRunning && value);
			this.fakeValueActive = isRunning;
			this.inited = true;
		}

		public static void SetNewCryptoKey(byte newKey)
		{
			ObscuredBool.cryptoKey = newKey;
		}

		public static int Encrypt(bool value)
		{
			return ObscuredBool.Encrypt(value, 0);
		}

		public static int Encrypt(bool value, byte key)
		{
			if (key == 0)
			{
				key = ObscuredBool.cryptoKey;
			}
			int num = (!value) ? 181 : 213;
			return num ^ (int)key;
		}

		public static bool Decrypt(int value)
		{
			return ObscuredBool.Decrypt(value, 0);
		}

		public static bool Decrypt(int value, byte key)
		{
			if (key == 0)
			{
				key = ObscuredBool.cryptoKey;
			}
			value ^= (int)key;
			return value != 181;
		}

		public void ApplyNewCryptoKey()
		{
			if (this.currentCryptoKey != ObscuredBool.cryptoKey)
			{
				this.hiddenValue = ObscuredBool.Encrypt(this.InternalDecrypt(), ObscuredBool.cryptoKey);
				this.currentCryptoKey = ObscuredBool.cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			bool value = this.InternalDecrypt();
			this.currentCryptoKey = (byte)UnityEngine.Random.Range(1, 150);
			this.hiddenValue = ObscuredBool.Encrypt(value, this.currentCryptoKey);
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
				this.currentCryptoKey = ObscuredBool.cryptoKey;
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

		public bool GetDecrypted()
		{
			return this.InternalDecrypt();
		}

		private bool InternalDecrypt()
		{
			if (!this.inited)
			{
				this.currentCryptoKey = ObscuredBool.cryptoKey;
				this.hiddenValue = ObscuredBool.Encrypt(false);
				this.fakeValue = false;
				this.fakeValueActive = false;
				this.inited = true;
				return false;
			}
			int num = this.hiddenValue;
			num ^= (int)this.currentCryptoKey;
			bool flag = num != 181;
			if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && flag != this.fakeValue)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return flag;
		}

		public static implicit operator ObscuredBool(bool value)
		{
			return new ObscuredBool(value);
		}

		public static implicit operator bool(ObscuredBool value)
		{
			return value.InternalDecrypt();
		}

		public override bool Equals(object obj)
		{
			return obj is ObscuredBool && this.Equals((ObscuredBool)obj);
		}

		public bool Equals(ObscuredBool obj)
		{
			if (this.currentCryptoKey == obj.currentCryptoKey)
			{
				return this.hiddenValue == obj.hiddenValue;
			}
			return ObscuredBool.Decrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredBool.Decrypt(obj.hiddenValue, obj.currentCryptoKey);
		}

		public override int GetHashCode()
		{
			return this.InternalDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return this.InternalDecrypt().ToString();
		}
	}
}
