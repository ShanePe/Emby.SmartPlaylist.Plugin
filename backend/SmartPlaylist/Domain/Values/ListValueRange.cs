namespace SmartPlaylist.Domain.Values
{
    public class ListValueRange : Value
    {
        public static readonly ListValueRange Default = new ListValueRange(ListValue.Default, ListValue.Default);

        public ListValueRange(ListValue from, ListValue to)
        {
            From = from;
            To = to;
        }

        public override string Kind => "listValueRange";
        public ListValue From { get; }
        public ListValue To { get; }

        internal override string Friendly => $"Range: {From.Friendly} to {To.Friendly}";

        public static ListValueRange Create(ListValue from, ListValue to)
        {
            return new ListValueRange(from, to);
        }

        protected bool Equals(ListValueRange other)
        {
            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ListValueRange)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (From.GetHashCode() * 397) ^ To.GetHashCode();
            }
        }

        public static bool operator ==(ListValueRange left, ListValueRange right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ListValueRange left, ListValueRange right)
        {
            return !Equals(left, right);
        }
    }
}