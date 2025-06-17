namespace WorldSimulator.Models.Produce
{
    public class Nut : Produce
    {
        public Nut()
        {
            Type = "Nut";
            DecayAge = SetDecayAge(10, 20);
        }
    }
}
