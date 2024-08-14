using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomControl.Control;

namespace CustomControl.ViewModel
{
    public partial class TreeViewViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<TreeViewItemViewModel> data;

        [ObservableProperty]
        private string searchKeyword;

        [ObservableProperty]
        private Brush selectedForeground;
        
        [ObservableProperty]
        private Brush selectedBackground;

        public TreeViewViewModel()
        {
            TreeViewItemViewModel node1 = new TreeViewItemViewModel()
            {
                Text = "Layer 1 Node 1",
                TextTranslate = "层1节点1",
                Nodes = new List<TreeViewItemViewModel>(),
            };
            TreeViewItemViewModel node2 = new TreeViewItemViewModel()
            {
                Text = "Layer 2 Node 1",
                TextTranslate = "层2节点1",
                Nodes = new List<TreeViewItemViewModel>(),
            };
            TreeViewItemViewModel node3 = new TreeViewItemViewModel()
            {
                Text = "Layer 2 Node 2",
                TextTranslate = "层2节点2",
                Nodes = new List<TreeViewItemViewModel>(),
            };
            TreeViewItemViewModel node4 = new TreeViewItemViewModel()
            {
                Text = "Layer 3 Node 1",
                TextTranslate = "层3节点1",
            };
            TreeViewItemViewModel node5 = new TreeViewItemViewModel()
            {
                Text = "Layer 3 Node 2",
                TextTranslate = "层3节点2",
            };
            TreeViewItemViewModel node6 = new TreeViewItemViewModel()
            {
                Text = "Layer 3 Node 3",
                TextTranslate = "层3节点3",
                Nodes = new List<TreeViewItemViewModel>(),
            };
            TreeViewItemViewModel node7 = new TreeViewItemViewModel()
            {
                Text = "Layer 4 Node 1",
                TextTranslate = "层4节点1",
            };
            TreeViewItemViewModel node8 = new TreeViewItemViewModel()
            {
                Text = "Layer 4 Node 2",
                TextTranslate = "层4节点2",
            };
            TreeViewItemViewModel node9 = new TreeViewItemViewModel()
            {
                Text = "Layer 1 Node 2",
                TextTranslate = "层1节点2",
            };
            node1.Nodes.Add(node2);
            node1.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node3.Nodes.Add(node5);
            node3.Nodes.Add(node6);
            node6.Nodes.Add(node7);
            node6.Nodes.Add(node8);
            node2.Parent = node1;
            node3.Parent = node1;
            node4.Parent = node2;
            node5.Parent = node3;
            node6.Parent = node3;
            node7.Parent = node6;
            node8.Parent = node6;
            Data = new List<TreeViewItemViewModel>()
            {
                node1,
                node9,
            };
            SelectedForeground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            //SelectedBackground = new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x78, 0xd7));
            SelectedBackground = new SolidColorBrush(Color.FromArgb(0xff, 0x55, 0x78, 0x00));
        }

        private FrameworkElement element;

        public FrameworkElement GetElement()
        {
            if (element == null)
            {
                element = new TreeViewControl();
                element.DataContext = this;
            }
            return element;
        }

        /// <summary>
        /// 如果搜索到匹配项，不显示不符合的子项
        /// </summary>
        [RelayCommand]
        private void Search1()
        {
            if (Data != null && Data.Count > 0)
            {
                for (int i = 0; i < Data.Count; ++i)
                {
                    ScanTreeView1(Data[i], MatchTreeViewItem, SearchKeyword);
                }
            }
        }

        /// <summary>
        /// 如果搜索到匹配项，显示所有子项
        /// </summary>
        [RelayCommand]
        private void Search2()
        {
            if (Data != null && Data.Count > 0)
            {
                for (int i = 0; i < Data.Count; ++i)
                {
                    ScanTreeView2(Data[i], MatchTreeViewItem, SearchKeyword);
                }
            }
        }

        /// <summary>
        /// 清除搜索结果，所有项都显示
        /// </summary>
        [RelayCommand]
        private void SearchClear()
        {
            SearchKeyword = "";
            if (Data != null && Data.Count > 0)
            {
                for (int i = 0; i < Data.Count; ++i)
                {
                    ScanTreeViewRecoverVisible(Data[i]);
                }
            }
        }

        /// <summary>
        /// 判断项是否匹配关键词，后续可以扩展成按空白符切成多个关键词
        /// </summary>
        /// <param name="node"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool MatchTreeViewItem(TreeViewItemViewModel node, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || node == null)
            {
                return true;
            }
            if (!string.IsNullOrWhiteSpace(node.Text) && node.Text.ToLower().Contains(keyword.ToLower())
                || !string.IsNullOrWhiteSpace(node.TextTranslate) && node.TextTranslate.ToLower().Contains(keyword.ToLower())
               )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 深度优先遍历树，如果搜索到匹配项，不显示不符合的子项
        /// </summary>
        /// <param name="root"></param>
        /// <param name="match"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool ScanTreeView1(TreeViewItemViewModel root, Func<TreeViewItemViewModel, string, bool> match, string keyword)
        {
            if (root == null)
            {
                return false;
            }
            bool isAllSubMatch = false;
            if (root.Nodes != null && root.Nodes.Count > 0)
            {
                for (int i = 0; i < root.Nodes.Count; ++i)
                {
                    isAllSubMatch |= ScanTreeView1(root.Nodes[i], match, keyword);
                }
            }
            if (isAllSubMatch || match(root, keyword))
            {
                root.Show = Visibility.Visible;
                return true;
            }
            else
            {
                root.Show = Visibility.Collapsed;
                return false;
            }
        }

        /// <summary>
        /// 深度优先遍历树，如果搜索到匹配项，不再匹配子项，显示所有子项
        /// </summary>
        /// <param name="root"></param>
        /// <param name="match"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool ScanTreeView2(TreeViewItemViewModel root, Func<TreeViewItemViewModel, string, bool> match, string keyword)
        {
            if (root == null)
            {
                return false;
            }
            if (match == null || match(root, keyword))
            {
                root.Show = Visibility.Visible;
                if (root.Nodes != null && root.Nodes.Count > 0)
                {
                    for (int i = 0; i < root.Nodes.Count; ++i)
                    {
                        ScanTreeView2(root.Nodes[i], null, null);
                    }
                }
                return true;
            }
            else
            {
                bool isAllSubMatch = false;
                if (root.Nodes != null && root.Nodes.Count > 0)
                {
                    for (int i = 0; i < root.Nodes.Count; ++i)
                    {
                        isAllSubMatch |= ScanTreeView2(root.Nodes[i], match, keyword);
                    }
                }
                if (isAllSubMatch)
                {
                    root.Show = Visibility.Visible;
                    return true;
                }
                else
                {
                    root.Show = Visibility.Collapsed;
                    return false;
                }
            }
        }

        /// <summary>
        /// 深度优先遍历树，所有项都显示
        /// </summary>
        /// <param name="root"></param>
        private void ScanTreeViewRecoverVisible(TreeViewItemViewModel root)
        {
            if (root == null)
            {
                return;
            }
            root.Show = Visibility.Visible;
            if (root.Nodes != null && root.Nodes.Count > 0)
            {
                for (int i = 0; i < root.Nodes.Count; ++i)
                {
                    ScanTreeViewRecoverVisible(root.Nodes[i]);
                }
            }
        }
    }

    public partial class TreeViewItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;
        
        [ObservableProperty]
        private string text;
        
        [ObservableProperty]
        private string textTranslate;
        
        [ObservableProperty]
        private object tag;
        
        [ObservableProperty]
        private ImageSource icon;
        
        [ObservableProperty]
        private bool isChecked1;
        
        [ObservableProperty]
        private bool isChecked2;
        
        //[ObservableProperty]
        //private bool isShowCheck1;
        
        //[ObservableProperty]
        //private bool isShowCheck2;
        
        [ObservableProperty]
        private Visibility showButton;
        
        [ObservableProperty]
        private Visibility show;

        [ObservableProperty]
        private List<TreeViewItemViewModel> nodes;

        [ObservableProperty]
        private TreeViewItemViewModel parent;

        public TreeViewItemViewModel()
        {
            Show = Visibility.Visible;
            ShowButton = Visibility.Visible;
        }
    }
}
