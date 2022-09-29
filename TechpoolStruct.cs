using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBoard.Types;

namespace TechBoard.Containers
{
    public struct TechpoolStruct
    {
        public readonly double[] EngineTechpool;
        public readonly double[] CarTechpool;

        public TechpoolStruct(double[] engineTechpool, double[] carTechpool)
        {
            EngineTechpool = engineTechpool;
            CarTechpool = carTechpool;
        }
    }
}
