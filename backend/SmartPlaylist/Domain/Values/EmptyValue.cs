using SmartPlaylist.Domain.Values;

public class EmptyValue : Value
{
    public EmptyValue(string value)
    {
        Value = value;
    }
    public override string Kind { get; } = "empty";
    public string Value { get; }

    internal override string Friendly => "Null";

    public static readonly EmptyValue Default = new EmptyValue("empty");

    public static Value Create(string value)
    {
        return new EmptyValue(value);
    }
}