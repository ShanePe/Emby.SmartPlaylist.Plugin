using System;

[AttributeUsage(AttributeTargets.Field)]
public class EpimodeAttribute : Attribute
{
    public Type MediaType { get; }

    public EpimodeAttribute(Type mediaType)
    {
        MediaType = mediaType;
    }
}