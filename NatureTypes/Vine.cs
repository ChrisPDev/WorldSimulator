namespace WorldSimulator.Models.NatureBase
{
    public class Vine : Nature
    {
        public Vine(string name)
        {
            Name = name;
            Type = "Vine";
            Age = 0;
            Lifespan = SetLifespan(5, 50);
            Stage = GrowthStage.Plant;
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
