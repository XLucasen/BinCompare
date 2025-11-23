# 拖拽文件到程序图标加载文件功能指南

## 功能说明

本功能允许用户在未打开程序的情况下，直接拖拽二进制文件到程序图标上，自动打开程序并加载文件。

## 使用方式

### 场景 1：拖拽单个文件
1. 找到一个 `.bin` 二进制文件
2. 将文件拖拽到 BinCompare 程序图标上
3. 程序自动启动并将文件加载到**文件A**
4. 可以手动加载文件B进行对比

### 场景 2：拖拽两个文件
1. 找到两个 `.bin` 二进制文件
2. 同时选中两个文件（Ctrl+点击）
3. 将两个文件拖拽到 BinCompare 程序图标上
4. 程序自动启动并：
   - 第一个文件加载到**文件A**
   - 第二个文件加载到**文件B**
   - 自动进行对比

### 场景 3：拖拽多个文件
- 如果拖拽 3 个或更多文件，只有前两个会被加载
- 第一个文件 → 文件A
- 第二个文件 → 文件B
- 其他文件被忽略

## 技术实现

### 工作原理

1. **命令行参数传递**
   - 当拖拽文件到程序图标时，Windows 会将文件路径作为命令行参数传递给程序
   - 例如：`BinCompare.exe "C:\path\to\file1.bin" "C:\path\to\file2.bin"`

2. **应用启动处理**
   - `App.xaml.cs` 中的 `Application_Startup` 事件处理器捕获命令行参数
   - 等待主窗口加载完成后，处理文件加载

3. **文件加载**
   - `MainWindowViewModel.LoadFileDirectly()` 方法直接加载指定的文件
   - 自动进行对比（如果两个文件都已加载）

### 修改的文件

#### 1. App.xaml
```xml
<!-- 添加 Startup 事件处理 -->
<Application ... Startup="Application_Startup">
```

#### 2. App.xaml.cs
```csharp
// 应用启动事件处理
private void Application_Startup(object sender, StartupEventArgs e)
{
    string[] args = e.Args;
    if (args.Length > 0)
    {
        MainWindow.Loaded += (s, ev) =>
        {
            ProcessCommandLineArgs(args);
        };
    }
}

// 处理命令行参数
private void ProcessCommandLineArgs(string[] args)
{
    if (args.Length == 1)
    {
        // 单个文件：加载到文件A
        viewModel.LoadFileDirectly(args[0], true);
    }
    else if (args.Length >= 2)
    {
        // 两个文件：分别加载到文件A和文件B
        viewModel.LoadFileDirectly(args[0], true);
        viewModel.LoadFileDirectly(args[1], false);
    }
}
```

#### 3. MainWindowViewModel.cs
```csharp
// 新增方法：直接加载文件
public void LoadFileDirectly(string filePath, bool isFileA)
{
    // 读取文件数据
    // 创建 BinaryFileData 对象
    // 自动进行对比（如果两个文件都已加载）
}
```

## 支持的文件类型

- **主要格式**：`.bin` 二进制文件
- **其他格式**：任何二进制文件（`.dat`, `.hex`, `.rom` 等）
- **文件大小**：支持任意大小的文件

## 错误处理

### 文件不存在
- 如果指定的文件不存在，会显示错误信息
- 程序继续运行，等待用户手动加载文件

### 文件读取失败
- 如果文件无法读取（权限问题等），会显示错误信息
- 程序继续运行

### 命令行参数处理错误
- 如果处理命令行参数时发生异常，会显示错误对话框
- 程序继续运行

## 常见问题

### Q: 拖拽文件后程序没有反应？
A: 
1. 确保文件格式正确（最好是 `.bin` 文件）
2. 检查文件是否存在且可读
3. 查看程序状态栏的错误信息

### Q: 拖拽两个文件，但只加载了一个？
A: 
1. 确保两个文件都被正确选中
2. 确保两个文件都存在且可读
3. 查看程序状态栏的加载信息

### Q: 拖拽文件后程序启动很慢？
A: 
1. 这是正常的，因为程序需要读取文件数据
2. 文件越大，启动时间越长
3. 等待程序完全加载即可

### Q: 可以拖拽其他类型的文件吗？
A: 
可以，程序会尝试读取任何文件。但建议使用二进制文件（`.bin`, `.dat`, `.hex` 等）

## 高级用法

### 通过命令行手动启动
```bash
# 加载单个文件到文件A
BinCompare.exe "C:\path\to\fileA.bin"

# 加载两个文件进行对比
BinCompare.exe "C:\path\to\fileA.bin" "C:\path\to\fileB.bin"
```

### 批处理脚本
```batch
@echo off
REM 自动对比两个文件
start BinCompare.exe "C:\data\file1.bin" "C:\data\file2.bin"
```

### PowerShell 脚本
```powershell
# 对比两个文件
& "C:\Program Files\BinCompare\BinCompare.exe" "C:\data\file1.bin" "C:\data\file2.bin"
```

## 性能考虑

- **文件大小**：支持任意大小的文件
- **加载时间**：取决于文件大小和系统性能
- **内存占用**：文件数据会完全加载到内存中

## 安全性

- 程序只读取文件，不修改原始文件
- 命令行参数中的文件路径不会被记录或上传
- 所有处理都在本地进行

## 未来改进

- [ ] 支持拖拽文件夹（自动对比文件夹中的所有文件）
- [ ] 支持拖拽多个文件对（自动进行多个对比）
- [ ] 添加最近打开文件列表
- [ ] 支持通过关联文件类型快速打开

## 测试清单

- [ ] 拖拽单个文件到程序图标，验证文件加载到文件A
- [ ] 拖拽两个文件到程序图标，验证文件分别加载到文件A和文件B
- [ ] 拖拽不存在的文件，验证错误处理
- [ ] 通过命令行启动程序，验证文件加载
- [ ] 拖拽大文件，验证加载性能
- [ ] 拖拽不同格式的文件（`.bin`, `.dat`, `.hex` 等）

---

**版本**：v1.6.0+  
**状态**：✅ 已实现
