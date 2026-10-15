// Decompile from assembly: Assembly-CSharp-firstpass.dll
// ILSpyBased#2
using CodeStage.AntiCheat.Common;
using CodeStage.AntiCheat.Detectors;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
    [Serializable]
    public struct ObscuredDecimal : IEquatable<ObscuredDecimal>, IFormattable
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct DecimalLongBytesUnion
        {
            [FieldOffset(0)]
            public decimal d;

            [FieldOffset(0)]
            public long l1;

            [FieldOffset(8)]
            public long l2;

            [FieldOffset(0)]
            public ACTkByte16 b16;
        }

        private static long cryptoKey = 209208L;

        private long currentCryptoKey;

        private ACTkByte16 hiddenValue;

        private bool inited;

        private decimal fakeValue;

        private bool fakeValueActive;

        private ObscuredDecimal(decimal value)
        {
            this.currentCryptoKey = ObscuredDecimal.cryptoKey;
            this.hiddenValue = ObscuredDecimal.InternalEncrypt(value);
            bool isRunning = ObscuredCheatingDetector.IsRunning;
            this.fakeValue = ((!isRunning) ? 0m : value);
            this.fakeValueActive = isRunning;
            this.inited = true;
        }

        public static void SetNewCryptoKey(long newKey)
        {
            ObscuredDecimal.cryptoKey = newKey;
        }

        public static decimal Encrypt(decimal value)
        {
            return ObscuredDecimal.Encrypt(value, ObscuredDecimal.cryptoKey);
        }

        public static decimal Encrypt(decimal value, long key)
        {
            DecimalLongBytesUnion decimalLongBytesUnion = default(DecimalLongBytesUnion);
            decimalLongBytesUnion.d = value;
            decimalLongBytesUnion.l1 ^= key;
            decimalLongBytesUnion.l2 ^= key;
            return decimalLongBytesUnion.d;
        }

        private static ACTkByte16 InternalEncrypt(decimal value)
        {
            return ObscuredDecimal.InternalEncrypt(value, 0L);
        }

        private static ACTkByte16 InternalEncrypt(decimal value, long key)
        {
            long num = key;
            if (num == 0)
            {
                num = ObscuredDecimal.cryptoKey;
            }
            DecimalLongBytesUnion decimalLongBytesUnion = default(DecimalLongBytesUnion);
            decimalLongBytesUnion.d = value;
            decimalLongBytesUnion.l1 ^= num;
            decimalLongBytesUnion.l2 ^= num;
            return decimalLongBytesUnion.b16;
        }

        public static decimal Decrypt(decimal value)
        {
            return ObscuredDecimal.Decrypt(value, ObscuredDecimal.cryptoKey);
        }

        public static decimal Decrypt(decimal value, long key)
        {
            DecimalLongBytesUnion decimalLongBytesUnion = default(DecimalLongBytesUnion);
            decimalLongBytesUnion.d = value;
            decimalLongBytesUnion.l1 ^= key;
            decimalLongBytesUnion.l2 ^= key;
            return decimalLongBytesUnion.d;
        }

        public void ApplyNewCryptoKey()
        {
            if (this.currentCryptoKey != ObscuredDecimal.cryptoKey)
            {
                this.hiddenValue = ObscuredDecimal.InternalEncrypt(this.InternalDecrypt(), ObscuredDecimal.cryptoKey);
                this.currentCryptoKey = ObscuredDecimal.cryptoKey;
            }
        }

        public void RandomizeCryptoKey()
        {
            decimal value = this.InternalDecrypt();
            do
            {
                this.currentCryptoKey = UnityEngine.Random.Range(-2147483648, 2147483647);
            }
            while (this.currentCryptoKey == 0);
            this.hiddenValue = ObscuredDecimal.InternalEncrypt(value, this.currentCryptoKey);
        }

        public decimal GetEncrypted()
        {
            this.ApplyNewCryptoKey();
            DecimalLongBytesUnion decimalLongBytesUnion = default(DecimalLongBytesUnion);
            decimalLongBytesUnion.b16 = this.hiddenValue;
            return decimalLongBytesUnion.d;
        }

        public void SetEncrypted(decimal encrypted)
        {
            this.inited = true;
            DecimalLongBytesUnion decimalLongBytesUnion = default(DecimalLongBytesUnion);
            decimalLongBytesUnion.d = encrypted;
            this.hiddenValue = decimalLongBytesUnion.b16;
            if (this.currentCryptoKey == 0)
            {
                this.currentCryptoKey = ObscuredDecimal.cryptoKey;
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

        public decimal GetDecrypted()
        {
            return this.InternalDecrypt();
        }

        private decimal InternalDecrypt()
        {
            if (!this.inited)
            {
                this.currentCryptoKey = ObscuredDecimal.cryptoKey;
                this.hiddenValue = ObscuredDecimal.InternalEncrypt(0m);
                this.fakeValue = 0m;
                this.fakeValueActive = false;
                this.inited = true;
                return 0m;
            }
            DecimalLongBytesUnion decimalLongBytesUnion = default(DecimalLongBytesUnion);
            decimalLongBytesUnion.b16 = this.hiddenValue;
            decimalLongBytesUnion.l1 ^= this.currentCryptoKey;
            decimalLongBytesUnion.l2 ^= this.currentCryptoKey;
            decimal d = decimalLongBytesUnion.d;
            if (ObscuredCheatingDetector.IsRunning && this.fakeValueActive && d != this.fakeValue)
            {
                ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }
            return d;
        }

        public static implicit operator ObscuredDecimal(decimal value)
        {
            return new ObscuredDecimal(value);
        }

        public static implicit operator decimal(ObscuredDecimal value)
        {
            return value.InternalDecrypt();
        }

        public static explicit operator ObscuredDecimal(ObscuredFloat f)
        {
            return (decimal)(float)f;
        }

        public static ObscuredDecimal operator ++(ObscuredDecimal input)
        {
            decimal value = input.InternalDecrypt() + 1m;
            input.hiddenValue = ObscuredDecimal.InternalEncrypt(value, input.currentCryptoKey);
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

        public static ObscuredDecimal operator --(ObscuredDecimal input)
        {
            decimal value = input.InternalDecrypt() - 1m;
            input.hiddenValue = ObscuredDecimal.InternalEncrypt(value, input.currentCryptoKey);
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

        public override bool Equals(object obj)
        {
            if (!(obj is ObscuredDecimal))
            {
                return false;
            }
            return this.Equals((ObscuredDecimal)obj);
        }

        public bool Equals(ObscuredDecimal obj)
        {
            return obj.InternalDecrypt().Equals(this.InternalDecrypt());
        }

        public override int GetHashCode()
        {
            return this.InternalDecrypt().GetHashCode();
        }
    }
}


