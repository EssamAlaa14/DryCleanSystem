
using Domain.ValueObjects.Base;

namespace Domain.ValueObjects
{
    public class Phone : ValueObject
    {
        public string Number { get; }

        public Phone(string number)
        {
            if (string.IsNullOrWhiteSpace(number) || number.Length < 7)
            {
                throw new ArgumentException("Invalid phone number", nameof(number));
            }
            Number = number;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

        public override string ToString()
        {
            return Number;
        }
    }
}
