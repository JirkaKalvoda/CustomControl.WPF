using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CustomControl.ViewModel;

namespace CustomControl.Control
{
    /// <summary>
    /// TreeViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class TreeViewControl : UserControl
    {
        public TreeViewControl()
        {
            InitializeComponent();
        }

        private void TreeViewItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void TreeViewItem_OnCollapsed(object sender, RoutedEventArgs e)
        {
            (DataContext as TreeViewViewModel).RefreshTreeViewLeft();
        }

        private void TreeViewItem_OnExpanded(object sender, RoutedEventArgs e)
        {
            (DataContext as TreeViewViewModel).RefreshTreeViewLeft();
        }
    }
}
