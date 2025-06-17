using System.Windows.Controls;
using System.Xml.Linq;
using System;

namespace WorldSimulator.Models.NatureBase
{
    public class Fern : Nature
    {
        public Fern(string name)
        {
            Name = name;
            Type = "Fern";
            Age = 0;
            Lifespan = SetLifespan(5, 100);
            Stage = GrowthStage.Plant;
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
