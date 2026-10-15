// Decompile from assembly: Assembly-CSharp-firstpass.dll
// ILSpyBased#2
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
    [Serializable]
    public sealed class ObscuredString
    {
        private static string cryptoKey = "4441";

        [SerializeField]
        private string currentCryptoKey;

        [SerializeField]
        private byte[] hiddenValue;

        [SerializeField]
        private bool inited;

        [SerializeField]
        private string fakeValue;

        [SerializeField]
        private bool fakeValueActive;

        public int Length
        {
            get
            {
                return this.hiddenValue.Length / 2;
            }
        }

        private ObscuredString()
        {
        }

        private ObscuredString(string value)
        {
            this.currentCryptoKey = ObscuredString.cryptoKey;
            this.hiddenValue = ObscuredString.InternalEncrypt(value);
            bool isRunning = ObscuredCheatingDetector.IsRunning;
            this.fakeValue = ((!isRunning) ? null : value);
            this.fakeValueActive = isRunning;
            this.inited = true;
        }

        public static void SetNewCryptoKey(string newKey)
        {
            ObscuredString.cryptoKey = newKey;
        }

        public static string EncryptDecrypt(string value)
        {
            return ObscuredString.EncryptDecrypt(value, string.Empty);
        }

        public static string EncryptDecrypt(string value, string key)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(key))
            {
                key = ObscuredString.cryptoKey;
            }
            int length = key.Length;
            int length2 = value.Length;
            char[] array = new char[length2];
            for (int i = 0; i < length2; i++)
            {
                array[i] = (char)(value[i] ^ key[i % length]);
            }
            return new string(array);
        }

        public void ApplyNewCryptoKey()
        {
            if (this.currentCryptoKey != ObscuredString.cryptoKey)
            {
                this.hiddenValue = ObscuredString.InternalEncrypt(this.InternalDecrypt());
                this.currentCryptoKey = ObscuredString.cryptoKey;
            }
        }

        public void RandomizeCryptoKey()
        {
            string value = this.InternalDecrypt();
            this.currentCryptoKey = UnityEngine.Random.Range(-2147483648, 2147483647).ToString();
            this.hiddenValue = ObscuredString.InternalEncrypt(value, this.currentCryptoKey);
        }

        public string GetEncrypted()
        {
            this.ApplyNewCryptoKey();
            return ObscuredString.GetString(this.hiddenValue);
        }

        public void SetEncrypted(string encrypted)
        {
            this.inited = true;
            this.hiddenValue = ObscuredString.GetBytes(encrypted);
            if (string.IsNullOrEmpty(this.currentCryptoKey))
            {
                this.currentCryptoKey = ObscuredString.cryptoKey;
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

        public string GetDecrypted()
        {
            return this.InternalDecrypt();
        }

        private static byte[] InternalEncrypt(string value)
        {
            return ObscuredString.InternalEncrypt(value, ObscuredString.cryptoKey);
        }

        private static byte[] InternalEncrypt(string value, string key)
        {
            return ObscuredString.GetBytes(ObscuredString.EncryptDecrypt(value, key));
        }

        private string InternalDecrypt()
        {
            if (!this.inited)
            {
                this.currentCryptoKey = ObscuredString.cryptoKey;
                this.hiddenValue = ObscuredString.InternalEncrypt(string.Empty);
                this.fakeValue = string.Empty;
                this.fakeValueActive = false;
                this.inited = true;
                return string.Empty;
            }
            string text = this.currentCryptoKey;
            if (string.IsNullOrEmpty(text))
            {
                text = ObscuredString.cryptoKey;
            }
            string text2 = ObscuredString.EncryptDecrypt(ObscuredString.GetString(this.hiddenValue), text);
            if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && text2 != this.fakeValue)
            {
                ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }
            return text2;
        }

        public static implicit operator ObscuredString(string value)
        {
            return (value != null) ? new ObscuredString(value) : null;
        }

        public static implicit operator string(ObscuredString value)
        {
            return (!(value == (ObscuredString)null)) ? value.InternalDecrypt() : null;
        }

        public override string ToString()
        {
            return this.InternalDecrypt();
        }

        public static bool operator ==(ObscuredString a, ObscuredString b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }
            if ((object)a != null && (object)b != null)
            {
                if (a.currentCryptoKey == b.currentCryptoKey)
                {
                    return ObscuredString.ArraysEquals(a.hiddenValue, b.hiddenValue);
                }
                return string.Equals(a.InternalDecrypt(), b.InternalDecrypt());
            }
            return false;
        }

        public static bool operator !=(ObscuredString a, ObscuredString b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ObscuredString))
            {
                return false;
            }
            return this.Equals((ObscuredString)obj);
        }

        public bool Equals(ObscuredString value)
        {
            if (value == (ObscuredString)null)
            {
                return false;
            }
            if (this.currentCryptoKey == value.currentCryptoKey)
            {
                return ObscuredString.ArraysEquals(this.hiddenValue, value.hiddenValue);
            }
            return string.Equals(this.InternalDecrypt(), value.InternalDecrypt());
        }

        public bool Equals(ObscuredString value, StringComparison comparisonType)
        {
            if (value == (ObscuredString)null)
            {
                return false;
            }
            return string.Equals(this.InternalDecrypt(), value.InternalDecrypt(), comparisonType);
        }

        public override int GetHashCode()
        {
            return this.InternalDecrypt().GetHashCode();
        }

        private static byte[] GetBytes(string str)
        {
            byte[] array = new byte[str.Length * 2];
            Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
            return array;
        }

        private static string GetString(byte[] bytes)
        {
            char[] array = new char[bytes.Length / 2];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
            return new string(array);
        }

        private static bool ArraysEquals(byte[] a1, byte[] a2)
        {
            if (a1 == a2)
            {
                return true;
            }
            if (a1 != null && a2 != null)
            {
                if (a1.Length != a2.Length)
                {
                    return false;
                }
                for (int i = 0; i < a1.Length; i++)
                {
                    if (a1[i] != a2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}


