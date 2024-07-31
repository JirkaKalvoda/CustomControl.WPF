using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CustomControl.ViewModel;
using CustomControlTest.Control;

namespace CustomControlTest.ViewModel
{
    public partial class ConvertUnitViewModel : ObservableObject
    {
        [ObservableProperty]
        private FrameworkElement lengthUnitControl;
        
        [ObservableProperty]
        private FrameworkElement velocityUnitControl;
        
        [ObservableProperty]
        private FrameworkElement velocityUnitSeparateControl;
        
        public ConvertUnitViewModel()
        {
            LengthUnitControl = new LengthUnitViewModel().GetElement();
            VelocityUnitControl = new VelocityUnitViewModel().GetElement();
            VelocityUnitSeparateControl = new VelocityUnitSeparateViewModel().GetElement();
        }

        private Window window;

        public Window GetWindow()
        {
            if (window == null)
            {
                window = new ConvertUnitWindow();
                window.DataContext = this;
            }
            return window;
        }
    }
}
