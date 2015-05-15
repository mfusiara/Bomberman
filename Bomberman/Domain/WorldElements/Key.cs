using Domain.DTO;

namespace Domain.WorldElements
{
    public class Key : WorldElement
    {
        public int Value { get; set; }

        public Key(int value)
        {
            Value = value;
        }

        public Key(int value, Coordinates coordinates) : base(coordinates)
        {
            Value = value;
        }

    }
}