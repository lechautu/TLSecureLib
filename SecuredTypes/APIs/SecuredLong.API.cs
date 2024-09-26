using System;

namespace TLSecure.SecuredTypes
{
    public partial struct SecuredLong : IEquatable<SecuredLong>, IEquatable<long>, IComparable, IComparable<SecuredLong>, IComparable<long>
    {
        public int CompareTo(object obj)
        {
            return Decrypt().CompareTo(obj);
        }

        public int CompareTo(SecuredLong other)
        {
            return Decrypt().CompareTo(other.Decrypt());
        }

        public int CompareTo(long other)
        {
            return Decrypt().CompareTo(other);
        }

        public bool Equals(SecuredLong other)
        {
            return CompareTo(other) == 0;
        }

        public bool Equals(long other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString()
        {
            return Decrypt().ToString();
        }

        public static implicit operator SecuredLong(long value)
        {
            return new SecuredLong(value);
        }

        public static implicit operator long(SecuredLong value)
        {
            return value.Decrypt();
        }
    }
}