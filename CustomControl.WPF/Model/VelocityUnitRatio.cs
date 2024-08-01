using System.Collections.Generic;
using System.Linq;
using CustomControl.Enum;

namespace CustomControl.Model
{
    public class VelocityUnitRatio : UnitRatioBase<VelocityUnit>
    {
        public static VelocityUnitRatio Instance { get; private set; } = new VelocityUnitRatio();

        private VelocityUnitRatio()
        {
            UnitRatioList = new List<UnitRatioItem<VelocityUnit>>();
            UnitRatioList.Add(new UnitRatioItem<VelocityUnit>(VelocityUnit.Mps, 1d));
            UnitRatioList.Add(new UnitRatioItem<VelocityUnit>(VelocityUnit.Mph, 1d / 3600d));
            UnitRatioList.Add(new UnitRatioItem<VelocityUnit>(VelocityUnit.Kmps, 1000d));
            UnitRatioList.Add(new UnitRatioItem<VelocityUnit>(VelocityUnit.Kmph, 1d / 3.6d));
        }
    }
}
