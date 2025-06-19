using WorldSimulator.Models.NatureBase;

namespace WorldSimulator.Models.Produce
{
    public class Blossom : Produce
    {
        public Blossom()
        {
            Type = ProduceType.Blossom;
            DecayAge = SetDecayAge(2, 5);
        }
    }
}
