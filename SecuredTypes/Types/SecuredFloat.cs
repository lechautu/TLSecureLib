using UnityEngine;
using TLSecure.SecuredTypes.Structs;

namespace TLSecure.SecuredTypes
{
	public partial struct SecuredFloat : IBaseSecuredType<float>
	{
		private bool isInit;

		private int currentCryptoKey;
		private StructByte4 encrypted;

        public SecuredFloat(float fVal)
        {
			currentCryptoKey = 0;
			encrypted = new StructByte4();
			isInit = false;

			RandomizeCryptoKey();
			encrypted.f = Encrypt(fVal);
		}

		public float Decrypt()
		{
			StructByte4 tmp = encrypted;
			tmp.i ^= currentCryptoKey;
			return tmp.f;
		}

		public float Encrypt(float value)
		{
			if (!isInit)
				RandomizeCryptoKey();
			StructByte4 tmp = new StructByte4()
			{
				f = value
			};
			tmp.i ^= currentCryptoKey;
			return tmp.f;
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
				encrypted.i = encrypted.i ^ currentCryptoKey ^ newCryptoKey;
			}
			currentCryptoKey = newCryptoKey;
			isInit = true;
		}

        public void SetValue(float value)
        {
			encrypted.f = Encrypt(value);
        }
    }
}