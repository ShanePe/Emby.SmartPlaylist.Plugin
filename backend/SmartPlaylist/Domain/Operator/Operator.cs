using SmartPlaylist.Contracts;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator
{
    public abstract class Operator
    {
        public abstract string Name { get; }

        public virtual string Type => DefaultValue.Kind;

        public abstract Value DefaultValue { get; }

        public abstract bool Compare(Value itemValue, Value value);

        public virtual bool CanCompare(Value itemValue, Value value)
        {
            return !value.IsNone && itemValue.IsType(value.GetType());
        }

        public abstract bool Valueless { get; }

        public OperatorDto ToDto()
        {
            return new OperatorDto
            {
                Name = Name,
                Type = Type
            };
        }
    }


    public abstract class OperatorGen<T1, T2> : Operator
        where T1 : Value
        where T2 : Value
    {
        public abstract bool Compare(T1 itemValue, T2 value);
        public abstract bool Compare(ArrayValue<T1> itemValues, T2 value);

        public override bool CanCompare(Value itemValue, Value value)
        {
            return !value.IsNone && (itemValue.IsType(typeof(T1)) || itemValue.IsType(typeof(ArrayValue<T1>))) && value.IsType(typeof(T2));
        }

        public override bool Valueless => false;
    }


}