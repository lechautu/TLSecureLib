using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLSecure.Crypto
{
	public abstract class BaseCrypto
	{
		protected byte[] hmacKey;
		protected byte[] salt;
		protected string passphrase;

		public BaseCrypto(string passphrase, string hmacKey)
		{
			this.passphrase = passphrase;
			this.hmacKey = Encoding.UTF8.GetBytes(hmacKey);
			this.salt = RandomSalt();
		}

		public abstract Task<byte[]> Encrypt(byte[] data);
		public abstract Task<byte[]> Decrypt(byte[] data);

		private byte[] RandomSalt()
		{
			byte[] array = new byte[16];
			new Random().NextBytes(array);
			return array;
		}

		protected async Task<byte[]> GenerateHeader(byte[] data)
		{
			using (MemoryStream ms = new())
			{
				await ms.WriteAsync(salt);
				byte[] signature = HMAC.Sign(data, hmacKey);
				await ms.WriteAsync(signature);
				return ms.ToArray();
			}
		}

		public async Task<(byte[] encryptedData, byte[] salt)> ExtractHeader(byte[] data)
		{
			using (MemoryStream ms = new(data))
			{
				ms.Seek(0, SeekOrigin.Begin);

				byte[] salt = new byte[16];
				await ms.ReadAsync(salt, 0, salt.Length);

				byte[] signature = new byte[32];
				await ms.ReadAsync(signature, 0, signature.Length);

				byte[] realData = new byte[data.Length - ms.Position];
				await ms.ReadAsync(realData, 0, realData.Length);

				if (!signature.SequenceEqual(HMAC.Sign(realData, hmacKey)))
				{
					throw new InvalidDataException("Invalid signature");
				}

				return (realData, salt);
			}
		}
	}
}
