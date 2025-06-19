using WorldSimulator.Models.NatureBase;

namespace WorldSimulator.Models.Produce
{
    public class Fungi : Produce
    {
        public Fungi()
        {
            Type = ProduceType.Fungi;
            DecayAge = SetDecayAge(8, 25);
        }
    }
}
