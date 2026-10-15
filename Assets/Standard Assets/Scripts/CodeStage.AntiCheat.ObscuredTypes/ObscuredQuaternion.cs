// Decompile from assembly: Assembly-CSharp-firstpass.dll
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredQuaternion
	{
		[Serializable]
		public struct RawEncryptedQuaternion
		{
			public int x;

			public int y;

			public int z;

			public int w;
		}

		private static int cryptoKey = 120205;

		private static readonly Quaternion identity = Quaternion.identity;

		[SerializeField]
		private int currentCryptoKey;

		[SerializeField]
		private ObscuredQuaternion.RawEncryptedQuaternion hiddenValue;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private Quaternion fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		private ObscuredQuaternion(Quaternion value)
		{
			this.currentCryptoKey = ObscuredQuaternion.cryptoKey;
			this.hiddenValue = ObscuredQuaternion.Encrypt(value);
			bool isRunning = ObscuredCheatingDetector.IsRunning;
			this.fakeValue = ((!isRunning) ? ObscuredQuaternion.identity : value);
			this.fakeValueActive = isRunning;
			this.inited = true;
		}

		public ObscuredQuaternion(float x, float y, float z, float w)
		{
			this.currentCryptoKey = ObscuredQuaternion.cryptoKey;
			this.hiddenValue = ObscuredQuaternion.Encrypt(x, y, z, w, this.currentCryptoKey);
			if (ObscuredCheatingDetector.IsRunning)
			{
				this.fakeValue.x = x;
				this.fakeValue.y = y;
				this.fakeValue.z = z;
				this.fakeValue.w = w;
				this.fakeValueActive = true;
			}
			else
			{
				this.fakeValue = ObscuredQuaternion.identity;
				this.fakeValueActive = false;
			}
			this.inited = true;
		}

		public static void SetNewCryptoKey(int newKey)
		{
			ObscuredQuaternion.cryptoKey = newKey;
		}

		public static ObscuredQuaternion.RawEncryptedQuaternion Encrypt(Quaternion value)
		{
			return ObscuredQuaternion.Encrypt(value, 0);
		}

		public static ObscuredQuaternion.RawEncryptedQuaternion Encrypt(Quaternion value, int key)
		{
			return ObscuredQuaternion.Encrypt(value.x, value.y, value.z, value.w, key);
		}

		public static ObscuredQuaternion.RawEncryptedQuaternion Encrypt(float x, float y, float z, float w, int key)
		{
			if (key == 0)
			{
				key = ObscuredQuaternion.cryptoKey;
			}
			ObscuredQuaternion.RawEncryptedQuaternion result;
			result.x = ObscuredFloat.Encrypt(x, key);
			result.y = ObscuredFloat.Encrypt(y, key);
			result.z = ObscuredFloat.Encrypt(z, key);
			result.w = ObscuredFloat.Encrypt(w, key);
			return result;
		}

		public static Quaternion Decrypt(ObscuredQuaternion.RawEncryptedQuaternion value)
		{
			return ObscuredQuaternion.Decrypt(value, 0);
		}

		public static Quaternion Decrypt(ObscuredQuaternion.RawEncryptedQuaternion value, int key)
		{
			if (key == 0)
			{
				key = ObscuredQuaternion.cryptoKey;
			}
			Quaternion result;
			result.x = ObscuredFloat.Decrypt(value.x, key);
			result.y = ObscuredFloat.Decrypt(value.y, key);
			result.z = ObscuredFloat.Decrypt(value.z, key);
			result.w = ObscuredFloat.Decrypt(value.w, key);
			return result;
		}

		public void ApplyNewCryptoKey()
		{
			if (this.currentCryptoKey != ObscuredQuaternion.cryptoKey)
			{
				this.hiddenValue = ObscuredQuaternion.Encrypt(this.InternalDecrypt(), ObscuredQuaternion.cryptoKey);
				this.currentCryptoKey = ObscuredQuaternion.cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			Quaternion value = this.InternalDecrypt();
			do
			{
				this.currentCryptoKey = UnityEngine.Random.Range(-2147483648, 2147483647);
			}
			while (this.currentCryptoKey == 0);
			this.hiddenValue = ObscuredQuaternion.Encrypt(value, this.currentCryptoKey);
		}

		public ObscuredQuaternion.RawEncryptedQuaternion GetEncrypted()
		{
			this.ApplyNewCryptoKey();
			return this.hiddenValue;
		}

		public void SetEncrypted(ObscuredQuaternion.RawEncryptedQuaternion encrypted)
		{
			this.inited = true;
			this.hiddenValue = encrypted;
			if (this.currentCryptoKey == 0)
			{
				this.currentCryptoKey = ObscuredQuaternion.cryptoKey;
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

		public Quaternion GetDecrypted()
		{
			return this.InternalDecrypt();
		}

		private Quaternion InternalDecrypt()
		{
			if (!this.inited)
			{
				this.currentCryptoKey = ObscuredQuaternion.cryptoKey;
				this.hiddenValue = ObscuredQuaternion.Encrypt(ObscuredQuaternion.identity);
				this.fakeValue = ObscuredQuaternion.identity;
				this.fakeValueActive = false;
				this.inited = true;
				return ObscuredQuaternion.identity;
			}
			Quaternion quaternion;
			quaternion.x = ObscuredFloat.Decrypt(this.hiddenValue.x, this.currentCryptoKey);
			quaternion.y = ObscuredFloat.Decrypt(this.hiddenValue.y, this.currentCryptoKey);
			quaternion.z = ObscuredFloat.Decrypt(this.hiddenValue.z, this.currentCryptoKey);
			quaternion.w = ObscuredFloat.Decrypt(this.hiddenValue.w, this.currentCryptoKey);
			if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && !this.CompareQuaternionsWithTolerance(quaternion, this.fakeValue))
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return quaternion;
		}

		private bool CompareQuaternionsWithTolerance(Quaternion q1, Quaternion q2)
		{
			float quaternionEpsilon = ObscuredCheatingDetector.Instance.quaternionEpsilon;
			return Math.Abs(q1.x - q2.x) < quaternionEpsilon && Math.Abs(q1.y - q2.y) < quaternionEpsilon && Math.Abs(q1.z - q2.z) < quaternionEpsilon && Math.Abs(q1.w - q2.w) < quaternionEpsilon;
		}

		public static implicit operator ObscuredQuaternion(Quaternion value)
		{
			return new ObscuredQuaternion(value);
		}

		public static implicit operator Quaternion(ObscuredQuaternion value)
		{
			return value.InternalDecrypt();
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
	}
}
