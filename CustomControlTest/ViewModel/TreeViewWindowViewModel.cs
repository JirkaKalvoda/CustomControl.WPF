using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CustomControl.ViewModel;
using CustomControlTest.Control;

namespace CustomControlTest.ViewModel
{
    public partial class TreeViewWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private FrameworkElement treeViewControl;

        public TreeViewWindowViewModel()
        {
            TreeViewControl = new TreeViewViewModel().GetElement();
        }

        private Window window;

        public Window GetWindow()
        {
            if (window == null)
            {
                window = new TreeViewWindow();
                window.DataContext = this;
            }
            return window;
        }
    }
}
