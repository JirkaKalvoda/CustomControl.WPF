using System.Collections.Generic;

namespace CustomControl.Model
{
    /// <summary>
    /// 存储单位和倍率的基类，派生类应该是单例，因为这种倍率数据是固定的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnitRatioBase<T> where T : System.Enum
    {
        protected List<UnitRatioItem<T>> UnitRatioList { get; set; }
    }

    public class UnitRatioItem<T> where T : System.Enum
    {
        public T Unit{ get; set; }

        public double Ratio { get; set; }

        public UnitRatioItem(T unit, double ratio)
        {
            Unit = unit;
            Ratio = ratio;
        }
    }
}
