using System;
using System.Collections.Generic;

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

        public DataRow()
        {
            Address = string.Empty;
            Data = string.Empty;
            AsciiData = string.Empty;
            HasDifference = false;
            DifferenceIndices = new List<int>();
        }
    }
}
