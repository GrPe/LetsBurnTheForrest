using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestFireSimulator.Model
{
    public class Data
    {
        public string FilePath { get; private set; }
        public int SelfInflammationPropability { get; private set; }
        public int InflammationFromNeighborPropability { get; private set; }
        public int RenewTreePropability { get; private set; }

        public Data(string filePath, int selfInflammantionPropability, int inflammantionFromNeighborPropability, int renewTreePropability)
        {
            FilePath = filePath;
            SelfInflammationPropability = selfInflammantionPropability;
            InflammationFromNeighborPropability = inflammantionFromNeighborPropability;
            RenewTreePropability = renewTreePropability;
        }
    }
}
