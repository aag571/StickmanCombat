// Decompile from assembly: Assembly-CSharp-firstpass.dll
// ILSpyBased#2
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
    [Serializable]
    public struct ObscuredUShort : IEquatable<ObscuredUShort>, IFormattable
    {
        private static ushort cryptoKey = 224;

        private ushort currentCryptoKey;

        private ushort hiddenValue;

        private bool inited;

        private ushort fakeValue;

        private bool fakeValueActive;

        private ObscuredUShort(ushort value)
        {
            this.currentCryptoKey = ObscuredUShort.cryptoKey;
            this.hiddenValue = ObscuredUShort.EncryptDecrypt(value);
            bool isRunning = ObscuredCheatingDetector.IsRunning;
            this.fakeValue = (ushort)(isRunning ? value : 0);
            this.fakeValueActive = isRunning;
            this.inited = true;
        }

        public static void SetNewCryptoKey(ushort newKey)
        {
            ObscuredUShort.cryptoKey = newKey;
        }

        public static ushort EncryptDecrypt(ushort value)
        {
            return ObscuredUShort.EncryptDecrypt(value, 0);
        }

        public static ushort EncryptDecrypt(ushort value, ushort key)
        {
            if (key == 0)
            {
                return (ushort)(value ^ ObscuredUShort.cryptoKey);
            }
            return (ushort)(value ^ key);
        }

        public void ApplyNewCryptoKey()
        {
            if (this.currentCryptoKey != ObscuredUShort.cryptoKey)
            {
                this.hiddenValue = ObscuredUShort.EncryptDecrypt(this.InternalDecrypt(), ObscuredUShort.cryptoKey);
                this.currentCryptoKey = ObscuredUShort.cryptoKey;
            }
        }

        public void RandomizeCryptoKey()
        {
            ushort value = this.InternalDecrypt();
            this.currentCryptoKey = (ushort)UnityEngine.Random.Range(1, 32767);
            this.hiddenValue = ObscuredUShort.EncryptDecrypt(value, this.currentCryptoKey);
        }

        public ushort GetEncrypted()
        {
            this.ApplyNewCryptoKey();
            return this.hiddenValue;
        }

        public void SetEncrypted(ushort encrypted)
        {
            this.inited = true;
            this.hiddenValue = encrypted;
            if (this.currentCryptoKey == 0)
            {
                this.currentCryptoKey = ObscuredUShort.cryptoKey;
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

        public ushort GetDecrypted()
        {
            return this.InternalDecrypt();
        }

        private ushort InternalDecrypt()
        {
            if (!this.inited)
            {
                this.currentCryptoKey = ObscuredUShort.cryptoKey;
                this.hiddenValue = ObscuredUShort.EncryptDecrypt(0);
                this.fakeValue = 0;
                this.fakeValueActive = false;
                this.inited = true;
                return 0;
            }
            ushort num = ObscuredUShort.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey);
            if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && num != this.fakeValue)
            {
                ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }
            return num;
        }

        public static implicit operator ObscuredUShort(ushort value)
        {
            return new ObscuredUShort(value);
        }

        public static implicit operator ushort(ObscuredUShort value)
        {
            return value.InternalDecrypt();
        }

        public static ObscuredUShort operator ++(ObscuredUShort input)
        {
            ushort value = (ushort)(input.InternalDecrypt() + 1);
            input.hiddenValue = ObscuredUShort.EncryptDecrypt(value, input.currentCryptoKey);
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

        public static ObscuredUShort operator --(ObscuredUShort input)
        {
            ushort value = (ushort)(input.InternalDecrypt() - 1);
            input.hiddenValue = ObscuredUShort.EncryptDecrypt(value, input.currentCryptoKey);
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
            if (!(obj is ObscuredUShort))
            {
                return false;
            }
            return this.Equals((ObscuredUShort)obj);
        }

        public bool Equals(ObscuredUShort obj)
        {
            if (this.currentCryptoKey == obj.currentCryptoKey)
            {
                return this.hiddenValue == obj.hiddenValue;
            }
            return ObscuredUShort.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredUShort.EncryptDecrypt(obj.hiddenValue, obj.currentCryptoKey);
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


