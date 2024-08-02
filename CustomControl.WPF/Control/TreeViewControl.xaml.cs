using System.Windows.Controls;
using System.Windows.Input;

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
    }
}
