using WorldSimulator.Models.NatureBase;

namespace WorldSimulator.Models.Produce
{
    public class Nut : Produce
    {
        public Nut()
        {
            Type = ProduceType.Nut;
            DecayAge = SetDecayAge(10, 20);
        }
    }
}
