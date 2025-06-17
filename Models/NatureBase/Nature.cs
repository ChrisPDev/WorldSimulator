using WorldSimulator.Utils;
using System.Diagnostics;

namespace WorldSimulator.Models.NatureBase
{
    public class Nature
    {
        private static readonly Random rand = new Random();

        private string _name;
        private string _type;
        private int _age;
        private int _lifespan;
        private GrowthStage _stage;
        public List<(int Age, GrowthStage Stage)> GrowthHistory { get; } = new List<(int Age, GrowthStage Stage)>();
        public List<string> LogEntries { get; } = new List<string>();
        public NatureLogger Logger { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
            }
        }
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
            }
        }
        public int Lifespan
        {
            get => _lifespan;
            set
            {
                _lifespan = value;
            }
        }
        public GrowthStage Stage
        {
            get => _stage;
            set
            {
                if (value != GrowthStage.Produced)
                {
                    _stage = value;
                }
            }
        }
        public Action<string> LogMessage { get; set; }
        public virtual string GetProduceType() => "N/A";
        public virtual string GetProduceSummary() => "None";
        public void IncrementAge()
        {
            Age++;
        }
        public int SetLifespan(int minSpanAge, int maxSpanAge)
        {
            return rand.Next(minSpanAge, maxSpanAge);
        }
        public void AttemptGrowth()
        {
            GrowthStage currentStage = Stage;
            GrowthStage newStage = currentStage;

            GrowthStage? nextStage = currentStage switch
            {
                GrowthStage.Plant => GrowthStage.Young,
                GrowthStage.Young => GrowthStage.Grown,
                GrowthStage.Grown => GrowthStage.Aged,
                GrowthStage.Aged => GrowthStage.Old,
                GrowthStage.Old => GrowthStage.Dead,
                GrowthStage.Dead => GrowthStage.None,
                _ => null
            };

            if (nextStage == null || nextStage == GrowthStage.Produced || nextStage == GrowthStage.None)
            {
                return;
            }

            var thresholds = GetGrowthStageThresholds();

            if (thresholds.TryGetValue(nextStage.Value, out double requiredPercent))
            {
                int requiredAge = (int)Math.Ceiling(Lifespan * requiredPercent);

                if (Age < requiredAge)
                {
                    return;
                }
            }

            switch (currentStage)
            {
                case GrowthStage.Plant:
                    newStage = rand.NextDouble() <= GetGrowthChance(0.5, 0.20) ? GrowthStage.Young : GrowthStage.Plant;
                    break;
                case GrowthStage.Young:
                    newStage = rand.NextDouble() <= GetGrowthChance(0.10, 0.35) ? GrowthStage.Grown : GrowthStage.Young;
                    break;
                case GrowthStage.Grown:
                    newStage = rand.NextDouble() <= GetGrowthChance(0.08, 0.30) ? GrowthStage.Aged : GrowthStage.Grown;
                    break;
                case GrowthStage.Aged:
                    newStage = rand.NextDouble() <= GetGrowthChance(0.05, 0.15) ? GrowthStage.Old : GrowthStage.Aged;
                    break;
                case GrowthStage.Old:
                    newStage = rand.NextDouble() <= GetGrowthChance(0.02, 0.10) ? GrowthStage.Dead : GrowthStage.Old;
                    break;
                case GrowthStage.Dead:
                    newStage = rand.NextDouble() <= GetGrowthChance(0.10, 0.40) ? GrowthStage.None : GrowthStage.Dead;
                    break;
            }

            if (newStage != currentStage && newStage != GrowthStage.Produced)
            {
                Logger?.LogGrowth(Name, currentStage, newStage, Age);
                Stage = newStage;
            }
        }
        private double GetAgeFactor()
        {
            if (Lifespan <= 0) return 0.0;

            double normalized = (double)Age / Lifespan;
            double result = Math.Min(normalized, 1.0);
            return result;
        }
        private double GetGrowthChance(double minChance, double maxChance)
        {
            double ageFactor = GetAgeFactor();
            double result = minChance + (maxChance - minChance) * ageFactor;
            return result;
        }
        public virtual Dictionary<GrowthStage, double> GetGrowthStageThresholds()
        {
            return new Dictionary<GrowthStage, double>
            {
                { GrowthStage.Young, 0.10 },
                { GrowthStage.Grown, 0.30 },
                { GrowthStage.Aged, 0.50 },
                { GrowthStage.Old, 0.75 },
                { GrowthStage.Dead, 0.90 },
            };
        }
    }

    public enum GrowthStage
    {
        Plant,
        Young,
        Grown,
        Aged,
        Old,
        Dead,
        None,
        Produced
    }
}
