﻿using System.Collections.Generic;
using System.Linq;
using CustomControl.Enum;

namespace CustomControl.Model
{
    public class LengthUnitRatio : UnitRatioBase<LengthUnit>
    {
        public static LengthUnitRatio Instance { get; private set; } = new LengthUnitRatio();

        private LengthUnitRatio()
        {
            UnitRatioList = new List<UnitRatioItem<LengthUnit>>();
            UnitRatioList.Add(new UnitRatioItem<LengthUnit>(LengthUnit.M, 1d));
            UnitRatioList.Add(new UnitRatioItem<LengthUnit>(LengthUnit.Km, 1000d));
            UnitRatioList.Add(new UnitRatioItem<LengthUnit>(LengthUnit.Mile, 1609.344d));
        }

        /// <summary>
        /// 单位换算
        /// </summary>
        /// <param name="input"></param>
        /// <param name="unitIn"></param>
        /// <param name="unitOut"></param>
        /// <returns></returns>
        public double ConvertUnit(double input, LengthUnit unitIn, LengthUnit unitOut)
        {
            double ratioIn = UnitRatioList.First(o => o.Unit == unitIn).Ratio;
            double ratioOut = UnitRatioList.First(o => o.Unit == unitOut).Ratio;
            return input * ratioIn / ratioOut;
        }
    }
}
