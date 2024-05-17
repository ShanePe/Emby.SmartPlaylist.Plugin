namespace SmartPlaylist.Domain.Values
{
    public class BoolValue : EmptyableValue
    {
        public static readonly BoolValue Default = new BoolValue(false);


        public BoolValue(bool value)
        {
            Value = value;
        }

        public override string Kind => "bool";

        public bool Value { get; }

        internal override bool IsEmpty => IsNone;

        internal override string Friendly => Value ? "True" : "False";

        public static Value Create(bool value)
        {
            return new BoolValue(value);
        }

        protected bool Equals(BoolValue other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BoolValue)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(BoolValue left, BoolValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BoolValue left, BoolValue right)
        {
            return !Equals(left, right);
        }
    }
}