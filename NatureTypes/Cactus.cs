namespace WorldSimulator.Models.NatureBase
{
    public class Cactus : ProduceCapableNature
    {
        public Cactus(string name, params ProduceType[] produceTypes)
        {
            Name = name;
            Type = "Cactus";
            Age = 0;
            Lifespan = SetLifespan(10, 200);
            Stage = GrowthStage.Plant;
            SupportedProduceTypes = produceTypes.ToList();
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
