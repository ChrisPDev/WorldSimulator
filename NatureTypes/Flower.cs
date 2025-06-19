namespace WorldSimulator.Models.NatureBase
{
    public class Flower : Nature
    {
        public Flower(string name)
        {
            Name = name;
            Type = "Flower";
            Age = 0;
            Lifespan = SetLifespan(3, 7);
            Stage = GrowthStage.Plant;
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
