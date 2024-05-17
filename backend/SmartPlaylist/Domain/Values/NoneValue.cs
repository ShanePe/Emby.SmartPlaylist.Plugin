namespace SmartPlaylist.Domain.Values
{
    public sealed class NoneValue : EmptyableValue
    {
        public override string Kind { get; } = nameof(NoneValue);

        internal override string Friendly => "No Value";

        internal override bool IsEmpty => true;
    }
}