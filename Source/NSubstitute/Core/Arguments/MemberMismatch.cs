using System;

namespace NSubstitute.Core.Arguments
{
    public class MemberMismatch : IEquatable<MemberMismatch>
    {
        public MemberMismatch(string member, object expected, object actual)
        {
            MemberName = member;
            Expected = expected;
            Actual = actual;
        }

        public string MemberName { get; private set; }
        public object Expected { get; private set; }
        public object Actual { get; private set; }

        public override string ToString()
        {
            return string.Format("For {0}, Expected: {1}, Actual: {2}", MemberName, Expected, Actual);
        }

        public bool Equals(MemberMismatch other)
        {
            return !ReferenceEquals(null, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (MemberMismatch)) return false;
            return Equals((MemberMismatch) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static bool operator ==(MemberMismatch left, MemberMismatch right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MemberMismatch left, MemberMismatch right)
        {
            return !Equals(left, right);
        }
    }
}