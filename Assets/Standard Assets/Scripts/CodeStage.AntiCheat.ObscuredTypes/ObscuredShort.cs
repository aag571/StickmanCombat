// Decompile from assembly: Assembly-CSharp-firstpass.dll
// ILSpyBased#2
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
    [Serializable]
    public struct ObscuredShort : IEquatable<ObscuredShort>, IFormattable
    {
        private static short cryptoKey = 214;

        [SerializeField]
        private short currentCryptoKey;

        [SerializeField]
        private short hiddenValue;

        [SerializeField]
        private bool inited;

        [SerializeField]
        private short fakeValue;

        [SerializeField]
        private bool fakeValueActive;

        private ObscuredShort(short value)
        {
            this.currentCryptoKey = ObscuredShort.cryptoKey;
            this.hiddenValue = ObscuredShort.EncryptDecrypt(value);
            bool isRunning = ObscuredCheatingDetector.IsRunning;
            this.fakeValue = (short)(isRunning ? value : 0);
            this.fakeValueActive = isRunning;
            this.inited = true;
        }

        public static void SetNewCryptoKey(short newKey)
        {
            ObscuredShort.cryptoKey = newKey;
        }

        public static short EncryptDecrypt(short value)
        {
            return ObscuredShort.EncryptDecrypt(value, 0);
        }

        public static short EncryptDecrypt(short value, short key)
        {
            if (key == 0)
            {
                return (short)(value ^ ObscuredShort.cryptoKey);
            }
            return (short)(value ^ key);
        }

        public void ApplyNewCryptoKey()
        {
            if (this.currentCryptoKey != ObscuredShort.cryptoKey)
            {
                this.hiddenValue = ObscuredShort.EncryptDecrypt(this.InternalDecrypt(), ObscuredShort.cryptoKey);
                this.currentCryptoKey = ObscuredShort.cryptoKey;
            }
        }

        public void RandomizeCryptoKey()
        {
            short value = this.InternalDecrypt();
            do
            {
                this.currentCryptoKey = (short)UnityEngine.Random.Range(-32768, 32767);
            }
            while (this.currentCryptoKey == 0);
            this.hiddenValue = ObscuredShort.EncryptDecrypt(value, this.currentCryptoKey);
        }

        public short GetEncrypted()
        {
            this.ApplyNewCryptoKey();
            return this.hiddenValue;
        }

        public void SetEncrypted(short encrypted)
        {
            this.inited = true;
            this.hiddenValue = encrypted;
            if (this.currentCryptoKey == 0)
            {
                this.currentCryptoKey = ObscuredShort.cryptoKey;
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

        public short GetDecrypted()
        {
            return this.InternalDecrypt();
        }

        private short InternalDecrypt()
        {
            if (!this.inited)
            {
                this.currentCryptoKey = ObscuredShort.cryptoKey;
                this.hiddenValue = ObscuredShort.EncryptDecrypt(0);
                this.fakeValue = 0;
                this.fakeValueActive = false;
                this.inited = true;
                return 0;
            }
            short num = ObscuredShort.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey);
            if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && num != this.fakeValue)
            {
                ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }
            return num;
        }

        public static implicit operator ObscuredShort(short value)
        {
            return new ObscuredShort(value);
        }

        public static implicit operator short(ObscuredShort value)
        {
            return value.InternalDecrypt();
        }

        public static ObscuredShort operator ++(ObscuredShort input)
        {
            short value = (short)(input.InternalDecrypt() + 1);
            input.hiddenValue = ObscuredShort.EncryptDecrypt(value);
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

        public static ObscuredShort operator --(ObscuredShort input)
        {
            short value = (short)(input.InternalDecrypt() - 1);
            input.hiddenValue = ObscuredShort.EncryptDecrypt(value);
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
            if (!(obj is ObscuredShort))
            {
                return false;
            }
            return this.Equals((ObscuredShort)obj);
        }

        public bool Equals(ObscuredShort obj)
        {
            if (this.currentCryptoKey == obj.currentCryptoKey)
            {
                return this.hiddenValue == obj.hiddenValue;
            }
            return ObscuredShort.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredShort.EncryptDecrypt(obj.hiddenValue, obj.currentCryptoKey);
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


