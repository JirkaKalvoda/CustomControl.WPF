using System.Windows;

namespace CustomControl.Control
{
    /// <summary>
    /// 附加属性，暂时没用上
    /// </summary>
    public class RealValueAttachedProperty : DependencyObject
    {
        public static readonly System.Windows.DependencyProperty RealValueProperty = System.Windows.DependencyProperty.RegisterAttached(
            nameof(RealValue), typeof(double), typeof(RealValueAttachedProperty), new PropertyMetadata(default(double)));

        public double RealValue
        {
            get { return (double)GetValue(RealValueProperty); }
            set { SetValue(RealValueProperty, value); }
        }

        public static double GetRealValue(DependencyObject obj)
        {
            return (double)obj.GetValue(RealValueProperty);
        }

        public static void SetRealValue(DependencyObject obj, double value)
        {
            obj.SetValue(RealValueProperty, value);
        }
    }
}
