using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BinCompare.Models
{
    /// <summary>
    /// 二进制文件数据模型
    /// </summary>
    public class BinaryFileData
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件字节数据
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        public BinaryFileData()
        {
            Data = Array.Empty<byte>();
            FileSize = 0;
            FilePath = string.Empty;
            FileName = string.Empty;
        }
    }

    /// <summary>
    /// 差异信息模型
    /// </summary>
    public class DifferenceInfo
    {
        /// <summary>
        /// 差异起始地址（十六进制）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 差异类型描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 差异的字节偏移量
        /// </summary>
        public long ByteOffset { get; set; }

        /// <summary>
        /// 文件A的值
        /// </summary>
        public string FileAValue { get; set; }

        /// <summary>
        /// 文件B的值
        /// </summary>
        public string FileBValue { get; set; }

        public DifferenceInfo()
        {
            Address = string.Empty;
            Description = string.Empty;
            FileAValue = string.Empty;
            FileBValue = string.Empty;
        }
    }

    /// <summary>
    /// 字节段模型（用于显示单个字节的差异高亮）
    /// </summary>
    public class ByteSegment : INotifyPropertyChanged
    {
        private string _text;
        private bool _isDifference;
        private bool _isHighlighted;

        /// <summary>
        /// 字节段文本内容
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        /// <summary>
        /// 是否为差异字节
        /// </summary>
        public bool IsDifference
        {
            get => _isDifference;
            set
            {
                if (_isDifference != value)
                {
                    _isDifference = value;
                    OnPropertyChanged(nameof(IsDifference));
                }
            }
        }

        /// <summary>
        /// 是否为临时高亮字节
        /// </summary>
        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    OnPropertyChanged(nameof(IsHighlighted));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ByteSegment()
        {
            _text = string.Empty;
            _isDifference = false;
            _isHighlighted = false;
        }

        public ByteSegment(string text, bool isDifference)
        {
            _text = text;
            _isDifference = isDifference;
            _isHighlighted = false;
        }
    }

    /// <summary>
    /// 数据行模型
    /// </summary>
    public class DataRow
    {
        /// <summary>
        /// 行地址（十六进制）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// ASCII字符表示
        /// </summary>
        public string AsciiData { get; set; }

        /// <summary>
        /// 是否有差异
        /// </summary>
        public bool HasDifference { get; set; }

        /// <summary>
        /// 差异字节索引列表
        /// </summary>
        public List<int> DifferenceIndices { get; set; }

        /// <summary>
        /// 字节段列表（用于单字节高亮）
        /// </summary>
        public List<ByteSegment> ByteSegments { get; set; }

        public DataRow()
        {
            Address = string.Empty;
            Data = string.Empty;
            AsciiData = string.Empty;
            HasDifference = false;
            DifferenceIndices = new List<int>();
            ByteSegments = new List<ByteSegment>();
        }
    }
}
