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
        private System.Timers.Timer _highlightTimer;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
            
            // 订阅跳转事件
            _viewModel.JumpToDifferenceRequested += ViewModel_JumpToDifferenceRequested;
            
            // 处理每行字节数变更
            CmbBytesPerRow.SelectionChanged += CmbBytesPerRow_SelectionChanged;
            
            // 初始化高亮计时器
            _highlightTimer = new System.Timers.Timer(2000); // 2秒后自动取消高亮
            _highlightTimer.AutoReset = false;
            _highlightTimer.Elapsed += HighlightTimer_Elapsed;
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

                // 计算行索引和字节在行中的位置
                int rowIndex = (int)(diff.ByteOffset / _viewModel.BytesPerRow);
                int byteIndexInRow = (int)(diff.ByteOffset % _viewModel.BytesPerRow);

                // 清除之前的高亮
                ClearPreviousHighlight();

                // 滚动到对应位置并高亮字节
                if (ListBoxFileA.Items.Count > rowIndex)
                {
                    ListBoxFileA.SelectedIndex = rowIndex;
                    ListBoxFileA.ScrollIntoView(ListBoxFileA.SelectedItem);
                    
                    // 高亮对应的字节
                    if (_viewModel.DataRowsA.Count > rowIndex)
                    {
                        var row = _viewModel.DataRowsA[rowIndex];
                        if (byteIndexInRow < row.ByteSegments.Count)
                        {
                            row.ByteSegments[byteIndexInRow].IsHighlighted = true;
                        }
                    }
                }

                if (ListBoxFileB.Items.Count > rowIndex)
                {
                    ListBoxFileB.SelectedIndex = rowIndex;
                    ListBoxFileB.ScrollIntoView(ListBoxFileB.SelectedItem);
                    
                    // 高亮对应的字节
                    if (_viewModel.DataRowsB.Count > rowIndex)
                    {
                        var row = _viewModel.DataRowsB[rowIndex];
                        if (byteIndexInRow < row.ByteSegments.Count)
                        {
                            row.ByteSegments[byteIndexInRow].IsHighlighted = true;
                        }
                    }
                }

                _viewModel.StatusMessage = $"已跳转到地址: 0x{diff.Address}";
                
                // 启动计时器，2秒后自动取消高亮
                _highlightTimer.Stop();
                _highlightTimer.Start();
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"跳转错误: {ex.Message}";
            }
        }

        /// <summary>
        /// 清除之前的高亮
        /// </summary>
        private void ClearPreviousHighlight()
        {
            foreach (var row in _viewModel.DataRowsA)
            {
                foreach (var segment in row.ByteSegments)
                {
                    segment.IsHighlighted = false;
                }
            }

            foreach (var row in _viewModel.DataRowsB)
            {
                foreach (var segment in row.ByteSegments)
                {
                    segment.IsHighlighted = false;
                }
            }
        }

        /// <summary>
        /// 高亮计时器事件处理
        /// </summary>
        private void HighlightTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ClearPreviousHighlight();
            });
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
        /// 文件A键盘事件处理（支持Shift+数字多选）
        /// </summary>
        private void ListBoxFileA_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Shift+点击已在XAML中通过SelectionMode="Extended"自动支持
                // 这里可以添加其他快捷键处理
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"键盘事件处理失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 文件A鼠标左键按下事件（支持Ctrl+点击多选）
        /// </summary>
        private void ListBoxFileA_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // 如果按住Ctrl键，则保持多选模式
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    // Ctrl+点击时保持多选
                    e.Handled = false;
                }
                // 否则使用默认的单选行为
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"鼠标事件处理失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 文件A预览右键按下事件（禁用默认右键选择行为）
        /// </summary>
        private void ListBoxFileA_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 禁用右键的默认选择行为
            e.Handled = true;
        }

        /// <summary>
        /// 文件A右键点击事件
        /// </summary>
        private void ListBoxFileA_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 右键菜单会自动显示，这里可以添加额外处理
        }

        /// <summary>
        /// 复制已选数据（文件A）
        /// </summary>
        private void MenuItemCopyData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxFileA.SelectedItems.Count == 0)
                {
                    _viewModel.StatusMessage = "请先选择数据行";
                    return;
                }

                var selectedRows = ListBoxFileA.SelectedItems.Cast<DataRow>().ToList();
                var sb = new System.Text.StringBuilder();

                foreach (var row in selectedRows)
                {
                    // 获取所有字节的十六进制值
                    var hexValues = row.ByteSegments.Select(b => b.Text).ToList();
                    sb.AppendLine(string.Join(" ", hexValues));
                }

                System.Windows.Clipboard.SetText(sb.ToString());
                _viewModel.StatusMessage = $"已复制 {selectedRows.Count} 行数据到剪贴板";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"复制数据失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 复制位置信息（文件A）
        /// </summary>
        private void MenuItemCopyLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxFileA.SelectedItems.Count == 0)
                {
                    _viewModel.StatusMessage = "请先选择数据行";
                    return;
                }

                var selectedRows = ListBoxFileA.SelectedItems.Cast<DataRow>().ToList();
                var sb = new System.Text.StringBuilder();

                foreach (var row in selectedRows)
                {
                    // 格式：起始地址-结束地址:数据
                    // 例如：0000-000F:00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
                    string address = row.Address; // 已经是十六进制格式
                    var hexValues = row.ByteSegments.Select(b => b.Text).ToList();
                    string dataStr = string.Join(" ", hexValues);
                    
                    // 计算结束地址
                    if (int.TryParse(address, System.Globalization.NumberStyles.HexNumber, null, out int startAddr))
                    {
                        int endAddr = startAddr + row.ByteSegments.Count - 1;
                        string locationInfo = $"{startAddr:X4}-{endAddr:X4}:{dataStr}";
                        sb.AppendLine(locationInfo);
                    }
                }

                System.Windows.Clipboard.SetText(sb.ToString());
                _viewModel.StatusMessage = $"已复制 {selectedRows.Count} 行位置信息到剪贴板";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"复制位置信息失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 编辑已选数据（文件A）
        /// </summary>
        private void MenuItemEditData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxFileA.SelectedItems.Count == 0)
                {
                    _viewModel.StatusMessage = "请先选择数据行";
                    return;
                }

                var selectedRows = ListBoxFileA.SelectedItems.Cast<DataRow>().ToList();

                // 检查是否为连续行或单行
                if (!IsContiguousSelection(selectedRows))
                {
                    _viewModel.StatusMessage = "编辑功能仅支持连续行或单行";
                    return;
                }

                // 打开编辑窗口
                DataEditWindow editWindow = new DataEditWindow();
                editWindow.Owner = this;
                editWindow.Initialize(selectedRows);

                if (editWindow.ShowDialog() == true && editWindow.IsConfirmed)
                {
                    // 获取编辑后的数据
                    byte[] editedData = editWindow.EditedData;

                    // 获取第一行的地址
                    if (!int.TryParse(selectedRows.First().Address, System.Globalization.NumberStyles.HexNumber, null, out int startAddr))
                    {
                        _viewModel.StatusMessage = "地址解析失败";
                        return;
                    }

                    // 创建新的文件A数据
                    byte[] newFileA = new byte[_viewModel.FileA.Data.Length];
                    Array.Copy(_viewModel.FileA.Data, newFileA, _viewModel.FileA.Data.Length);

                    // 将编辑后的数据写入文件A
                    Array.Copy(editedData, 0, newFileA, startAddr, editedData.Length);

                    // 更新文件A
                    _viewModel.FileA.Data = newFileA;

                    // 标记文件A已修改
                    _viewModel.IsFileAModified = true;

                    // 重新进行对比
                    _viewModel.PerformComparison();
                    _viewModel.StatusMessage = $"已编辑 {selectedRows.Count} 行数据";
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"编辑数据失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 文件B键盘事件处理（支持Shift+数字多选）
        /// </summary>
        private void ListBoxFileB_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Shift+点击已在XAML中通过SelectionMode="Extended"自动支持
                // 这里可以添加其他快捷键处理
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"键盘事件处理失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 文件B鼠标左键按下事件（支持Ctrl+点击多选）
        /// </summary>
        private void ListBoxFileB_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // 如果按住Ctrl键，则保持多选模式
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    // Ctrl+点击时保持多选
                    e.Handled = false;
                }
                // 否则使用默认的单选行为
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"鼠标事件处理失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 文件B预览右键按下事件（禁用默认右键选择行为）
        /// </summary>
        private void ListBoxFileB_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 禁用右键的默认选择行为
            e.Handled = true;
        }

        /// <summary>
        /// 文件B右键点击事件
        /// </summary>
        private void ListBoxFileB_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 右键菜单会自动显示，这里可以添加额外处理
        }

        /// <summary>
        /// 复制已选数据（文件B）
        /// </summary>
        private void MenuItemCopyDataB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxFileB.SelectedItems.Count == 0)
                {
                    _viewModel.StatusMessage = "请先选择数据行";
                    return;
                }

                var selectedRows = ListBoxFileB.SelectedItems.Cast<DataRow>().ToList();
                var sb = new System.Text.StringBuilder();

                foreach (var row in selectedRows)
                {
                    // 获取所有字节的十六进制值
                    var hexValues = row.ByteSegments.Select(b => b.Text).ToList();
                    sb.AppendLine(string.Join(" ", hexValues));
                }

                System.Windows.Clipboard.SetText(sb.ToString());
                _viewModel.StatusMessage = $"已复制 {selectedRows.Count} 行数据到剪贴板";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"复制数据失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 复制位置信息（文件B）
        /// </summary>
        private void MenuItemCopyLocationB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxFileB.SelectedItems.Count == 0)
                {
                    _viewModel.StatusMessage = "请先选择数据行";
                    return;
                }

                var selectedRows = ListBoxFileB.SelectedItems.Cast<DataRow>().ToList();
                var sb = new System.Text.StringBuilder();

                foreach (var row in selectedRows)
                {
                    // 格式：起始地址-结束地址:数据
                    // 例如：0000-000F:00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
                    string address = row.Address; // 已经是十六进制格式
                    var hexValues = row.ByteSegments.Select(b => b.Text).ToList();
                    string dataStr = string.Join(" ", hexValues);
                    
                    // 计算结束地址
                    if (int.TryParse(address, System.Globalization.NumberStyles.HexNumber, null, out int startAddr))
                    {
                        int endAddr = startAddr + row.ByteSegments.Count - 1;
                        string locationInfo = $"{startAddr:X4}-{endAddr:X4}:{dataStr}";
                        sb.AppendLine(locationInfo);
                    }
                }

                System.Windows.Clipboard.SetText(sb.ToString());
                _viewModel.StatusMessage = $"已复制 {selectedRows.Count} 行位置信息到剪贴板";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"复制位置信息失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 编辑已选数据（文件B）
        /// </summary>
        private void MenuItemEditDataB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxFileB.SelectedItems.Count == 0)
                {
                    _viewModel.StatusMessage = "请先选择数据行";
                    return;
                }

                var selectedRows = ListBoxFileB.SelectedItems.Cast<DataRow>().ToList();

                // 检查是否为连续行或单行
                if (!IsContiguousSelection(selectedRows))
                {
                    _viewModel.StatusMessage = "编辑功能仅支持连续行或单行";
                    return;
                }

                // 打开编辑窗口
                DataEditWindow editWindow = new DataEditWindow();
                editWindow.Owner = this;
                editWindow.Initialize(selectedRows);

                if (editWindow.ShowDialog() == true && editWindow.IsConfirmed)
                {
                    // 获取编辑后的数据
                    byte[] editedData = editWindow.EditedData;

                    // 获取第一行的地址
                    if (!int.TryParse(selectedRows.First().Address, System.Globalization.NumberStyles.HexNumber, null, out int startAddr))
                    {
                        _viewModel.StatusMessage = "地址解析失败";
                        return;
                    }

                    // 创建新的文件B数据
                    byte[] newFileB = new byte[_viewModel.FileB.Data.Length];
                    Array.Copy(_viewModel.FileB.Data, newFileB, _viewModel.FileB.Data.Length);

                    // 将编辑后的数据写入文件B
                    Array.Copy(editedData, 0, newFileB, startAddr, editedData.Length);

                    // 更新文件B
                    _viewModel.FileB.Data = newFileB;

                    // 标记文件B已修改
                    _viewModel.IsFileBModified = true;

                    // 重新进行对比
                    _viewModel.PerformComparison();
                    _viewModel.StatusMessage = $"已编辑 {selectedRows.Count} 行数据";
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"编辑数据失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 保存文件A
        /// </summary>
        private void BtnSaveFileA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.FileA == null || string.IsNullOrEmpty(_viewModel.FileA.FilePath))
                {
                    _viewModel.StatusMessage = "文件A未加载或路径不存在";
                    return;
                }

                System.IO.File.WriteAllBytes(_viewModel.FileA.FilePath, _viewModel.FileA.Data);
                _viewModel.IsFileAModified = false;
                _viewModel.StatusMessage = $"文件A已保存: {_viewModel.FileA.FileName}";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"保存文件A失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 另存为文件A
        /// </summary>
        private void BtnSaveAsFileA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.FileA == null || _viewModel.FileA.Data.Length == 0)
                {
                    _viewModel.StatusMessage = "文件A为空";
                    return;
                }

                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "二进制文件 (*.bin)|*.bin|所有文件 (*.*)|*.*",
                    FileName = _viewModel.FileA.FileName
                };

                if (dialog.ShowDialog() == true)
                {
                    System.IO.File.WriteAllBytes(dialog.FileName, _viewModel.FileA.Data);
                    _viewModel.IsFileAModified = false;
                    _viewModel.StatusMessage = $"文件A已另存为: {System.IO.Path.GetFileName(dialog.FileName)}";
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"另存为文件A失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 还原文件A
        /// </summary>
        private void BtnRevertFileA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.FileA == null || string.IsNullOrEmpty(_viewModel.FileA.FilePath))
                {
                    _viewModel.StatusMessage = "文件A未加载或路径不存在";
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    "确定要还原文件A吗？所有修改将被丢弃。",
                    "确认还原",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    byte[] fileData = System.IO.File.ReadAllBytes(_viewModel.FileA.FilePath);
                    _viewModel.FileA.Data = fileData;
                    _viewModel.IsFileAModified = false;
                    _viewModel.PerformComparison();
                    _viewModel.StatusMessage = $"文件A已还原";
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"还原文件A失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 保存文件B
        /// </summary>
        private void BtnSaveFileB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.FileB == null || string.IsNullOrEmpty(_viewModel.FileB.FilePath))
                {
                    _viewModel.StatusMessage = "文件B未加载或路径不存在";
                    return;
                }

                System.IO.File.WriteAllBytes(_viewModel.FileB.FilePath, _viewModel.FileB.Data);
                _viewModel.IsFileBModified = false;
                _viewModel.StatusMessage = $"文件B已保存: {_viewModel.FileB.FileName}";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"保存文件B失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 另存为文件B
        /// </summary>
        private void BtnSaveAsFileB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.FileB == null || _viewModel.FileB.Data.Length == 0)
                {
                    _viewModel.StatusMessage = "文件B为空";
                    return;
                }

                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "二进制文件 (*.bin)|*.bin|所有文件 (*.*)|*.*",
                    FileName = _viewModel.FileB.FileName
                };

                if (dialog.ShowDialog() == true)
                {
                    System.IO.File.WriteAllBytes(dialog.FileName, _viewModel.FileB.Data);
                    _viewModel.IsFileBModified = false;
                    _viewModel.StatusMessage = $"文件B已另存为: {System.IO.Path.GetFileName(dialog.FileName)}";
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"另存为文件B失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 还原文件B
        /// </summary>
        private void BtnRevertFileB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.FileB == null || string.IsNullOrEmpty(_viewModel.FileB.FilePath))
                {
                    _viewModel.StatusMessage = "文件B未加载或路径不存在";
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    "确定要还原文件B吗？所有修改将被丢弃。",
                    "确认还原",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    byte[] fileData = System.IO.File.ReadAllBytes(_viewModel.FileB.FilePath);
                    _viewModel.FileB.Data = fileData;
                    _viewModel.IsFileBModified = false;
                    _viewModel.PerformComparison();
                    _viewModel.StatusMessage = $"文件B已还原";
                }
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"还原文件B失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 同步到文件B（文件A）
        /// </summary>
        private void MenuItemSyncToFileB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxFileA.SelectedItems.Count == 0)
                {
                    _viewModel.StatusMessage = "请先选择数据行";
                    return;
                }

                var selectedRows = ListBoxFileA.SelectedItems.Cast<DataRow>().ToList();
                
                if (selectedRows.Count == 0)
                    return;

                // 从文件A中提取数据
                if (_viewModel.FileA == null || _viewModel.FileA.Data.Length == 0)
                {
                    _viewModel.StatusMessage = "文件A为空";
                    return;
                }

                // 将数据写入文件B
                if (_viewModel.FileB == null || _viewModel.FileB.Data.Length == 0)
                {
                    _viewModel.StatusMessage = "文件B为空";
                    return;
                }

                // 创建新的文件B数据
                byte[] newFileB = new byte[_viewModel.FileB.Data.Length];
                Array.Copy(_viewModel.FileB.Data, newFileB, _viewModel.FileB.Data.Length);

                int bytesPerRow = _viewModel.BytesPerRow;
                int syncCount = 0;

                // 为每个选中的行同步数据
                foreach (var row in selectedRows)
                {
                    if (!int.TryParse(row.Address, System.Globalization.NumberStyles.HexNumber, null, out int startAddr))
                        continue;

                    // 获取该行的字节数据
                    int rowLength = row.ByteSegments.Count;
                    
                    // 检查地址范围是否有效
                    if (startAddr + rowLength > _viewModel.FileA.Data.Length)
                        continue;

                    if (startAddr + rowLength > newFileB.Length)
                        continue;

                    // 从文件A复制该行数据到文件B
                    Array.Copy(_viewModel.FileA.Data, startAddr, newFileB, startAddr, rowLength);
                    syncCount++;
                }

                if (syncCount == 0)
                {
                    _viewModel.StatusMessage = "没有有效的行可以同步";
                    return;
                }

                // 更新文件B
                _viewModel.FileB.Data = newFileB;

                // 标记文件B已修改
                _viewModel.IsFileBModified = true;

                // 重新进行对比
                _viewModel.PerformComparison();
                _viewModel.StatusMessage = $"已同步 {syncCount} 行数据到文件B";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"同步到文件B失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 同步到文件A（文件B）
        /// </summary>
        private void MenuItemSyncToFileA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxFileB.SelectedItems.Count == 0)
                {
                    _viewModel.StatusMessage = "请先选择数据行";
                    return;
                }

                var selectedRows = ListBoxFileB.SelectedItems.Cast<DataRow>().ToList();
                
                if (selectedRows.Count == 0)
                    return;

                // 从文件B中提取数据
                if (_viewModel.FileB == null || _viewModel.FileB.Data.Length == 0)
                {
                    _viewModel.StatusMessage = "文件B为空";
                    return;
                }

                // 将数据写入文件A
                if (_viewModel.FileA == null || _viewModel.FileA.Data.Length == 0)
                {
                    _viewModel.StatusMessage = "文件A为空";
                    return;
                }

                // 创建新的文件A数据
                byte[] newFileA = new byte[_viewModel.FileA.Data.Length];
                Array.Copy(_viewModel.FileA.Data, newFileA, _viewModel.FileA.Data.Length);

                int bytesPerRow = _viewModel.BytesPerRow;
                int syncCount = 0;

                // 为每个选中的行同步数据
                foreach (var row in selectedRows)
                {
                    if (!int.TryParse(row.Address, System.Globalization.NumberStyles.HexNumber, null, out int startAddr))
                        continue;

                    // 获取该行的字节数据
                    int rowLength = row.ByteSegments.Count;
                    
                    // 检查地址范围是否有效
                    if (startAddr + rowLength > _viewModel.FileB.Data.Length)
                        continue;

                    if (startAddr + rowLength > newFileA.Length)
                        continue;

                    // 从文件B复制该行数据到文件A
                    Array.Copy(_viewModel.FileB.Data, startAddr, newFileA, startAddr, rowLength);
                    syncCount++;
                }

                if (syncCount == 0)
                {
                    _viewModel.StatusMessage = "没有有效的行可以同步";
                    return;
                }

                // 更新文件A
                _viewModel.FileA.Data = newFileA;

                // 标记文件A已修改
                _viewModel.IsFileAModified = true;

                // 重新进行对比
                _viewModel.PerformComparison();
                _viewModel.StatusMessage = $"已同步 {syncCount} 行数据到文件A";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"同步到文件A失败: {ex.Message}";
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

                // 支持多选，同步文件B的选择
                ListBoxFileB.SelectedItems.Clear();
                foreach (var item in ListBoxFileA.SelectedItems)
                {
                    ListBoxFileB.SelectedItems.Add(item);
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

                // 支持多选，同步文件A的选择
                ListBoxFileA.SelectedItems.Clear();
                foreach (var item in ListBoxFileB.SelectedItems)
                {
                    ListBoxFileA.SelectedItems.Add(item);
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

        /// <summary>
        /// 检查选择是否为连续行或单行
        /// </summary>
        private bool IsContiguousSelection(List<DataRow> selectedRows)
        {
            if (selectedRows == null || selectedRows.Count == 0)
                return false;

            // 单行总是连续的
            if (selectedRows.Count == 1)
                return true;

            // 检查多行是否连续
            // 获取所有行的地址并排序
            var addresses = new List<int>();
            foreach (var row in selectedRows)
            {
                if (int.TryParse(row.Address, System.Globalization.NumberStyles.HexNumber, null, out int addr))
                {
                    addresses.Add(addr);
                }
                else
                {
                    return false;
                }
            }

            addresses.Sort();

            // 检查地址是否连续
            // 假设每行的字节数相同（都是 BytesPerRow）
            int bytesPerRow = _viewModel.BytesPerRow;
            for (int i = 1; i < addresses.Count; i++)
            {
                if (addresses[i] != addresses[i - 1] + bytesPerRow)
                {
                    return false;
                }
            }

            return true;
        }
    }
}