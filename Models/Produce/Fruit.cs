using WorldSimulator.Models.NatureBase;

namespace WorldSimulator.Models.Produce
{
    public class Fruit : Produce
    {
        public Fruit()
        {
            Type = ProduceType.Fruit;
            DecayAge = SetDecayAge(3, 8);
        }
    }
}
