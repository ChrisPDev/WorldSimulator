using System;

namespace WorldSim.Core.Models
{
    public class MineralData : Evolvable<MineralType>
    {
        public MineralData()
        {
            Type = MineralType.None;
        }
    }
}
