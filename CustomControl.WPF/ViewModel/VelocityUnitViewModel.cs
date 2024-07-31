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
    public partial class VelocityUnitViewModel : ObservableObject
    {
        [ObservableProperty]
        private VelocityUnit realUnit;

        [ObservableProperty]
        private VelocityUnit dispUnit;

        [ObservableProperty]
        private double realValue;

        [ObservableProperty]
        private double dispValue;

        [ObservableProperty]
        private List<VelocityUnit> realUnitList;
        
        [ObservableProperty]
        private List<VelocityUnit> dispUnitList;

        public VelocityUnitViewModel()
        {
            RealUnitList = ((VelocityUnit[])System.Enum.GetValues(typeof(VelocityUnit))).ToList();
            DispUnitList = ((VelocityUnit[])System.Enum.GetValues(typeof(VelocityUnit))).ToList();
            RealUnit = VelocityUnit.Mps;
            DispUnit = VelocityUnit.Kmph;
            PropertyChanged += ViewModelPropertyChanged;
            RealValue = 10;
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RealValue)
                || e.PropertyName == nameof(DispUnit)
                )
            {
                DispValue = VelocityUnitRatio.Instance.ConvertUnit(RealValue, RealUnit, DispUnit);
            }
            if (e.PropertyName == nameof(DispValue)
                || e.PropertyName == nameof(RealUnit)
               )
            {
                RealValue = VelocityUnitRatio.Instance.ConvertUnit(DispValue, DispUnit, RealUnit);
            }
        }

        private FrameworkElement element;

        public FrameworkElement GetElement()
        {
            if (element == null)
            {
                element = new VelocityUnitControl();
                element.DataContext = this;
            }
            return element;
        }
    }
}
