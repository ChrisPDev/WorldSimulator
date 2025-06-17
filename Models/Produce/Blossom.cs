using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSimulator.Models.Produce
{
    public class Blossom : Produce
    {
        public Blossom()
        {
            Type = "Blossom";
            DecayAge = SetDecayAge(2, 5);
        }
    }
}
