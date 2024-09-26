using System;

namespace TLSecure.SecuredTypes
{
    public partial struct SecuredDouble : IEquatable<SecuredDouble>, IEquatable<double>, IComparable, IComparable<SecuredDouble>, IComparable<double>
    {
        public int CompareTo(object obj)
        {
            return Decrypt().CompareTo(obj);
        }

        public int CompareTo(SecuredDouble other)
        {
            return Decrypt().CompareTo(other.Decrypt());
        }

        public int CompareTo(double other)
        {
            return Decrypt().CompareTo(other);
        }

        public bool Equals(SecuredDouble other)
        {
            return CompareTo(other) == 0;
        }

        public bool Equals(double other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString()
        {
            return Decrypt().ToString();
        }

        public static implicit operator SecuredDouble(double value)
        {
            return new SecuredDouble(value);
        }

        public static implicit operator double(SecuredDouble secured)
        {
            return secured.Decrypt();
        }
    }
}