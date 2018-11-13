namespace Day_11
{
    public struct Transition
    {
        public int Direction;
        public int[] ItemIndicies;

        public Transition(int direction, int[] itemIndicies)
        {
            Direction = direction;
            ItemIndicies = itemIndicies;
        }
    }
}