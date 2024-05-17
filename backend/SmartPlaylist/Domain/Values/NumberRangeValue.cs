namespace SmartPlaylist.Domain.Values
{
    public class NumberRangeValue : EmptyableValue
    {
        public static readonly NumberRangeValue Default = new NumberRangeValue(0, 0);

        public NumberRangeValue(float from, float to)
        {
            From = from;
            To = to;
        }

        public override string Kind => "numberRange";
        public float From { get; }
        public float To { get; }

        internal override bool IsEmpty => IsNone || (From == 0 && To == 0);

        internal override string Friendly => $"Range: {From} to {To}";

        public static NumberRangeValue Create(float from, float to)
        {
            return new NumberRangeValue(from, to);
        }

        protected bool Equals(NumberRangeValue other)
        {
            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NumberRangeValue)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (From.GetHashCode() * 397) ^ To.GetHashCode();
            }
        }

        public static bool operator ==(NumberRangeValue left, NumberRangeValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NumberRangeValue left, NumberRangeValue right)
        {
            return !Equals(left, right);
        }
    }
}