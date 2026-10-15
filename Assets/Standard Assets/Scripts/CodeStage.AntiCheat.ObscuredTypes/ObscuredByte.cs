// Decompile from assembly: Assembly-CSharp-firstpass.dll
// ILSpyBased#2
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
    [Serializable]
    public struct ObscuredByte : IEquatable<ObscuredByte>, IFormattable
    {
        private static byte cryptoKey = 244;

        private byte currentCryptoKey;

        private byte hiddenValue;

        private bool inited;

        private byte fakeValue;

        private bool fakeValueActive;

        private ObscuredByte(byte value)
        {
            this.currentCryptoKey = ObscuredByte.cryptoKey;
            this.hiddenValue = ObscuredByte.EncryptDecrypt(value);
            bool isRunning = ObscuredCheatingDetector.IsRunning;
            this.fakeValue = (byte)(isRunning ? value : 0);
            this.fakeValueActive = isRunning;
            this.inited = true;
        }

        public static void SetNewCryptoKey(byte newKey)
        {
            ObscuredByte.cryptoKey = newKey;
        }

        public static byte EncryptDecrypt(byte value)
        {
            return ObscuredByte.EncryptDecrypt(value, 0);
        }

        public static byte EncryptDecrypt(byte value, byte key)
        {
            if (key == 0)
            {
                return (byte)(value ^ ObscuredByte.cryptoKey);
            }
            return (byte)(value ^ key);
        }

        public void ApplyNewCryptoKey()
        {
            if (this.currentCryptoKey != ObscuredByte.cryptoKey)
            {
                this.hiddenValue = ObscuredByte.EncryptDecrypt(this.InternalDecrypt(), ObscuredByte.cryptoKey);
                this.currentCryptoKey = ObscuredByte.cryptoKey;
            }
        }

        public void RandomizeCryptoKey()
        {
            byte value = this.InternalDecrypt();
            this.currentCryptoKey = (byte)UnityEngine.Random.Range(1, 255);
            this.hiddenValue = ObscuredByte.EncryptDecrypt(value, this.currentCryptoKey);
        }

        public byte GetEncrypted()
        {
            this.ApplyNewCryptoKey();
            return this.hiddenValue;
        }

        public void SetEncrypted(byte encrypted)
        {
            this.inited = true;
            this.hiddenValue = encrypted;
            if (this.currentCryptoKey == 0)
            {
                this.currentCryptoKey = ObscuredByte.cryptoKey;
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

        public byte GetDecrypted()
        {
            return this.InternalDecrypt();
        }

        private byte InternalDecrypt()
        {
            if (!this.inited)
            {
                this.currentCryptoKey = ObscuredByte.cryptoKey;
                this.hiddenValue = ObscuredByte.EncryptDecrypt(0);
                this.fakeValue = 0;
                this.fakeValueActive = false;
                this.inited = true;
                return 0;
            }
            byte b = ObscuredByte.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey);
            if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && b != this.fakeValue)
            {
                ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }
            return b;
        }

        public static implicit operator ObscuredByte(byte value)
        {
            return new ObscuredByte(value);
        }

        public static implicit operator byte(ObscuredByte value)
        {
            return value.InternalDecrypt();
        }

        public static ObscuredByte operator ++(ObscuredByte input)
        {
            byte value = (byte)(input.InternalDecrypt() + 1);
            input.hiddenValue = ObscuredByte.EncryptDecrypt(value, input.currentCryptoKey);
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

        public static ObscuredByte operator --(ObscuredByte input)
        {
            byte value = (byte)(input.InternalDecrypt() - 1);
            input.hiddenValue = ObscuredByte.EncryptDecrypt(value, input.currentCryptoKey);
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
            if (!(obj is ObscuredByte))
            {
                return false;
            }
            return this.Equals((ObscuredByte)obj);
        }

        public bool Equals(ObscuredByte obj)
        {
            if (this.currentCryptoKey == obj.currentCryptoKey)
            {
                return this.hiddenValue == obj.hiddenValue;
            }
            return ObscuredByte.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredByte.EncryptDecrypt(obj.hiddenValue, obj.currentCryptoKey);
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


