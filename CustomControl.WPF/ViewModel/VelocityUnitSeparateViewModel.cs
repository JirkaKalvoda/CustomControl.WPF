using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CustomControl.Control;
using CustomControl.Enum;
using CustomControl.Model;

namespace CustomControl.ViewModel
{
    public partial class VelocityUnitSeparateViewModel : ObservableObject
    {
        [ObservableProperty]
        private LengthUnit realUnit;

        [ObservableProperty]
        private LengthUnit dispUnit;

        [ObservableProperty]
        private TimeUnit realUnit2;

        [ObservableProperty]
        private TimeUnit dispUnit2;

        [ObservableProperty]
        private double realValue;

        [ObservableProperty]
        private double dispValue;

        [ObservableProperty]
        private List<LengthUnit> realUnitList;
        
        [ObservableProperty]
        private List<LengthUnit> dispUnitList;

        [ObservableProperty]
        private List<TimeUnit> realUnitList2;

        [ObservableProperty]
        private List<TimeUnit> dispUnitList2;

        public VelocityUnitSeparateViewModel()
        {
            RealUnitList = ((LengthUnit[])System.Enum.GetValues(typeof(LengthUnit))).ToList();
            DispUnitList = ((LengthUnit[])System.Enum.GetValues(typeof(LengthUnit))).ToList();
            RealUnitList2 = ((TimeUnit[])System.Enum.GetValues(typeof(TimeUnit))).ToList();
            DispUnitList2 = ((TimeUnit[])System.Enum.GetValues(typeof(TimeUnit))).ToList();
            RealUnit = LengthUnit.Km;
            DispUnit = LengthUnit.M;
            RealUnit2 = TimeUnit.Sec;
            DispUnit2 = TimeUnit.Sec;
            PropertyChanged += ViewModelPropertyChanged;
            RealValue = 10;
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RealValue)
                || e.PropertyName == nameof(DispUnit)
                || e.PropertyName == nameof(DispUnit2)
                )
            {
                double temp = LengthUnitRatio.Instance.ConvertUnit(RealValue, RealUnit, DispUnit);
                DispValue = TimeUnitRatio.Instance.ConvertUnit(temp, DispUnit2, RealUnit2);
            }
            if (e.PropertyName == nameof(DispValue)
                || e.PropertyName == nameof(RealUnit)
                || e.PropertyName == nameof(RealUnit2)
               )
            {
                double temp = LengthUnitRatio.Instance.ConvertUnit(DispValue, DispUnit, RealUnit);
                RealValue = TimeUnitRatio.Instance.ConvertUnit(temp, RealUnit2, DispUnit2);
            }
        }

        private FrameworkElement element;

        public FrameworkElement GetElement()
        {
            if (element == null)
            {
                element = new VelocityUnitSeparateControl();
                element.DataContext = this;
            }
            return element;
        }
    }
}
