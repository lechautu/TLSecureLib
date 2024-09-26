using System;

namespace TLSecure.SecuredTypes
{
    public partial struct SecuredFloat : IEquatable<float>, IEquatable<SecuredFloat>, IComparable, IComparable<float>, IComparable<SecuredFloat>
	{
        public int CompareTo(object obj)
        {
            return Decrypt().CompareTo(obj);
        }

        public int CompareTo(float other)
        {
            return Decrypt().CompareTo(other);
        }

        public int CompareTo(SecuredFloat other)
        {
            return Decrypt().CompareTo(other.Decrypt());
        }

        public bool Equals(float other)
        {
            return CompareTo(other) == 0;
        }

        public bool Equals(SecuredFloat other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString()
		{
            return $"Value = {Decrypt()}" +
                $"\n{currentCryptoKey}";
		}

		public static implicit operator float(SecuredFloat securedFloat)
		{
			return securedFloat.Decrypt();
		}

		public static implicit operator SecuredFloat(float fVal)
		{
			return new SecuredFloat(fVal);
		}
	}
}