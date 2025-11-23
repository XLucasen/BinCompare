using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using BinCompare.Models;
using BinCompare.Services;

namespace BinCompare.ViewModels
{
    /// <summary>
    /// 主窗口ViewModel
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly BinaryCompareService _compareService;

        private BinaryFileData _fileA;
        private BinaryFileData _fileB;
        private BinaryFileData _originalFileA;
        private BinaryFileData _originalFileB;
        private ObservableCollection<DataRow> _dataRowsA;
        private ObservableCollection<DataRow> _dataRowsB;
        private ObservableCollection<DifferenceInfo> _differences;
        private bool _isHexMode;
        private int _bytesPerRow;
        private string _statusMessage;
        private int _differenceCount;
        private bool _showAscii;
        private bool _isFileAModified;
        private bool _isFileBModified;

        /// <summary>
        /// 文件A数据
        /// </summary>
        public BinaryFileData FileA
        {
            get => _fileA;
            set => SetProperty(ref _fileA, value);
        }

        /// <summary>
        /// 文件B数据
        /// </summary>
        public BinaryFileData FileB
        {
            get => _fileB;
            set => SetProperty(ref _fileB, value);
        }

        /// <summary>
        /// 文件A的数据行集合
        /// </summary>
        public ObservableCollection<DataRow> DataRowsA
        {
            get => _dataRowsA;
            set => SetProperty(ref _dataRowsA, value);
        }

        /// <summary>
        /// 文件B的数据行集合
        /// </summary>
        public ObservableCollection<DataRow> DataRowsB
        {
            get => _dataRowsB;
            set => SetProperty(ref _dataRowsB, value);
        }

        /// <summary>
        /// 差异信息集合
        /// </summary>
        public ObservableCollection<DifferenceInfo> Differences
        {
            get => _differences;
            set => SetProperty(ref _differences, value);
        }

        /// <summary>
        /// 是否为十六进制模式
        /// </summary>
        public bool IsHexMode
        {
            get => _isHexMode;
            set
            {
                if (SetProperty(ref _isHexMode, value))
                {
                    RefreshDisplay();
                }
            }
        }

        /// <summary>
        /// 每行显示字节数
        /// </summary>
        public int BytesPerRow
        {
            get => _bytesPerRow;
            set
            {
                if (SetProperty(ref _bytesPerRow, value))
                {
                    RefreshDisplay();
                }
            }
        }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        /// <summary>
        /// 差异总数
        /// </summary>
        public int DifferenceCount
        {
            get => _differenceCount;
            set => SetProperty(ref _differenceCount, value);
        }

        /// <summary>
        /// 是否显示ASCII字符
        /// </summary>
        public bool ShowAscii
        {
            get => _showAscii;
            set => SetProperty(ref _showAscii, value);
        }

        /// <summary>
        /// 文件A是否被修改
        /// </summary>
        public bool IsFileAModified
        {
            get => _isFileAModified;
            set => SetProperty(ref _isFileAModified, value);
        }

        /// <summary>
        /// 文件B是否被修改
        /// </summary>
        public bool IsFileBModified
        {
            get => _isFileBModified;
            set => SetProperty(ref _isFileBModified, value);
        }

        /// <summary>
        /// 选择文件A命令
        /// </summary>
        public ICommand SelectFileACommand { get; }

        /// <summary>
        /// 选择文件B命令
        /// </summary>
        public ICommand SelectFileBCommand { get; }

        /// <summary>
        /// 清除所有命令
        /// </summary>
        public ICommand ClearAllCommand { get; }

        /// <summary>
        /// 切换显示模式命令
        /// </summary>
        public ICommand ToggleModeCommand { get; }

        /// <summary>
        /// 导出差异命令
        /// </summary>
        public ICommand ExportDifferencesCommand { get; }

        /// <summary>
        /// 跳转到差异命令
        /// </summary>
        public ICommand JumpToDifferenceCommand { get; }

        /// <summary>
        /// 切换ASCII显示命令
        /// </summary>
        public ICommand ToggleAsciiCommand { get; }

        public MainWindowViewModel()
        {
            _compareService = new BinaryCompareService();
            _fileA = new BinaryFileData();
            _fileB = new BinaryFileData();
            _dataRowsA = new ObservableCollection<DataRow>();
            _dataRowsB = new ObservableCollection<DataRow>();
            _differences = new ObservableCollection<DifferenceInfo>();
            _isHexMode = true;
            _bytesPerRow = 16;
            _statusMessage = "就绪";
            _differenceCount = 0;
            _showAscii = true;

            SelectFileACommand = new RelayCommand(_ => SelectFile(true));
            SelectFileBCommand = new RelayCommand(_ => SelectFile(false));
            ClearAllCommand = new RelayCommand(_ => ClearAll());
            ToggleModeCommand = new RelayCommand(_ => ToggleMode());
            ExportDifferencesCommand = new RelayCommand(_ => ExportDifferences());
            JumpToDifferenceCommand = new RelayCommand<DifferenceInfo>(diff => JumpToDifference(diff));
            ToggleAsciiCommand = new RelayCommand(_ => ToggleAscii());
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        private void SelectFile(bool isFileA)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "二进制文件 (*.bin)|*.bin|所有文件 (*.*)|*.*",
                    Title = isFileA ? "选择文件A" : "选择文件B"
                };

                if (dialog.ShowDialog() == true)
                {
                    byte[] fileData = File.ReadAllBytes(dialog.FileName);
                    var fileInfo = new FileInfo(dialog.FileName);

                    if (isFileA)
                    {
                        FileA = new BinaryFileData
                        {
                            FilePath = dialog.FileName,
                            Data = fileData,
                            FileSize = fileInfo.Length,
                            FileName = fileInfo.Name
                        };
                        StatusMessage = $"文件A已加载: {fileInfo.Name} ({fileInfo.Length} 字节)";
                    }
                    else
                    {
                        FileB = new BinaryFileData
                        {
                            FilePath = dialog.FileName,
                            Data = fileData,
                            FileSize = fileInfo.Length,
                            FileName = fileInfo.Name
                        };
                        StatusMessage = $"文件B已加载: {fileInfo.Name} ({fileInfo.Length} 字节)";
                    }

                    // 如果两个文件都已加载，自动进行对比
                    if (FileA.Data.Length > 0 && FileB.Data.Length > 0)
                    {
                        PerformComparison();
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"错误: {ex.Message}";
            }
        }

        /// <summary>
        /// 执行对比
        /// </summary>
        public void PerformComparison()
        {
            try
            {
                // 对比文件
                var differences = _compareService.CompareBinaryFiles(FileA, FileB);
                DifferenceCount = differences.Count;

                // 生成显示行
                var rowsA = IsHexMode
                    ? _compareService.GenerateHexRows(FileA.Data, BytesPerRow)
                    : _compareService.GenerateBinaryRows(FileA.Data, BytesPerRow);

                var rowsB = IsHexMode
                    ? _compareService.GenerateHexRows(FileB.Data, BytesPerRow)
                    : _compareService.GenerateBinaryRows(FileB.Data, BytesPerRow);

                // 标记差异
                _compareService.MarkDifferences(rowsA, rowsB, differences, BytesPerRow);

                // 更新UI
                DataRowsA = new ObservableCollection<DataRow>(rowsA);
                DataRowsB = new ObservableCollection<DataRow>(rowsB);
                Differences = new ObservableCollection<DifferenceInfo>(differences.OrderBy(d => d.ByteOffset));

                StatusMessage = $"对比完成: 发现 {differences.Count} 处差异";
            }
            catch (Exception ex)
            {
                StatusMessage = $"对比错误: {ex.Message}";
            }
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void RefreshDisplay()
        {
            if (FileA.Data.Length > 0 && FileB.Data.Length > 0)
            {
                PerformComparison();
            }
        }

        /// <summary>
        /// 切换显示模式
        /// </summary>
        private void ToggleMode()
        {
            IsHexMode = !IsHexMode;
        }

        /// <summary>
        /// 清除所有
        /// </summary>
        private void ClearAll()
        {
            FileA = new BinaryFileData();
            FileB = new BinaryFileData();
            DataRowsA = new ObservableCollection<DataRow>();
            DataRowsB = new ObservableCollection<DataRow>();
            Differences = new ObservableCollection<DifferenceInfo>();
            DifferenceCount = 0;
            StatusMessage = "已清除所有数据";
        }

        /// <summary>
        /// 导出差异
        /// </summary>
        private void ExportDifferences()
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "文本文件 (*.txt)|*.txt",
                    FileName = $"差异报告_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                };

                if (dialog.ShowDialog() == true)
                {
                    string content = _compareService.ExportDifferences(
                        Differences.ToList(),
                        FileA.FileName,
                        FileB.FileName
                    );

                    File.WriteAllText(dialog.FileName, content);
                    StatusMessage = $"差异已导出到: {dialog.FileName}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"导出错误: {ex.Message}";
            }
        }

        /// <summary>
        /// 切换ASCII显示
        /// </summary>
        private void ToggleAscii()
        {
            ShowAscii = !ShowAscii;
            StatusMessage = ShowAscii ? "ASCII字符已显示" : "ASCII字符已隐藏";
        }

        /// <summary>
        /// 跳转到差异位置
        /// </summary>
        public event EventHandler<DifferenceInfo> JumpToDifferenceRequested;

        private void JumpToDifference(DifferenceInfo diff)
        {
            JumpToDifferenceRequested?.Invoke(this, diff);
        }

        /// <summary>
        /// 直接加载文件（用于命令行参数）
        /// </summary>
        public void LoadFileDirectly(string filePath, bool isFileA)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    StatusMessage = $"文件不存在: {filePath}";
                    return;
                }

                byte[] fileData = File.ReadAllBytes(filePath);
                var fileInfo = new FileInfo(filePath);

                if (isFileA)
                {
                    FileA = new BinaryFileData
                    {
                        FilePath = filePath,
                        Data = fileData,
                        FileSize = fileInfo.Length,
                        FileName = fileInfo.Name
                    };
                    StatusMessage = $"文件A已加载: {fileInfo.Name} ({fileInfo.Length} 字节)";
                }
                else
                {
                    FileB = new BinaryFileData
                    {
                        FilePath = filePath,
                        Data = fileData,
                        FileSize = fileInfo.Length,
                        FileName = fileInfo.Name
                    };
                    StatusMessage = $"文件B已加载: {fileInfo.Name} ({fileInfo.Length} 字节)";
                }

                // 如果两个文件都已加载，自动进行对比
                if (FileA.Data.Length > 0 && FileB.Data.Length > 0)
                {
                    PerformComparison();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"加载文件错误: {ex.Message}";
            }
        }
    }
}
