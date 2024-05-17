using System;
using MediaBrowser.Model.Serialization;

namespace SmartPlaylist.Domain.Values
{
    public abstract class Value
    {
        public static readonly NoneValue None = new NoneValue();
        public abstract string Kind { get; }
        internal abstract string Friendly { get; }

        [IgnoreDataMember] public virtual bool IsNone => this == None;

        public bool IsType(Type type)
        {
            return GetType() == type;
        }
    }

    public abstract class EmptyableValue : Value
    {
        internal abstract bool IsEmpty { get; }
    }
}