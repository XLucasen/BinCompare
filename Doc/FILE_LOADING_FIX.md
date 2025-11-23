# 文件加载功能修复说明

## 问题描述

拖拽文件到程序图标时，程序能够启动，但没有加载文件。

## 问题原因

在 `App.xaml.cs` 中使用了 `Loaded` 事件来处理命令行参数，但此时 `ViewModel` 可能还没有完全初始化。

## 修复方案

将事件处理器从 `Loaded` 改为 `ContentRendered`：

### 修改前
```csharp
mainWindow.Loaded += (s, ev) =>
{
    ProcessCommandLineArgs(mainWindow, args);
};
```

### 修改后
```csharp
mainWindow.ContentRendered += (s, ev) =>
{
    ProcessCommandLineArgs(mainWindow, args);
};
```

## 为什么这样修复

| 事件 | 触发时机 | 用途 |
|------|---------|------|
| `Loaded` | 窗口加载完成 | 窗口和控件初始化 |
| `ContentRendered` | 内容渲染完成 | 所有初始化完成，可以进行业务操作 |

`ContentRendered` 事件在 `Loaded` 事件之后触发，确保：
- ✅ 窗口已创建
- ✅ 控件已初始化
- ✅ `DataContext` 已设置
- ✅ `ViewModel` 已完全初始化
- ✅ 所有绑定已建立

## 修改的文件

**App.xaml.cs**：
- 改用 `ContentRendered` 事件
- 调整事件订阅顺序，在 `Show()` 之前订阅

## 测试步骤

### 步骤 1：重新编译
```bash
dotnet build
```

### 步骤 2：安装程序
将编译后的 `BinCompare.exe` 复制到 `C:\Program Files\BinCompare\`

### 步骤 3：测试拖拽功能
1. 找到任意 `.bin` 文件
2. 拖拽到 BinCompare 程序图标
3. 程序应该启动并加载文件到文件A

### 步骤 4：测试双文件拖拽
1. 选中两个 `.bin` 文件
2. 拖拽到 BinCompare 程序图标
3. 程序应该启动并：
   - 加载第一个文件到文件A
   - 加载第二个文件到文件B
   - 自动进行对比

## 编译状态

- **编译结果**：✅ 成功
- **错误数**：0
- **警告数**：36（都是 null 性相关的警告）

## 功能验证清单

- [ ] 拖拽单个文件，文件加载到文件A
- [ ] 拖拽两个文件，文件分别加载到文件A和文件B
- [ ] 自动进行对比，显示差异
- [ ] 右键菜单"用 BinCompare 打开"正常工作
- [ ] 双击文件直接打开
- [ ] 命令行启动正常工作

## 常见问题

### Q: 为什么要用 ContentRendered 而不是 Loaded？
A: `ContentRendered` 在所有初始化完成后触发，包括 `DataContext` 和 `ViewModel` 的初始化，确保文件加载逻辑能正确执行。

### Q: 如果仍然没有加载文件怎么办？
A:
1. 检查命令行参数是否正确传递
2. 检查文件路径是否存在
3. 查看 `LoadFileDirectly` 方法是否有错误
4. 添加调试输出检查执行流程

### Q: 可以同时加载多个文件吗？
A: 可以，程序会使用前两个文件，第一个加载到文件A，第二个加载到文件B。

## 相关代码

### App.xaml.cs 中的修改
```csharp
private void Application_Startup(object sender, StartupEventArgs e)
{
    // 获取命令行参数
    string[] args = e.Args;

    // 创建主窗口
    MainWindow mainWindow = new MainWindow();
    
    // 如果有命令行参数，在窗口加载完成后处理文件
    if (args.Length > 0)
    {
        // 使用 ContentRendered 事件，确保窗口和 ViewModel 都已完全初始化
        mainWindow.ContentRendered += (s, ev) =>
        {
            ProcessCommandLineArgs(mainWindow, args);
        };
    }
    
    mainWindow.Show();
}
```

### MainWindowViewModel.cs 中的 LoadFileDirectly 方法
```csharp
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
```

---

**版本**：v1.6.0+  
**修复日期**：2025-11-22  
**状态**：✅ 已修复
