using System;

namespace TLSecure.SecuredTypes
{
    public partial struct SecuredInt: IEquatable<int>, IEquatable<SecuredInt>, IComparable, IComparable<int>, IComparable<SecuredInt>
    {
        public int CompareTo(object obj)
        {
            return Decrypt().CompareTo(obj);
        }

        public int CompareTo(int other)
        {
            return Decrypt().CompareTo(other);
        }

        public int CompareTo(SecuredInt other)
        {
            return Decrypt().CompareTo(other.Decrypt());
        }

        public bool Equals(int other)
        {
            return CompareTo(other) == 0;
        }

        public bool Equals(SecuredInt other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString()
		{
            return Decrypt().ToString();
		}

		public static implicit operator int(SecuredInt securedInt)
		{
			return securedInt.Decrypt();
		}

		public static implicit operator SecuredInt(int value)
		{
			return new SecuredInt(value);
		}
	}
}