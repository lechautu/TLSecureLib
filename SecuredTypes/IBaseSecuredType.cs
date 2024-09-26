namespace TLSecure.SecuredTypes
{
	public interface IBaseSecuredType<T>
	{
        /// <summary>
        /// Set new value
        /// </summary>
        /// <param name="value">Raw value to store</param>
        void SetValue(T value);

		/// <summary>
        /// Generate new crypto key and recalculate new encrypted value
        /// </summary>
		void RandomizeCryptoKey();

		/// <summary>
        /// Decrypt encrypted value
        /// </summary>
        /// <returns>Decrypted value</returns>
		T Decrypt();

		/// <summary>
        /// Encrypt the given value
        /// </summary>
        /// <param name="value">Value to encrypt</param>
        /// <returns>Encrypted value</returns>
		T Encrypt(T value);
	}
}