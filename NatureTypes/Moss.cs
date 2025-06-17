using System.Windows.Controls;
using System.Xml.Linq;
using System;

namespace WorldSimulator.Models.NatureBase
{
    public class Moss : Nature
    {
        public Moss(string name)
        {
            Name = name;
            Type = "Moss";
            Age = 0;
            Lifespan = SetLifespan(10, 100);
            Stage = GrowthStage.Plant;
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
