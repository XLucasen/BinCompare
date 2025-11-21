# 功能详解

## 1. 文件选择和加载

### 功能描述
- 支持选择任意.BIN或二进制文件
- 显示文件名和大小信息
- 自动验证文件格式
- 加载后自动进行对比

### 实现细节
```csharp
// 文件加载流程
1. 打开文件对话框
2. 读取文件内容到内存
3. 获取文件信息（名称、大小）
4. 触发自动对比
5. 更新UI显示
```

### 状态提示
- "文件A已加载: filename.bin (1024 字节)"
- "文件B已加载: filename.bin (2048 字节)"
- "对比完成: 发现 5 处差异"

## 2. 二进制对比引擎

### 对比算法
```
时间复杂度：O(n)，其中n是较长文件的字节数
空间复杂度：O(m)，其中m是差异数量
```

### 对比过程
1. 逐字节比较两个文件
2. 记录所有不同的位置
3. 标记超出范围的部分
4. 生成差异信息列表

### 差异分类
| 类型 | 描述 | 示例 |
|------|------|------|
| 数据不同 | 字节值不同 | A: 0x48 vs B: 0x49 |
| 文件A超出 | A更长 | 地址超过B的长度 |
| 文件B超出 | B更长 | 地址超过A的长度 |
| 超出范围 | 两个都结束 | 长度不同时的末尾 |

## 3. 显示模式

### 十六进制模式
- **格式**：大写的0-9、A-F
- **优点**：
  - 紧凑高效
  - 易于理解二进制数据
  - 便于与十六进制编辑器对比
- **示例**：`48 65 6C 6C 6F 20 57 6F 72 6C 64 21`

### 二进制模式
- **格式**：8位的0和1序列
- **优点**：
  - 显示位级细节
  - 便于分析位操作
  - 清晰展示每一位的差异
- **示例**：`01001000 01100101 01101100 01101100`

### 显示切换
```csharp
// 切换时的处理
1. 保存当前滚动位置
2. 重新生成显示行
3. 标记差异位置
4. 恢复滚动位置
```

## 4. 行长度配置

### 可选长度
| 长度 | 用途 | 优点 |
|------|------|------|
| 8字节 | 详细查看 | 精确定位差异 |
| 16字节 | 平衡模式 | 默认选项，最常用 |
| 32字节 | 快速浏览 | 一次看更多数据 |
| 64字节 | 大文件 | 快速定位大范围差异 |

### 地址计算
```
行地址 = 行索引 × 每行字节数
字节地址 = 行地址 + 行内偏移
```

## 5. 差异高亮

### 高亮规则
- **背景色**：浅红色（RGB: 255, 200, 200）
- **文字色**：深红色（RGB: 209, 52, 56）
- **应用范围**：整行（如果该行有差异）

### 高亮过程
```csharp
1. 遍历所有差异
2. 计算差异所在的行
3. 标记该行的DifferenceIndices
4. 在UI中应用颜色转换器
```

### 视觉效果
```
正常行：黑色文字，白色背景
差异行：红色文字，浅红色背景
地址栏：灰色文字，浅灰色背景
```

## 6. 差异信息区

### 显示内容
每条差异显示以下信息：
- **地址**：十六进制地址（可点击）
- **类型**：差异分类
- **文件A值**：原始字节值
- **文件B值**：对比字节值

### 交互功能
- **点击地址**：自动跳转到该位置
- **鼠标悬停**：显示完整信息
- **排序**：按地址升序排列

### 差异统计
- 显示总差异数量
- 实时更新计数
- 支持导出统计

## 7. 滚动同步

### 同步机制
```csharp
// 事件流
ListBoxFileA.ScrollChanged 
  → 获取A的滚动偏移
  → 获取B的ScrollViewer
  → 设置B的滚动偏移
```

### 特点
- 自动同步两列的垂直滚动
- 保持对齐的地址行
- 支持鼠标滚轮和滚动条
- 支持键盘导航

## 8. 快速导航

### 跳转功能
1. 在差异信息区点击地址
2. 计算对应的行索引
3. 滚动两个ListBox到该行
4. 高亮选中行
5. 显示"已跳转到地址: 0x..."

### 导航公式
```
行索引 = 差异字节偏移 ÷ 每行字节数
```

## 9. 差异导出

### 导出格式
```
=== 二进制文件对比报告 ===
文件A: fileA.bin
文件B: fileB.bin
总差异数: 5
=== 差异详情 ===
地址: 0x0000000B | 类型: 数据不同 | A: 21 | B: 3F
地址: 0x0000000F | 类型: 数据不同 | A: 00 | B: 01
...
```

### 导出过程
1. 打开保存文件对话框
2. 生成差异报告文本
3. 保存到指定位置
4. 显示成功提示

## 10. 键盘快捷键

### 快捷键映射
```csharp
Ctrl+O           → SelectFileACommand
Ctrl+Shift+O     → SelectFileBCommand
Ctrl+H           → ToggleModeCommand
Ctrl+E           → ExportDifferencesCommand
Delete           → ClearAllCommand
```

### 实现方式
- 在Window_KeyDown事件中处理
- 支持修饰键组合
- 设置Handled标志防止冒泡

## 11. 数据模型

### BinaryFileData
```csharp
public class BinaryFileData
{
    public string FilePath { get; set; }      // 文件路径
    public byte[] Data { get; set; }          // 文件内容
    public long FileSize { get; set; }        // 文件大小
    public string FileName { get; set; }      // 文件名
}
```

### DifferenceInfo
```csharp
public class DifferenceInfo
{
    public string Address { get; set; }       // 十六进制地址
    public string Description { get; set; }   // 差异描述
    public long ByteOffset { get; set; }      // 字节偏移
    public string FileAValue { get; set; }    // A文件值
    public string FileBValue { get; set; }    // B文件值
}
```

### DataRow
```csharp
public class DataRow
{
    public string Address { get; set; }       // 行地址
    public string Data { get; set; }          // 行数据
    public bool HasDifference { get; set; }   // 是否有差异
    public List<int> DifferenceIndices { get; set; }  // 差异字节索引
}
```

## 12. MVVM架构

### 分层结构
```
View (MainWindow.xaml)
  ↓
ViewModel (MainWindowViewModel)
  ↓
Model (BinaryFileData, DifferenceInfo)
  ↓
Service (BinaryCompareService)
```

### 数据绑定
- 使用INotifyPropertyChanged实现属性变更通知
- 使用ObservableCollection实现集合变更通知
- 使用ICommand实现命令绑定

### 命令模式
```csharp
// 命令定义
SelectFileACommand      // 选择文件A
SelectFileBCommand      // 选择文件B
ClearAllCommand         // 清除所有
ToggleModeCommand       // 切换模式
ExportDifferencesCommand // 导出差异
JumpToDifferenceCommand // 跳转到差异
```

## 13. 值转换器

### DifferenceColorConverter
- 将HasDifference布尔值转换为颜色
- 差异行：红色（RGB: 209, 52, 56）
- 正常行：黑色

### DifferenceBackgroundConverter
- 将HasDifference布尔值转换为背景色
- 差异行：浅红色（RGB: 255, 200, 200）
- 正常行：白色

## 14. 性能优化

### 优化策略
1. **内存管理**：一次性加载文件
2. **算法效率**：O(n)对比算法
3. **UI虚拟化**：ListBox项目虚拟化
4. **事件节流**：避免频繁更新

### 性能指标
- 1MB文件对比：< 100ms
- 10MB文件对比：< 1s
- 100MB文件对比：< 10s

## 15. 错误处理

### 异常处理
```csharp
try
{
    // 文件操作
    byte[] fileData = File.ReadAllBytes(path);
}
catch (Exception ex)
{
    StatusMessage = $"错误: {ex.Message}";
}
```

### 用户反馈
- 状态栏显示操作结果
- 错误消息清晰明了
- 支持重试操作
