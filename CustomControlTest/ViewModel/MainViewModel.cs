using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CustomControlTest.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private Window convertUnitWindow;

        [RelayCommand]
        private void OpenConvertUnitWindow()
        {
            if (convertUnitWindow == null || !convertUnitWindow.IsVisible)
            {
                convertUnitWindow = new ConvertUnitViewModel().GetWindow();
            }
            if (convertUnitWindow.Visibility != Visibility.Visible)
            {
                convertUnitWindow.Show();
            }
            if (!convertUnitWindow.IsActive)
            {
                convertUnitWindow.Activate();
            }
        }
    }
}
