using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControl.Control
{
    /// <summary>
    /// 可多选的树状图，废弃<see cref="TreeView.SelectedItem"/>，一律使用<see cref="SelectedItems"/>
    /// <br>TreeViewMultiSelect.xaml 的交互逻辑</br>
    /// </summary>
    public partial class TreeViewMultiSelect : TreeView
    {
        /// <summary>
        /// 把所有节点按顺序放到列表里，用于shift连选和快速查找
        /// </summary>
        private List<TreeViewItem> allTreeViewItems;

        /// <summary>
        /// shift连选的开始节点
        /// </summary>
        private TreeViewItem firstTreeViewItem;

        private List<TreeViewItem> selectedTreeViewItems;

        private IList selectedItems;

        [Bindable(true)]
        [ReadOnly(false)]
        public bool IsMultiSelect
        {
            get
            {
                return (bool)GetValue(IsMultiSelectProperty);
            }
            set
            {
                SetValue(IsMultiSelectProperty, value);
            }
        }

        private List<TreeViewItem> SelectedTreeViewItems
        {
            get
            {
                return selectedTreeViewItems;
            }
            set
            {
                if (!ListEx.CompareIListOmitOrder(selectedTreeViewItems, value))
                {
                    selectedTreeViewItems = value;
                    PaintTreeViewItems();
                    List<object> selectedItemsTemp = value.Select(o => o.Header).ToList();
                    SelectedItems = new List<object>(selectedItemsTemp);
                }
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public IList SelectedItems
        {
            get
            {
                selectedItems = (IList)GetValue(SelectedItemsProperty);
                return selectedItems;
            }
            set
            {
                if (!ListEx.CompareIListOmitOrder(selectedItems, value))
                {
                    selectedItems = value;
                    SetValue(SelectedItemsProperty, selectedItems);
                    List<TreeViewItem> selectedTreeViewItemsTemp = allTreeViewItems.Where(o => selectedItems.Contains(o.Header)).ToList();
                    SelectedTreeViewItems = new List<TreeViewItem>(selectedTreeViewItemsTemp);
                }
            }
        }

#if NESTEDDEPENDENCY
        [Bindable(true)]
        [ReadOnly(false)]
        public TreeViewItemStyle DefaultItemStyle
        {
            get
            {
                return (TreeViewItemStyle)GetValue(DefaultItemStyleProperty);
            }
            set
            {
                SetValue(DefaultItemStyleProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public TreeViewItemStyle SelectedItemStyle
        {
            get
            {
                return (TreeViewItemStyle)GetValue(SelectedItemStyleProperty);
            }
            set
            {
                SetValue(SelectedItemStyleProperty, value);
            }
        }
#else
        [Bindable(true)]
        [ReadOnly(false)]
        public Brush DefaultItemForeground
        {
            get
            {
                return (Brush)GetValue(DefaultItemForegroundProperty);
            }
            set
            {
                SetValue(DefaultItemForegroundProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Brush DefaultItemBackground
        {
            get
            {
                return (Brush)GetValue(DefaultItemBackgroundProperty);
            }
            set
            {
                SetValue(DefaultItemBackgroundProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Brush DefaultItemBorderBrush
        {
            get
            {
                return (Brush)GetValue(DefaultItemBorderBrushProperty);
            }
            set
            {
                SetValue(DefaultItemBorderBrushProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Thickness DefaultItemBorderThickness
        {
            get
            {
                return (Thickness)GetValue(DefaultItemBorderThicknessProperty);
            }
            set
            {
                SetValue(DefaultItemBorderThicknessProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Brush SelectedItemForeground
        {
            get
            {
                return (Brush)GetValue(SelectedItemForegroundProperty);
            }
            set
            {
                SetValue(SelectedItemForegroundProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Brush SelectedItemBackground
        {
            get
            {
                return (Brush)GetValue(SelectedItemBackgroundProperty);
            }
            set
            {
                SetValue(SelectedItemBackgroundProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Brush SelectedItemBorderBrush
        {
            get
            {
                return (Brush)GetValue(SelectedItemBorderBrushProperty);
            }
            set
            {
                SetValue(SelectedItemBorderBrushProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Thickness SelectedItemBorderThickness
        {
            get
            {
                return (Thickness)GetValue(SelectedItemBorderThicknessProperty);
            }
            set
            {
                SetValue(SelectedItemBorderThicknessProperty, value);
            }
        }
#endif

        public static readonly DependencyProperty IsMultiSelectProperty;
        public static readonly DependencyProperty SelectedItemsProperty;
#if NESTEDDEPENDENCY
        public static readonly DependencyProperty DefaultItemStyleProperty;
        public static readonly DependencyProperty SelectedItemStyleProperty;
#else
        public static readonly DependencyProperty DefaultItemForegroundProperty;
        public static readonly DependencyProperty DefaultItemBackgroundProperty;
        public static readonly DependencyProperty DefaultItemBorderBrushProperty;
        public static readonly DependencyProperty DefaultItemBorderThicknessProperty;
        public static readonly DependencyProperty SelectedItemForegroundProperty;
        public static readonly DependencyProperty SelectedItemBackgroundProperty;
        public static readonly DependencyProperty SelectedItemBorderBrushProperty;
        public static readonly DependencyProperty SelectedItemBorderThicknessProperty;
#endif
        public TreeViewMultiSelect()
        {
            InitializeComponent();
            this.AddHandler(TreeViewItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown), true);
            this.AddHandler(TreeViewItem.MouseRightButtonDownEvent, new MouseButtonEventHandler(OnMouseRightButtonDown), true);
            //this.AddHandler(TreeView.MouseDownEvent, new MouseButtonEventHandler(OnTreeViewMouseDown), false);
            //this.AddHandler(TreeView.PreviewMouseDownEvent, new MouseButtonEventHandler(OnTreeViewMouseDown), true);
            this.Loaded += TreeViewMultiSelect_Loaded;
        }

        // 注册依赖属性等
        static TreeViewMultiSelect()
        {
            IsMultiSelectProperty = DependencyProperty.Register(nameof(IsMultiSelect), typeof(bool), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(false));
            SelectedItemsProperty = DependencyProperty.Register(nameof(SelectedItems), typeof(IList), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(null));
#if NESTEDDEPENDENCY
            // 属性的默认值无法绑定到特定线程
            //DefaultItemStyleProperty = DependencyProperty.Register(nameof(DefaultItemStyle), typeof(TreeViewItemStyle), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(
            //    new TreeViewItemStyle()
            //    {
            //        Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00)),
            //        Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff)),
            //        BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00)),
            //        BorderThickness = new Thickness(0),
            //    }));
            //SelectedItemStyleProperty = DependencyProperty.Register(nameof(SelectedItemStyle), typeof(TreeViewItemStyle), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(
            //    new TreeViewItemStyle()
            //    {
            //        Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff)),
            //        Background = new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x78, 0xd7)),
            //        BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00)),
            //        BorderThickness = new Thickness(0),
            //    }));
            DefaultItemStyleProperty = DependencyProperty.Register(nameof(DefaultItemStyle), typeof(TreeViewItemStyle), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(null));
            SelectedItemStyleProperty = DependencyProperty.Register(nameof(SelectedItemStyle), typeof(TreeViewItemStyle), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(null));
#else
            DefaultItemForegroundProperty = DependencyProperty.Register(nameof(DefaultItemForeground), typeof(Brush), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00))));
            DefaultItemBackgroundProperty = DependencyProperty.Register(nameof(DefaultItemBackground), typeof(Brush), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff))));
            DefaultItemBorderBrushProperty = DependencyProperty.Register(nameof(DefaultItemBorderBrush), typeof(Brush), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00))));
            DefaultItemBorderThicknessProperty = DependencyProperty.Register(nameof(DefaultItemBorderThickness), typeof(Thickness), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(new Thickness(0)));
            SelectedItemForegroundProperty = DependencyProperty.Register(nameof(SelectedItemForeground), typeof(Brush), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff))));
            SelectedItemBackgroundProperty = DependencyProperty.Register(nameof(SelectedItemBackground), typeof(Brush), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x78, 0xd7))));
            SelectedItemBorderBrushProperty = DependencyProperty.Register(nameof(SelectedItemBorderBrush), typeof(Brush), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00))));
            SelectedItemBorderThicknessProperty = DependencyProperty.Register(nameof(SelectedItemBorderThickness), typeof(Thickness), typeof(TreeViewMultiSelect), new FrameworkPropertyMetadata(new Thickness(0)));
#endif
        }


        // 顺序：TreeView.PreviewMouseDown、TreeViewItem.MouseDown、TreeView.MouseDown，阻止了前面事件就不执行后面，所以目前先在点击TreeViewItem里判断点的是不是TreeViewItem
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (item == null)
            {
                return;
            }
            if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == ModifierKeys.None)
            {
                SelectedTreeViewItems = new List<TreeViewItem>()
                {
                    item,
                };
                firstTreeViewItem = item;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && IsMultiSelect)
            {
                // 按快捷键选第1个和单选一样
                if (SelectedTreeViewItems == null || SelectedTreeViewItems.Count == 0)
                {
                    SelectedTreeViewItems = new List<TreeViewItem>()
                    {
                        item,
                    };
                    firstTreeViewItem = item;
                }
                else
                {
                    List<TreeViewItem> tempItems = new List<TreeViewItem>(SelectedTreeViewItems);
                    if (tempItems.Contains(item))
                    {
                        tempItems.Remove(item);
                        item.IsSelected = false;  // 按control点击可以取消选择，普通点击无法取消选择，所以手动设置不被选
                    }
                    else
                    {
                        tempItems.Add(item);
                    }
                    SelectedTreeViewItems = new List<TreeViewItem>(tempItems);
                    firstTreeViewItem = item;    // 按control无论选还是不选都是shift连选的开始，和win10一致
                }
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift && IsMultiSelect)
            {
                // 按快捷键选第1个和单选一样
                if (SelectedTreeViewItems == null || SelectedTreeViewItems.Count == 0)
                {
                    SelectedTreeViewItems = new List<TreeViewItem>()
                    {
                        item,
                    };
                    firstTreeViewItem = item;
                }
                else
                {
                    List<TreeViewItem> tempItems = new List<TreeViewItem>();
                    int start = allTreeViewItems.IndexOf(firstTreeViewItem);
                    int stop = allTreeViewItems.IndexOf(item);
                    if (start > stop)
                    {
                        int temp = start;
                        start = stop;
                        stop = temp;
                    }
                    for (int i = start; i <= stop; ++i)
                    {
                        tempItems.Add(allTreeViewItems[i]);
                    }
                    SelectedTreeViewItems = new List<TreeViewItem>(tempItems);
                }
            }
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (item == null)
            {
                return;
            }
            if (SelectedTreeViewItems == null || !SelectedTreeViewItems.Contains(item))
            {
                SelectedTreeViewItems = new List<TreeViewItem>()
                {
                    item,
                };
                firstTreeViewItem = item;
                item.IsSelected = true;
            }
        }

        //private void OnTreeViewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    e.Handled = true;
        //}
        
        /// <summary>
        /// 当节点变化时把树节点平铺存到列表里
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            PutTreeViewItemsInList();
        }

        /// <summary>
        /// 当数据源变化时把树节点平铺存到列表里
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            PutTreeViewItemsInList();
        }

        /// <summary>
        /// UI加载后把树节点平铺存到列表里，否则获取不到节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewMultiSelect_Loaded(object sender, RoutedEventArgs e)
        {
            PutTreeViewItemsInList();
        }

        /// <summary>
        /// 把树节点平铺存到列表里，在对象变化后使用，但是在UI加载完后才有效
        /// </summary>
        private void PutTreeViewItemsInList()
        {
            allTreeViewItems = new List<TreeViewItem>();
            ScanTreeViewItems(this);
        }

        /// <summary>
        /// 遍历每个节点下的节点
        /// </summary>
        /// <param name="node">TreeView或者TreeViewItem</param>
        private void ScanTreeViewItems(ItemsControl node)
        {
            if (node != null && node.Items != null && node.Items.Count > 0)
            {
                foreach (object obj in node.Items)
                {
                    TreeViewItem item = node.ItemContainerGenerator.ContainerFromItem(obj) as TreeViewItem;
                    if (item != null)
                    {
                        allTreeViewItems.Add(item);
                        ScanTreeViewItems(item);
                    }
                }
            }
        }

        /// <summary>
        /// 重新给树节点使用风格
        /// </summary>
        private void PaintTreeViewItems()
        {
            for (int i = 0; i < allTreeViewItems.Count; ++i)
            {
                if (selectedTreeViewItems != null && selectedTreeViewItems.Count > 0 && selectedTreeViewItems.Contains(allTreeViewItems[i]))
                {
#if NESTEDDEPENDENCY
                    allTreeViewItems[i].Foreground = SelectedItemStyle.Foreground;
                    allTreeViewItems[i].Background = SelectedItemStyle.Background;
                    allTreeViewItems[i].BorderBrush = SelectedItemStyle.BorderBrush;
                    allTreeViewItems[i].BorderThickness = SelectedItemStyle.BorderThickness;
#else
                    allTreeViewItems[i].Foreground = SelectedItemForeground;
                    allTreeViewItems[i].Background = SelectedItemBackground;
                    allTreeViewItems[i].BorderBrush = SelectedItemBorderBrush;
                    allTreeViewItems[i].BorderThickness = SelectedItemBorderThickness;
#endif
                }
                else
                {
#if NESTEDDEPENDENCY
                    allTreeViewItems[i].Foreground = DefaultItemStyle.Foreground;
                    allTreeViewItems[i].Background = DefaultItemStyle.Background;
                    allTreeViewItems[i].BorderBrush = DefaultItemStyle.BorderBrush;
                    allTreeViewItems[i].BorderThickness = DefaultItemStyle.BorderThickness;
#else
                    allTreeViewItems[i].Foreground = DefaultItemForeground;
                    allTreeViewItems[i].Background = DefaultItemBackground;
                    allTreeViewItems[i].BorderBrush = DefaultItemBorderBrush;
                    allTreeViewItems[i].BorderThickness = DefaultItemBorderThickness;
#endif
                }
            }
        }

        private DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                source = VisualTreeHelper.GetParent(source);
            }
            return source;
        }
    }


#if NESTEDDEPENDENCY
    /// <summary>
    /// 树状图节点的所有风格
    /// <br>本来想直接取shift连选的开始节点的风格，但是原有的IsSelected设置风格应该是在鼠标点击事件之后，所以会覆盖自己设置的风格，并且没有完全覆盖，调试时发现前景背景色都是白色</br>
    /// <br>所以创建依赖对象类，也方便外部设置</br>
    /// <br>但是依赖属性里套依赖属性绑定不传值，但是传字符串正常</br>
    /// </summary>
    public class TreeViewItemStyle : DependencyObject //FrameworkElement
    {
        [Bindable(true)]
        [ReadOnly(false)]
        public Brush Foreground
        {
            get
            {
                return (Brush)GetValue(ForegroundProperty);
            }
            set
            {
                SetValue(ForegroundProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Brush Background
        {
            get
            {
                return (Brush)GetValue(BackgroundProperty);
            }
            set
            {
                SetValue(BackgroundProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Brush BorderBrush
        {
            get
            {
                return (Brush)GetValue(BorderBrushProperty);
            }
            set
            {
                SetValue(BorderBrushProperty, value);
            }
        }

        [Bindable(true)]
        [ReadOnly(false)]
        public Thickness BorderThickness
        {
            get
            {
                return (Thickness)GetValue(BorderThicknessProperty);
            }
            set
            {
                SetValue(BorderThicknessProperty, value);
            }
        }

        public static readonly DependencyProperty ForegroundProperty;
        public static readonly DependencyProperty BackgroundProperty;
        public static readonly DependencyProperty BorderBrushProperty;
        public static readonly DependencyProperty BorderThicknessProperty;

        public TreeViewItemStyle()
        {
        }

        static TreeViewItemStyle()
        {
            ForegroundProperty = DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(TreeViewItemStyle), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff)), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
            BackgroundProperty = DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(TreeViewItemStyle), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x78, 0xd7)), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
            BorderBrushProperty = DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(TreeViewItemStyle), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00)), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
            //ForegroundProperty = DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(TreeViewItemStyle), new FrameworkPropertyMetadata(null));
            //BackgroundProperty = DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(TreeViewItemStyle), new FrameworkPropertyMetadata(null));
            //BorderBrushProperty = DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(TreeViewItemStyle), new FrameworkPropertyMetadata(null));
            BorderThicknessProperty = DependencyProperty.Register(nameof(BorderThickness), typeof(Thickness), typeof(TreeViewItemStyle), new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        }
    }
#endif
}
