using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TLSecure.SecuredTypes.Structs;

namespace TLSecure.SecuredTypes
{
    public partial struct SecuredDouble : IBaseSecuredType<double>
    {
        bool isInit;

        int currentCryptoKey;
        StructByte8 encrypted;

        public SecuredDouble(double value)
        {
            currentCryptoKey = 0;
            encrypted = new StructByte8();
            isInit = false;

            RandomizeCryptoKey();
            encrypted.d = Encrypt(value);
        }

        public double Decrypt()
        {
            StructByte8 tmp = encrypted;
            tmp.l ^= currentCryptoKey;
            return tmp.d;
        }

        public double Encrypt(double value)
        {
            if (!isInit)
                RandomizeCryptoKey();
            StructByte8 tmp = new StructByte8()
            {
                d = value
            };
            tmp.l ^= currentCryptoKey;
            return tmp.d;
        }

        public void RandomizeCryptoKey()
        {
            int newCryptoKey;
            do
            {
                newCryptoKey = Random.Range(int.MinValue, int.MaxValue);
            }
            while (newCryptoKey == 0 || newCryptoKey == currentCryptoKey);
            if (isInit)
            {
                encrypted.l = encrypted.l ^ currentCryptoKey ^ newCryptoKey;
            }
            currentCryptoKey = newCryptoKey;
        }

        public void SetValue(double value)
        {
            encrypted.d = Encrypt(value);
        }
    }
}