namespace Domain.WorldElements
{
    public class Door : WorldElement
    {
        public int Key { get; protected set; }
        public Door(int key)
        {
            Key = key;
        } 

        public  Door(int key, Coordinates coordinates) : base(coordinates)
        {
            Key = key;
        }

        public bool Open(Key key)
        {
            return true;
        }
    }
}