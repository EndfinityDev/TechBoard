using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBoard.Types
{
    public enum EngineTechpoolTypes : byte
    {
        Family,
        BottomEnd,
        TopEnd,
        Aspiration,
        FuelSystem,
        Exhaust,
        Count
    }
    public enum CarTechpoolTypes : byte
    {
        Body,
        Chassis,
        Interior,
        Assists,
        Safety,
        Drivetrain,
        Tyres,
        Brakes,
        Aerodynamics,
        Suspension,
        Count
    }
}
