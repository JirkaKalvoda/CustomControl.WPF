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
    public partial class LengthUnitViewModel : ObservableObject
    {
        [ObservableProperty]
        private LengthUnit realUnit;

        [ObservableProperty]
        private LengthUnit dispUnit;

        [ObservableProperty]
        private double realValue;

        [ObservableProperty]
        private double dispValue;

        [ObservableProperty]
        private List<LengthUnit> realUnitList;
        
        [ObservableProperty]
        private List<LengthUnit> dispUnitList;

        public LengthUnitViewModel()
        {
            RealUnitList = ((LengthUnit[])System.Enum.GetValues(typeof(LengthUnit))).ToList();
            DispUnitList = ((LengthUnit[])System.Enum.GetValues(typeof(LengthUnit))).ToList();
            RealUnit = LengthUnit.Km;
            DispUnit = LengthUnit.M;
            PropertyChanged += ViewModelPropertyChanged;
            RealValue = 10;
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RealValue)
                || e.PropertyName == nameof(DispUnit)
                )
            {
                DispValue = LengthUnitRatio.Instance.ConvertUnit(RealValue, RealUnit, DispUnit);
            }
            if (e.PropertyName == nameof(DispValue)
                || e.PropertyName == nameof(RealUnit)
               )
            {
                RealValue = LengthUnitRatio.Instance.ConvertUnit(DispValue, DispUnit, RealUnit);
            }
        }

        private FrameworkElement element;

        public FrameworkElement GetElement()
        {
            if (element == null)
            {
                element = new LengthUnitControl();
                element.DataContext = this;
            }
            return element;
        }
    }
}
