using UnityEngine;

namespace TLSecure.SecuredTypes
{
    public partial struct SecuredInt : IBaseSecuredType<int>
    {
		private bool isInit;

		private int currentCryptoKey;
		private int encrypted;

		public SecuredInt(int value)
		{
			currentCryptoKey = 0;
			encrypted = 0;
			isInit = false;

			RandomizeCryptoKey();
			encrypted = Encrypt(value);
		}

		public int Decrypt()
		{
			return encrypted ^ currentCryptoKey;
		}

		public int Encrypt(int value)
		{
			if (!isInit)
				RandomizeCryptoKey();
			return value ^ currentCryptoKey;
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
				encrypted = encrypted ^ currentCryptoKey ^ newCryptoKey;
			}
			currentCryptoKey = newCryptoKey;
			isInit = true;
		}

		public void SetValue(int value)
		{
			encrypted = Encrypt(value);
		}
	}
}