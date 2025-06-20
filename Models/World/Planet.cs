using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSimulator.Models.World
{
    public class Planet
    {
        public string Name { get; set; }
        public WorldMap WorldMap { get; set; }

        public int TotalCells
        {
            get
            {
                return WorldMap != null ? WorldMap.MapWidth * WorldMap.MapHeight : 0;
            }
        }
        public Planet(string name, WorldMap worldMap)
        {
            Name = name;
            WorldMap = worldMap;
        }
        public override string ToString()
        {
            return $"{Name} - Total Cells: {TotalCells}";
        }
    }
}
