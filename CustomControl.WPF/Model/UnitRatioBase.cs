using System.Collections.Generic;
using System.Linq;

namespace CustomControl.Model
{
    /// <summary>
    /// 存储单位和倍率的基类，派生类应该是单例，因为这种倍率数据是固定的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnitRatioBase<T> where T : System.Enum
    {
        protected List<UnitRatioItem<T>> UnitRatioList { get; set; }

        /// <summary>
        /// 单位换算，适用于数类型
        /// </summary>
        /// <param name="input"></param>
        /// <param name="unitIn">如果是分子 则填输入单位，如果是分母 则填输出单位</param>
        /// <param name="unitOut">如果是分子 则填输出单位，如果是分母 则填输入单位</param>
        /// <returns></returns>
        public double ConvertUnit(double input, T unitIn, T unitOut)
        {
            double ratioIn = UnitRatioList.First(o => o.Unit.Equals(unitIn)).Ratio;
            double ratioOut = UnitRatioList.First(o => o.Unit.Equals(unitOut)).Ratio;
            return input * ratioIn / ratioOut;
        }
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
