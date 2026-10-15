// Decompile from assembly: Assembly-CSharp-firstpass.dll
// ILSpyBased#2
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
    [Serializable]
    public struct ObscuredSByte : IEquatable<ObscuredSByte>, IFormattable
    {
        private static sbyte cryptoKey = 112;

        private sbyte currentCryptoKey;

        private sbyte hiddenValue;

        private bool inited;

        private sbyte fakeValue;

        private bool fakeValueActive;

        private ObscuredSByte(sbyte value)
        {
            this.currentCryptoKey = ObscuredSByte.cryptoKey;
            this.hiddenValue = ObscuredSByte.EncryptDecrypt(value);
            bool isRunning = ObscuredCheatingDetector.IsRunning;
            this.fakeValue = (sbyte)(isRunning ? value : 0);
            this.fakeValueActive = isRunning;
            this.inited = true;
        }

        public static void SetNewCryptoKey(sbyte newKey)
        {
            ObscuredSByte.cryptoKey = newKey;
        }

        public static sbyte EncryptDecrypt(sbyte value)
        {
            return ObscuredSByte.EncryptDecrypt(value, 0);
        }

        public static sbyte EncryptDecrypt(sbyte value, sbyte key)
        {
            if (key == 0)
            {
                return (sbyte)(value ^ ObscuredSByte.cryptoKey);
            }
            return (sbyte)(value ^ key);
        }

        public void ApplyNewCryptoKey()
        {
            if (this.currentCryptoKey != ObscuredSByte.cryptoKey)
            {
                this.hiddenValue = ObscuredSByte.EncryptDecrypt(this.InternalDecrypt(), ObscuredSByte.cryptoKey);
                this.currentCryptoKey = ObscuredSByte.cryptoKey;
            }
        }

        public void RandomizeCryptoKey()
        {
            sbyte value = this.InternalDecrypt();
            do
            {
                this.currentCryptoKey = (sbyte)UnityEngine.Random.Range(-128, 127);
            }
            while (this.currentCryptoKey == 0);
            this.hiddenValue = ObscuredSByte.EncryptDecrypt(value, this.currentCryptoKey);
        }

        public sbyte GetEncrypted()
        {
            this.ApplyNewCryptoKey();
            return this.hiddenValue;
        }

        public void SetEncrypted(sbyte encrypted)
        {
            this.inited = true;
            this.hiddenValue = encrypted;
            if (this.currentCryptoKey == 0)
            {
                this.currentCryptoKey = ObscuredSByte.cryptoKey;
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

        public sbyte GetDecrypted()
        {
            return this.InternalDecrypt();
        }

        private sbyte InternalDecrypt()
        {
            if (!this.inited)
            {
                this.currentCryptoKey = ObscuredSByte.cryptoKey;
                this.hiddenValue = ObscuredSByte.EncryptDecrypt(0);
                this.fakeValue = 0;
                this.fakeValueActive = false;
                this.inited = true;
                return 0;
            }
            sbyte b = ObscuredSByte.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey);
            if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && b != this.fakeValue)
            {
                ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }
            return b;
        }

        public static implicit operator ObscuredSByte(sbyte value)
        {
            return new ObscuredSByte(value);
        }

        public static implicit operator sbyte(ObscuredSByte value)
        {
            return value.InternalDecrypt();
        }

        public static ObscuredSByte operator ++(ObscuredSByte input)
        {
            sbyte value = (sbyte)(input.InternalDecrypt() + 1);
            input.hiddenValue = ObscuredSByte.EncryptDecrypt(value, input.currentCryptoKey);
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

        public static ObscuredSByte operator --(ObscuredSByte input)
        {
            sbyte value = (sbyte)(input.InternalDecrypt() - 1);
            input.hiddenValue = ObscuredSByte.EncryptDecrypt(value, input.currentCryptoKey);
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
            if (!(obj is ObscuredSByte))
            {
                return false;
            }
            return this.Equals((ObscuredSByte)obj);
        }

        public bool Equals(ObscuredSByte obj)
        {
            if (this.currentCryptoKey == obj.currentCryptoKey)
            {
                return this.hiddenValue == obj.hiddenValue;
            }
            return ObscuredSByte.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredSByte.EncryptDecrypt(obj.hiddenValue, obj.currentCryptoKey);
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


