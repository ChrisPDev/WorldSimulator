namespace WorldSimulator.Models.Produce
{
    public class Fruit : Produce
    {
        public Fruit()
        {
            Type = "Fruit";
            DecayAge = SetDecayAge(3, 8);
        }
    }
}
