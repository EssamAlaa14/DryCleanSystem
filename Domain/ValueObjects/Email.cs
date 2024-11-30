using Domain.ValueObjects.Base;

namespace Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
            {
                throw new ArgumentException("Invalid email address", nameof(value));
            }
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
