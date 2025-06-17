using WorldSimulator.Models.Produce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using WorldSimulator.Models.NatureBase;

namespace WorldSimulator.Models.NatureBase
{
    public abstract class ProduceCapableNature : Nature
    {
        protected int _produceLimit = 10;
        protected static readonly Random rand = new Random();
        public List<Models.Produce.Produce> ProduceItems { get; private set; } = new List<Models.Produce.Produce>();
        public List<ProduceType> SupportedProduceTypes { get; set; } = new List<ProduceType>();
        public override string GetProduceType() => SupportedProduceTypes.Count == 0 ? "None" : string.Join(", ", SupportedProduceTypes);
        public override string GetProduceSummary()
        {
            if (ProduceItems.Count == 0) return "None";

            var summary = SupportedProduceTypes.Select(type => $"{type}s ({ProduceItems.Count(p => p.Type == type.ToString())} / {_produceLimit})");
            return string.Join(", ", summary);
        }
        public virtual void AttemptProduceGrowth()
        {
            if (Stage != GrowthStage.Grown && Stage != GrowthStage.Aged && Stage != GrowthStage.Old) return;

            foreach (var type in SupportedProduceTypes)
            {
                if (ProduceItems.Count(p => p.Type == type.ToString()) >= _produceLimit)
                {
                    continue;
                }

                if (rand.NextDouble() < 0.4)
                {
                    Models.Produce.Produce newProduce = type switch
                    {
                        ProduceType.Blossom => new Blossom(),
                        ProduceType.Fruit => new Fruit(),
                        ProduceType.Fungi => new Fungi(),
                        ProduceType.Nut => new Nut(),
                        _ => null
                    };

                    if (newProduce != null)
                    {
                        ProduceItems.Add(newProduce);
                        GrowthHistory.Add((Age, GrowthStage.Produced));
                        Logger?.LogProduced(Name, type.ToString(), Age);
                    }
                }
            }
        }
        public virtual void AgeAndDecayProduce()
        {
            for (int i = ProduceItems.Count - 1; i >= 0; i--)
            {
                ProduceItems[i].Age++;

                if (ProduceItems[i].Age >= ProduceItems[i].DecayAge)
                {
                    string type = ProduceItems[i].Type.ToLower();
                    ProduceItems.RemoveAt(i);
                    GrowthHistory.Add((Age, GrowthStage.Produced));
                    Logger?.LogDecayed(Name, type, Age);
                }
            }
        }
    }
}
