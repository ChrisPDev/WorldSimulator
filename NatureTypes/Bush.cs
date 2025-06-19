namespace WorldSimulator.Models.NatureBase
{
    public class Bush : ProduceCapableNature
    {
        public Bush(string name, params ProduceType[] produceTypes)
        {
            Name = name;
            Type = "Bush";
            Age = 0;
            Lifespan = SetLifespan(10, 50);
            Stage = GrowthStage.Plant;
            SupportedProduceTypes = produceTypes.ToList();
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
