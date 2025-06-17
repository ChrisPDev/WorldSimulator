namespace WorldSimulator.Models.Produce
{
    public class Produce
    {
        private static readonly Random rand = new Random();
        public string Type { get; set; }
        public int Age { get; set; } = 0;
        public int DecayAge { get; set; }

        public Produce()
        {
            DecayAge = SetDecayAge(5, 15);
        }

        public int SetDecayAge(int minDecayAge, int maxDecayAge)
        {
            return rand.Next(minDecayAge, maxDecayAge);
        }
    }
}
