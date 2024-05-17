namespace SmartPlaylist.Domain.Values
{
    public class NumberValue : EmptyableValue
    {
        public static readonly NumberValue Default = new NumberValue(0);

        public NumberValue(float value)
        {
            Value = value;
        }

        public override string Kind => "number";

        public float Value { get; }

        internal override bool IsEmpty => IsNone || Value == 0f;

        internal override string Friendly => Value.ToString();

        public static NumberValue Create(float value)
        {
            return new NumberValue(value);
        }

        protected bool Equals(NumberValue other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NumberValue)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(NumberValue left, NumberValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NumberValue left, NumberValue right)
        {
            return !Equals(left, right);
        }

        public bool IsInRange(NumberRangeValue range)
        {
            return Value >= range.From && Value <= range.To;
        }
    }
}