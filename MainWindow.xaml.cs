using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BinCompare.Models;
using BinCompare.ViewModels;

namespace BinCompare
{
    /// <summary>
    /// 主窗口交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
            
            // 订阅跳转事件
            _viewModel.JumpToDifferenceRequested += ViewModel_JumpToDifferenceRequested;
            
            // 处理每行字节数变更
            CmbBytesPerRow.SelectionChanged += CmbBytesPerRow_SelectionChanged;
        }

        /// <summary>
        /// 处理每行字节数变更
        /// </summary>
        private void CmbBytesPerRow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbBytesPerRow.SelectedItem is ComboBoxItem item && int.TryParse(item.Content.ToString(), out int bytesPerRow))
            {
                _viewModel.BytesPerRow = bytesPerRow;
            }
        }

        /// <summary>
        /// 处理跳转到差异位置
        /// </summary>
        private void ViewModel_JumpToDifferenceRequested(object sender, DifferenceInfo diff)
        {
            try
            {
                if (diff == null)
                    return;

                // 计算行索引
                int rowIndex = (int)(diff.ByteOffset / _viewModel.BytesPerRow);

                // 滚动到对应位置
                if (ListBoxFileA.Items.Count > rowIndex)
                {
                    ListBoxFileA.SelectedIndex = rowIndex;
                    ListBoxFileA.ScrollIntoView(ListBoxFileA.SelectedItem);
                }

                if (ListBoxFileB.Items.Count > rowIndex)
                {
                    ListBoxFileB.SelectedIndex = rowIndex;
                    ListBoxFileB.ScrollIntoView(ListBoxFileB.SelectedItem);
                }

                _viewModel.StatusMessage = $"已跳转到地址: 0x{diff.Address}";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"跳转错误: {ex.Message}";
            }
        }

        /// <summary>
        /// 处理差异地址点击
        /// </summary>
        private void DifferenceAddress_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is DifferenceInfo diff)
            {
                _viewModel.JumpToDifferenceCommand.Execute(diff);
            }
        }

        /// <summary>
        /// 处理窗口键盘事件
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+O: 打开文件A
            if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _viewModel.SelectFileACommand.Execute(null);
                e.Handled = true;
            }
            // Ctrl+Shift+O: 打开文件B
            else if (e.Key == Key.O && Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                _viewModel.SelectFileBCommand.Execute(null);
                e.Handled = true;
            }
            // Ctrl+H: 切换显示模式
            else if (e.Key == Key.H && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _viewModel.ToggleModeCommand.Execute(null);
                e.Handled = true;
            }
            // Ctrl+E: 导出差异
            else if (e.Key == Key.E && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _viewModel.ExportDifferencesCommand.Execute(null);
                e.Handled = true;
            }
            // Delete: 清除所有
            else if (e.Key == Key.Delete)
            {
                _viewModel.ClearAllCommand.Execute(null);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 同步两个ListBox的滚动位置
        /// </summary>
        private void ListBoxFileA_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == ListBoxFileA && ListBoxFileB != null)
            {
                var scrollViewer = GetScrollViewer(ListBoxFileB);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
                }
            }
        }

        private void ListBoxFileB_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == ListBoxFileB && ListBoxFileA != null)
            {
                var scrollViewer = GetScrollViewer(ListBoxFileA);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
                }
            }
        }

        /// <summary>
        /// 获取ListBox的ScrollViewer
        /// </summary>
        private ScrollViewer GetScrollViewer(DependencyObject element)
        {
            if (element == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is ScrollViewer scrollViewer)
                    return scrollViewer;
                
                var result = GetScrollViewer(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// 文件A拖拽进入事件
        /// </summary>
        private void BorderFileA_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                BorderFileA.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 212));
                BorderFileA.BorderThickness = new Thickness(2);
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        /// <summary>
        /// 文件A拖拽离开事件
        /// </summary>
        private void BorderFileA_DragLeave(object sender, DragEventArgs e)
        {
            BorderFileA.BorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204));
            BorderFileA.BorderThickness = new Thickness(1);
            e.Handled = true;
        }

        /// <summary>
        /// 文件A拖拽放下事件
        /// </summary>
        private void BorderFileA_Drop(object sender, DragEventArgs e)
        {
            try
            {
                BorderFileA.BorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204));
                BorderFileA.BorderThickness = new Thickness(1);

                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                        string filePath = files[0];
                        LoadFileA(filePath);
                    }
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"拖拽错误: {ex.Message}";
            }
        }

        /// <summary>
        /// 文件B拖拽进入事件
        /// </summary>
        private void BorderFileB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                BorderFileB.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 124, 16));
                BorderFileB.BorderThickness = new Thickness(2);
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        /// <summary>
        /// 文件B拖拽离开事件
        /// </summary>
        private void BorderFileB_DragLeave(object sender, DragEventArgs e)
        {
            BorderFileB.BorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204));
            BorderFileB.BorderThickness = new Thickness(1);
            e.Handled = true;
        }

        /// <summary>
        /// 文件B拖拽放下事件
        /// </summary>
        private void BorderFileB_Drop(object sender, DragEventArgs e)
        {
            try
            {
                BorderFileB.BorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204));
                BorderFileB.BorderThickness = new Thickness(1);

                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                        string filePath = files[0];
                        LoadFileB(filePath);
                    }
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"拖拽错误: {ex.Message}";
            }
        }

        /// <summary>
        /// 加载文件A
        /// </summary>
        private void LoadFileA(string filePath)
        {
            try
            {
                byte[] fileData = System.IO.File.ReadAllBytes(filePath);
                var fileInfo = new System.IO.FileInfo(filePath);

                _viewModel.FileA = new BinaryFileData
                {
                    FilePath = filePath,
                    Data = fileData,
                    FileSize = fileInfo.Length,
                    FileName = fileInfo.Name
                };
                _viewModel.StatusMessage = $"文件A已加载: {fileInfo.Name} ({fileInfo.Length} 字节)";

                // 如果两个文件都已加载，自动进行对比
                if (_viewModel.FileA.Data.Length > 0 && _viewModel.FileB.Data.Length > 0)
                {
                    _viewModel.PerformComparison();
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"加载文件A失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 加载文件B
        /// </summary>
        private void LoadFileB(string filePath)
        {
            try
            {
                byte[] fileData = System.IO.File.ReadAllBytes(filePath);
                var fileInfo = new System.IO.FileInfo(filePath);

                _viewModel.FileB = new BinaryFileData
                {
                    FilePath = filePath,
                    Data = fileData,
                    FileSize = fileInfo.Length,
                    FileName = fileInfo.Name
                };
                _viewModel.StatusMessage = $"文件B已加载: {fileInfo.Name} ({fileInfo.Length} 字节)";

                // 如果两个文件都已加载，自动进行对比
                if (_viewModel.FileA.Data.Length > 0 && _viewModel.FileB.Data.Length > 0)
                {
                    _viewModel.PerformComparison();
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"加载文件B失败: {ex.Message}";
            }
        }

        /// <summary>
        /// GridSplitter鼠标进入事件
        /// </summary>
        private void GridSplitter_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is GridSplitter splitter)
            {
                splitter.Background = new SolidColorBrush(Color.FromRgb(0, 120, 212));
                splitter.Cursor = Cursors.SizeWE;
            }
        }

        /// <summary>
        /// GridSplitter鼠标离开事件
        /// </summary>
        private void GridSplitter_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is GridSplitter splitter)
            {
                splitter.Background = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                splitter.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// 文件A选择行变更事件
        /// </summary>
        private void ListBoxFileA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListBoxFileA == null || ListBoxFileB == null)
                    return;

                // 获取文件A选中的行索引
                int selectedIndexA = ListBoxFileA.SelectedIndex;
                
                // 同步选中文件B的相同行
                if (selectedIndexA >= 0 && selectedIndexA < ListBoxFileB.Items.Count)
                {
                    ListBoxFileB.SelectedIndex = selectedIndexA;
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"同步选择失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 文件B选择行变更事件
        /// </summary>
        private void ListBoxFileB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListBoxFileA == null || ListBoxFileB == null)
                    return;

                // 获取文件B选中的行索引
                int selectedIndexB = ListBoxFileB.SelectedIndex;
                
                // 同步选中文件A的相同行
                if (selectedIndexB >= 0 && selectedIndexB < ListBoxFileA.Items.Count)
                {
                    ListBoxFileA.SelectedIndex = selectedIndexB;
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"同步选择失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 隐藏/显示差异信息区
        /// </summary>
        private void BtnToggleDifferencePanel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BorderDifferencePanel == null)
                    return;

                // 切换显示/隐藏
                if (BorderDifferencePanel.Visibility == Visibility.Visible)
                {
                    // 隐藏差异信息区
                    BorderDifferencePanel.Visibility = Visibility.Collapsed;
                    BtnToggleDifferencePanel.Content = "显示差异信息";
                    
                    // 修改Grid列宽，腾出空间给文件显示区
                    if (MainContentGrid != null)
                    {
                        MainContentGrid.ColumnDefinitions[1].Width = new GridLength(0);
                    }
                    
                    _viewModel.StatusMessage = "差异信息已隐藏";
                }
                else
                {
                    // 显示差异信息区
                    BorderDifferencePanel.Visibility = Visibility.Visible;
                    BtnToggleDifferencePanel.Content = "隐藏差异信息";
                    
                    // 恢复Grid列宽
                    if (MainContentGrid != null)
                    {
                        MainContentGrid.ColumnDefinitions[1].Width = new GridLength(300);
                    }
                    
                    _viewModel.StatusMessage = "差异信息已显示";
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"切换差异信息显示失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 显示帮助窗口
        /// </summary>
        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpWindow helpWindow = new HelpWindow();
                helpWindow.Owner = this;
                helpWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"打开帮助窗口失败: {ex.Message}";
            }
        }
    }
}