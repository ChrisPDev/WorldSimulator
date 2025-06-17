using System.Windows.Controls;
using System.Xml.Linq;
using System;

namespace WorldSimulator.Models.NatureBase
{
    public class Tree : ProduceCapableNature
    {
        public Tree(string name, params ProduceType[] produceTypes)
        {
            Name = name;
            Type = "Tree";
            Age = 0;
            Lifespan = SetLifespan(270, 330);
            Stage = GrowthStage.Plant;
            SupportedProduceTypes = produceTypes.ToList();
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
