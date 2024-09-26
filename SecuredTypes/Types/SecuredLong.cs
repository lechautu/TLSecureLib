using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLSecure.SecuredTypes
{
	public partial struct SecuredLong : IBaseSecuredType<long>
	{
		private bool isInit;

		private int currentCryptoKey;
		private long encrypted;

        public SecuredLong(long value)
        {
			currentCryptoKey = 0;
			encrypted = 0;
			isInit = false;

			RandomizeCryptoKey();
			encrypted = Encrypt(value);
		}

        public long Decrypt()
		{
			return encrypted ^ currentCryptoKey;
		}

		public long Encrypt(long value)
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

        public void SetValue(long value)
        {
			encrypted = Encrypt(value);
        }
	}
}