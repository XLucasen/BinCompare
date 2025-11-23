using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BinCompare.Models;

namespace BinCompare
{
    /// <summary>
    /// 数据编辑窗口
    /// </summary>
    public partial class DataEditWindow : Window
    {
        /// <summary>
        /// 编辑后的字节数据
        /// </summary>
        public byte[] EditedData { get; private set; }

        /// <summary>
        /// 是否确认编辑
        /// </summary>
        public bool IsConfirmed { get; private set; }

        /// <summary>
        /// 选中的数据行列表
        /// </summary>
        private List<DataRow> _selectedRows;

        public DataEditWindow()
        {
            InitializeComponent();
            IsConfirmed = false;
            EditedData = null;
        }

        /// <summary>
        /// 初始化编辑窗口
        /// </summary>
        public void Initialize(List<DataRow> selectedRows)
        {
            _selectedRows = selectedRows;

            if (_selectedRows == null || _selectedRows.Count == 0)
                return;

            // 显示地址信息
            string firstAddress = _selectedRows.First().Address;
            string lastAddress = _selectedRows.Last().Address;
            
            if (_selectedRows.Count == 1)
            {
                AddressInfo.Text = $"地址: {firstAddress}";
            }
            else
            {
                AddressInfo.Text = $"地址: {firstAddress} - {lastAddress} (共 {_selectedRows.Count} 行)";
            }

            // 显示十六进制数据
            var hexLines = new List<string>();
            foreach (var row in _selectedRows)
            {
                var hexValues = row.ByteSegments.Select(b => b.Text).ToList();
                hexLines.Add(string.Join(" ", hexValues));
            }

            HexDataTextBox.Text = string.Join("\n", hexLines);
        }

        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorMessage.Text = "";
                
                // 解析十六进制数据
                string hexText = HexDataTextBox.Text.Trim();
                if (string.IsNullOrEmpty(hexText))
                {
                    ErrorMessage.Text = "请输入十六进制数据";
                    return;
                }

                // 将多行数据合并为一行，用空格分隔
                string[] lines = hexText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                var hexValues = new List<string>();
                
                foreach (var line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (string.IsNullOrEmpty(trimmedLine))
                        continue;

                    // 分割十六进制值
                    string[] parts = trimmedLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    hexValues.AddRange(parts);
                }

                if (hexValues.Count == 0)
                {
                    ErrorMessage.Text = "请输入有效的十六进制数据";
                    return;
                }

                // 验证十六进制格式并转换为字节数组
                var bytes = new List<byte>();
                foreach (var hex in hexValues)
                {
                    if (hex.Length != 2 || !byte.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out byte value))
                    {
                        ErrorMessage.Text = $"无效的十六进制值: {hex}";
                        return;
                    }
                    bytes.Add(value);
                }

                // 计算预期的字节数
                int expectedByteCount = _selectedRows.Sum(r => r.ByteSegments.Count);
                
                if (bytes.Count != expectedByteCount)
                {
                    ErrorMessage.Text = $"字节数不匹配。预期: {expectedByteCount}, 实际: {bytes.Count}";
                    return;
                }

                EditedData = bytes.ToArray();
                IsConfirmed = true;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = $"错误: {ex.Message}";
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            DialogResult = false;
            Close();
        }
    }
}
