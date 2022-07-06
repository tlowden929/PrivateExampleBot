namespace ExampleBot
{
    public class ExampleBoth
    {
        public int SetOnlyBody { get; set; }
        public int SetOnlyConstructor { get; set; }
        public int SetBoth { get; set; }

        public ExampleBoth(int constuctor, int both)
        {
            SetOnlyConstructor = constuctor;
            SetBoth = both;
        }
    }
}
