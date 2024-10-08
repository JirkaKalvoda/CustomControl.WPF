﻿using System.Collections.Generic;
using System.Linq;
using CustomControl.Enum;

namespace CustomControl.Model
{
    public class TimeUnitRatio : UnitRatioBase<TimeUnit>
    {
        public static TimeUnitRatio Instance { get; private set; } = new TimeUnitRatio();

        private TimeUnitRatio()
        {
            UnitRatioList = new List<UnitRatioItem<TimeUnit>>();
            UnitRatioList.Add(new UnitRatioItem<TimeUnit>(TimeUnit.Sec, 1d));
            UnitRatioList.Add(new UnitRatioItem<TimeUnit>(TimeUnit.Min, 60d));
            UnitRatioList.Add(new UnitRatioItem<TimeUnit>(TimeUnit.Hour, 3600d));
        }
    }
}
