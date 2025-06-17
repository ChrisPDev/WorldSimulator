using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSimulator.Models.Produce
{
    internal class Fungi : Produce
    {
        public Fungi()
        {
            Type = "Fungi";
            DecayAge = SetDecayAge(8, 25);
        }
    }
}
