using System;
using System.Collections.Generic;
using System.Linq;
using BinCompare.Models;

namespace BinCompare.Services
{
    /// <summary>
    /// 二进制文件对比服务
    /// </summary>
    public class BinaryCompareService
    {
        /// <summary>
        /// 对比两个二进制文件
        /// </summary>
        public List<DifferenceInfo> CompareBinaryFiles(BinaryFileData fileA, BinaryFileData fileB)
        {
            var differences = new List<DifferenceInfo>();

            if (fileA?.Data == null || fileB?.Data == null)
                return differences;

            long maxLength = Math.Max(fileA.Data.Length, fileB.Data.Length);

            for (long i = 0; i < maxLength; i++)
            {
                byte byteA = i < fileA.Data.Length ? fileA.Data[i] : (byte)0xFF;
                byte byteB = i < fileB.Data.Length ? fileB.Data[i] : (byte)0xFF;

                if (byteA != byteB)
                {
                    var diff = new DifferenceInfo
                    {
                        ByteOffset = i,
                        Address = i.ToString("X8"),
                        FileAValue = byteA.ToString("X2"),
                        FileBValue = byteB.ToString("X2"),
                        Description = GetDifferenceDescription(i, fileA.Data.Length, fileB.Data.Length)
                    };
                    differences.Add(diff);
                }
            }

            return differences;
        }

        /// <summary>
        /// 获取差异描述
        /// </summary>
        private string GetDifferenceDescription(long offset, long lengthA, long lengthB)
        {
            if (offset >= lengthA && offset >= lengthB)
                return "超出范围";
            if (offset >= lengthA)
                return "文件A超出";
            if (offset >= lengthB)
                return "文件B超出";
            return "数据不同";
        }

        /// <summary>
        /// 生成显示行数据（十六进制格式）
        /// </summary>
        public List<DataRow> GenerateHexRows(byte[] data, int bytesPerRow, long startOffset = 0)
        {
            var rows = new List<DataRow>();

            if (data == null || data.Length == 0)
                return rows;

            for (int i = 0; i < data.Length; i += bytesPerRow)
            {
                int rowLength = Math.Min(bytesPerRow, data.Length - i);
                byte[] rowData = new byte[rowLength];
                Array.Copy(data, i, rowData, 0, rowLength);

                string hexString = string.Join(" ", rowData.Select(b => b.ToString("X2")));
                string address = (startOffset + i).ToString("X8");
                string asciiString = ConvertBytesToAscii(rowData);

                // 初始化字节段（暂时都是非差异）
                var byteSegments = new List<ByteSegment>();
                foreach (var b in rowData)
                {
                    byteSegments.Add(new ByteSegment(b.ToString("X2"), false));
                }

                rows.Add(new DataRow
                {
                    Address = address,
                    Data = hexString,
                    AsciiData = asciiString,
                    HasDifference = false,
                    DifferenceIndices = new List<int>(),
                    ByteSegments = byteSegments
                });
            }

            return rows;
        }

        /// <summary>
        /// 生成显示行数据（二进制格式）
        /// </summary>
        public List<DataRow> GenerateBinaryRows(byte[] data, int bytesPerRow, long startOffset = 0)
        {
            var rows = new List<DataRow>();

            if (data == null || data.Length == 0)
                return rows;

            for (int i = 0; i < data.Length; i += bytesPerRow)
            {
                int rowLength = Math.Min(bytesPerRow, data.Length - i);
                byte[] rowData = new byte[rowLength];
                Array.Copy(data, i, rowData, 0, rowLength);

                string binaryString = string.Join(" ", rowData.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                string address = (startOffset + i).ToString("X8");
                string asciiString = ConvertBytesToAscii(rowData);

                // 初始化字节段（暂时都是非差异）
                var byteSegments = new List<ByteSegment>();
                foreach (var b in rowData)
                {
                    byteSegments.Add(new ByteSegment(Convert.ToString(b, 2).PadLeft(8, '0'), false));
                }

                rows.Add(new DataRow
                {
                    Address = address,
                    Data = binaryString,
                    AsciiData = asciiString,
                    HasDifference = false,
                    DifferenceIndices = new List<int>(),
                    ByteSegments = byteSegments
                });
            }

            return rows;
        }

        /// <summary>
        /// 标记差异位置
        /// </summary>
        public void MarkDifferences(List<DataRow> rowsA, List<DataRow> rowsB, List<DifferenceInfo> differences, int bytesPerRow)
        {
            foreach (var diff in differences)
            {
                int rowIndex = (int)(diff.ByteOffset / bytesPerRow);
                int byteIndexInRow = (int)(diff.ByteOffset % bytesPerRow);

                if (rowIndex < rowsA.Count)
                {
                    rowsA[rowIndex].HasDifference = true;
                    rowsA[rowIndex].DifferenceIndices.Add(byteIndexInRow);
                    // 标记字节段中的差异
                    if (byteIndexInRow < rowsA[rowIndex].ByteSegments.Count)
                    {
                        rowsA[rowIndex].ByteSegments[byteIndexInRow].IsDifference = true;
                    }
                }

                if (rowIndex < rowsB.Count)
                {
                    rowsB[rowIndex].HasDifference = true;
                    rowsB[rowIndex].DifferenceIndices.Add(byteIndexInRow);
                    // 标记字节段中的差异
                    if (byteIndexInRow < rowsB[rowIndex].ByteSegments.Count)
                    {
                        rowsB[rowIndex].ByteSegments[byteIndexInRow].IsDifference = true;
                    }
                }
            }
        }

        /// <summary>
        /// 将字节数组转换为ASCII字符串
        /// </summary>
        private string ConvertBytesToAscii(byte[] data)
        {
            if (data == null || data.Length == 0)
                return string.Empty;

            var sb = new System.Text.StringBuilder();
            foreach (byte b in data)
            {
                // 如果是可打印的ASCII字符，显示字符；否则显示点
                if (b >= 32 && b <= 126)
                {
                    sb.Append((char)b);
                }
                else
                {
                    sb.Append('.');
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 导出差异信息为文本
        /// </summary>
        public string ExportDifferences(List<DifferenceInfo> differences, string fileAName, string fileBName)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== 二进制文件对比报告 ===");
            sb.AppendLine($"文件A: {fileAName}");
            sb.AppendLine($"文件B: {fileBName}");
            sb.AppendLine($"总差异数: {differences.Count}");
            sb.AppendLine("=== 差异详情 ===");

            foreach (var diff in differences.OrderBy(d => d.ByteOffset))
            {
                sb.AppendLine($"地址: 0x{diff.Address} | 类型: {diff.Description} | A: {diff.FileAValue} | B: {diff.FileBValue}");
            }

            return sb.ToString();
        }
    }
}
