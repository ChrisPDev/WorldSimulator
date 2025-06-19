namespace WorldSimulator.Models.NatureBase
{
    public class Grass : Nature
    {
        public Grass(string name)
        {
            Name = name;
            Type = "Grass";
            Age = 0;
            Lifespan = SetLifespan(1, 20);
            Stage = GrowthStage.Plant;
            GrowthHistory.Add((0, GrowthStage.Plant));
        }
    }
}
