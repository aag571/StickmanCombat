// Decompile from assembly: Assembly-CSharp-firstpass.dll
// ILSpyBased#2
using CodeStage.AntiCheat.Detectors;
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
    [Serializable]
    public struct ObscuredChar : IEquatable<ObscuredChar>
    {
        private static char cryptoKey = '—';

        private char currentCryptoKey;

        private char hiddenValue;

        private bool inited;

        private char fakeValue;

        private bool fakeValueActive;

        private ObscuredChar(char value)
        {
            this.currentCryptoKey = ObscuredChar.cryptoKey;
            this.hiddenValue = ObscuredChar.EncryptDecrypt(value);
            bool isRunning = ObscuredCheatingDetector.IsRunning;
            this.fakeValue = (isRunning ? value : '\0');
            this.fakeValueActive = isRunning;
            this.inited = true;
        }

        public static void SetNewCryptoKey(char newKey)
        {
            ObscuredChar.cryptoKey = newKey;
        }

        public static char EncryptDecrypt(char value)
        {
            return ObscuredChar.EncryptDecrypt(value, '\0');
        }

        public static char EncryptDecrypt(char value, char key)
        {
            if (key == '\0')
            {
                return (char)(ushort)(value ^ ObscuredChar.cryptoKey);
            }
            return (char)(ushort)(value ^ key);
        }

        public void ApplyNewCryptoKey()
        {
            if (this.currentCryptoKey != ObscuredChar.cryptoKey)
            {
                this.hiddenValue = ObscuredChar.EncryptDecrypt(this.InternalDecrypt(), ObscuredChar.cryptoKey);
                this.currentCryptoKey = ObscuredChar.cryptoKey;
            }
        }

        public void RandomizeCryptoKey()
        {
            char value = this.InternalDecrypt();
            this.currentCryptoKey = (char)UnityEngine.Random.Range(1, 65535);
            this.hiddenValue = ObscuredChar.EncryptDecrypt(value, this.currentCryptoKey);
        }

        public char GetEncrypted()
        {
            this.ApplyNewCryptoKey();
            return this.hiddenValue;
        }

        public void SetEncrypted(char encrypted)
        {
            this.inited = true;
            this.hiddenValue = encrypted;
            if (this.currentCryptoKey == '\0')
            {
                this.currentCryptoKey = ObscuredChar.cryptoKey;
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

        public char GetDecrypted()
        {
            return this.InternalDecrypt();
        }

        private char InternalDecrypt()
        {
            if (!this.inited)
            {
                this.currentCryptoKey = ObscuredChar.cryptoKey;
                this.hiddenValue = ObscuredChar.EncryptDecrypt('\0');
                this.fakeValue = '\0';
                this.fakeValueActive = false;
                this.inited = true;
                return '\0';
            }
            char c = ObscuredChar.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey);
            if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && c != this.fakeValue)
            {
                ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }
            return c;
        }

        public static implicit operator ObscuredChar(char value)
        {
            return new ObscuredChar(value);
        }

        public static implicit operator char(ObscuredChar value)
        {
            return value.InternalDecrypt();
        }

        public static ObscuredChar operator ++(ObscuredChar input)
        {
            char value = (char)(input.InternalDecrypt() + 1);
            input.hiddenValue = ObscuredChar.EncryptDecrypt(value, input.currentCryptoKey);
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

        public static ObscuredChar operator --(ObscuredChar input)
        {
            char value = (char)(input.InternalDecrypt() - 1);
            input.hiddenValue = ObscuredChar.EncryptDecrypt(value, input.currentCryptoKey);
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
            if (!(obj is ObscuredChar))
            {
                return false;
            }
            return this.Equals((ObscuredChar)obj);
        }

        public bool Equals(ObscuredChar obj)
        {
            if (this.currentCryptoKey == obj.currentCryptoKey)
            {
                return this.hiddenValue == obj.hiddenValue;
            }
            return ObscuredChar.EncryptDecrypt(this.hiddenValue, this.currentCryptoKey) == ObscuredChar.EncryptDecrypt(obj.hiddenValue, obj.currentCryptoKey);
        }

        public override string ToString()
        {
            return this.InternalDecrypt().ToString();
        }

        public string ToString(IFormatProvider provider)
        {
            return this.InternalDecrypt().ToString(provider);
        }

        public override int GetHashCode()
        {
            return this.InternalDecrypt().GetHashCode();
        }
    }
}


