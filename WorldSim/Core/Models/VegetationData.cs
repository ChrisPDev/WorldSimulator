namespace WorldSim.Core.Models
{
    public class VegetationData : Evolvable<VegetationType>
    {
        public VegetationData()
        {
            Type = VegetationType.None;
        }
    }
}
