using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CustomControlTest.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private Window convertUnitWindow;

        private Window treeViewWindow;

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

        [RelayCommand]
        private void OpenTreeViewWindow()
        {
            if (treeViewWindow == null || !treeViewWindow.IsVisible)
            {
                treeViewWindow = new TreeViewWindowViewModel().GetWindow();
            }
            if (treeViewWindow.Visibility != Visibility.Visible)
            {
                treeViewWindow.Show();
            }
            if (!treeViewWindow.IsActive)
            {
                treeViewWindow.Activate();
            }
        }
    }
}
