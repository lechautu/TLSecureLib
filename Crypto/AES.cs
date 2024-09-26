using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;

namespace TLSecure.Crypto
{
	public class AES : BaseCrypto
	{
		private byte[] key;
		private byte[] iv;

		public AES(string passphrase, string hmacKey) : base(passphrase, hmacKey)
		{
			this.key = PBKDF2.DeriveKey(passphrase, this.salt, 1000, 32);
			this.iv = PBKDF2.DeriveKey(passphrase, this.salt, 1000, 16);
		}

		public override async Task<byte[]> Encrypt(byte[] data)
		{
			try
			{
				using (Aes aes = Aes.Create())
				{
					aes.Key = key;
					aes.IV = iv;

					using (MemoryStream ms = new())
					{
						using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
						{
							await cs.WriteAsync(data, 0, data.Length);
						}
						byte[] header = await GenerateHeader(ms.ToArray());
						using (MemoryStream ms2 = new())
						{
							await ms2.WriteAsync(header);
							await ms2.WriteAsync(ms.ToArray());
							return ms2.ToArray();
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error while encrypting data. {ex.Message}. {ex.StackTrace}");
			}
		}

		public override async Task<byte[]> Decrypt(byte[] data)
		{
			try
			{
				var (encryptedData, salt) = await ExtractHeader(data);
				using (Aes aes = Aes.Create())
				{
					aes.Key = PBKDF2.DeriveKey(passphrase, salt, 1000, 32);
					aes.IV = PBKDF2.DeriveKey(passphrase, salt, 1000, 16);

					ICryptoTransform decryptor = aes.CreateDecryptor();
					using (MemoryStream ms = new(encryptedData))
					{
						using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
						{
							using (MemoryStream destStream = new())
							{
								await cs.CopyToAsync(destStream);
								return destStream.ToArray();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error while decrypting data. {ex.Message}. {ex.StackTrace}");
			}
		}
	}
}
