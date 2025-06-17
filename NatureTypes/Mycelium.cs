using System.Windows.Controls;
using System.Xml.Linq;
using System;

namespace WorldSimulator.Models.NatureBase
{
    public class Mycelium : ProduceCapableNature
    {
        public Mycelium(string name, params ProduceType[] produceTypes)
        {
            Name = name;
            Type = "Mycelium";
            Age = 0;
            Lifespan = SetLifespan(10, 1000);
            Stage = GrowthStage.Plant;
            SupportedProduceTypes = produceTypes.ToList();
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
