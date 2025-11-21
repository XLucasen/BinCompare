# 二进制文件对比工具 - 文档索引

## 📚 快速导航

### 🚀 快速开始
- **[QUICKSTART.md](QUICKSTART.md)** - 5分钟快速上手指南
  - 安装和运行
  - 基本操作步骤
  - 常见操作
  - 键盘快捷键

### 📖 完整文档
- **[README.md](README.md)** - 项目完整说明
  - 功能特性
  - 项目结构
  - 技术栈
  - 使用指南
  - 编译和运行

- **[FEATURES.md](FEATURES.md)** - 功能详解
  - 15个功能详细说明
  - 算法原理
  - 代码示例
  - 性能指标

- **[PROJECT_SUMMARY.md](PROJECT_SUMMARY.md)** - 项目总结
  - 核心成就
  - 技术架构
  - 项目统计
  - 学习价值

- **[DRAG_DROP_FEATURE.md](DRAG_DROP_FEATURE.md)** - 拖拽功能说明
  - 功能概述
  - 使用方法
  - 技术实现
  - 常见问题

- **[UPDATE_NOTES.md](UPDATE_NOTES.md)** - 更新说明
  - 新增功能
  - 技术实现
  - 编译状态
  - 向后兼容性

### ✅ 质量保证
- **[VERIFICATION_CHECKLIST.md](VERIFICATION_CHECKLIST.md)** - 验证清单
  - 功能验证
  - 代码质量
  - 架构验证
  - 性能验证
  - 测试场景

- **[DELIVERY_SUMMARY.md](DELIVERY_SUMMARY.md)** - 交付总结
  - 交付物清单
  - 功能实现清单
  - 技术指标
  - 质量保证

## 📁 源代码结构

### 数据模型 (Models/)
```
Models/
└── BinaryFileData.cs
    ├── BinaryFileData - 二进制文件数据
    ├── DifferenceInfo - 差异信息
    └── DataRow - 显示行数据
```

### 业务逻辑 (ViewModels/)
```
ViewModels/
├── MainWindowViewModel.cs - 主窗口业务逻辑
├── ViewModelBase.cs - ViewModel基类
└── RelayCommand.cs - 命令实现
```

### 服务层 (Services/)
```
Services/
└── BinaryCompareService.cs
    ├── CompareBinaryFiles - 对比算法
    ├── GenerateHexRows - 十六进制行生成
    ├── GenerateBinaryRows - 二进制行生成
    ├── MarkDifferences - 差异标记
    └── ExportDifferences - 差异导出
```

### 值转换器 (Converters/)
```
Converters/
└── DifferenceColorConverter.cs
    ├── DifferenceColorConverter - 颜色转换
    └── DifferenceBackgroundConverter - 背景转换
```

### 用户界面
```
UI/
├── MainWindow.xaml - 主窗口UI
├── MainWindow.xaml.cs - 交互逻辑
├── App.xaml - 应用资源
└── App.xaml.cs - 应用代码
```

## 🔍 按功能查找

### 文件操作
- 选择文件：[MainWindowViewModel.cs](ViewModels/MainWindowViewModel.cs) - `SelectFile()`
- 加载文件：[BinaryCompareService.cs](Services/BinaryCompareService.cs) - `CompareBinaryFiles()`
- 清除数据：[MainWindowViewModel.cs](ViewModels/MainWindowViewModel.cs) - `ClearAll()`

### 显示模式
- 十六进制：[BinaryCompareService.cs](Services/BinaryCompareService.cs) - `GenerateHexRows()`
- 二进制：[BinaryCompareService.cs](Services/BinaryCompareService.cs) - `GenerateBinaryRows()`
- 模式切换：[MainWindowViewModel.cs](ViewModels/MainWindowViewModel.cs) - `IsHexMode` 属性

### 差异处理
- 差异检测：[BinaryCompareService.cs](Services/BinaryCompareService.cs) - `CompareBinaryFiles()`
- 差异标记：[BinaryCompareService.cs](Services/BinaryCompareService.cs) - `MarkDifferences()`
- 差异导出：[BinaryCompareService.cs](Services/BinaryCompareService.cs) - `ExportDifferences()`

### 交互功能
- 快速导航：[MainWindow.xaml.cs](MainWindow.xaml.cs) - `ViewModel_JumpToDifferenceRequested()`
- 滚动同步：[MainWindow.xaml.cs](MainWindow.xaml.cs) - `ListBoxFileA_ScrollChanged()`
- 键盘快捷键：[MainWindow.xaml.cs](MainWindow.xaml.cs) - `Window_KeyDown()`

## 🎯 按任务查找

### 我想...

#### 快速开始使用
→ 阅读 [QUICKSTART.md](QUICKSTART.md)

#### 了解所有功能
→ 阅读 [README.md](README.md) 和 [FEATURES.md](FEATURES.md)

#### 学习代码架构
→ 阅读 [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md) 的"技术架构"部分

#### 修改UI界面
→ 编辑 [MainWindow.xaml](MainWindow.xaml)

#### 添加新功能
→ 修改 [MainWindowViewModel.cs](ViewModels/MainWindowViewModel.cs)

#### 改进对比算法
→ 修改 [BinaryCompareService.cs](Services/BinaryCompareService.cs)

#### 修复问题
→ 查看 [VERIFICATION_CHECKLIST.md](VERIFICATION_CHECKLIST.md) 的"已知问题"

#### 了解性能
→ 阅读 [FEATURES.md](FEATURES.md) 的"性能优化"部分

#### 部署应用
→ 阅读 [README.md](README.md) 的"编译和运行"部分

#### 测试应用
→ 查看 [VERIFICATION_CHECKLIST.md](VERIFICATION_CHECKLIST.md) 的"测试场景"

## 📊 文档统计

| 文档 | 用途 | 长度 | 更新 |
|------|------|------|------|
| README.md | 项目概述 | ~2000字 | ✅ |
| QUICKSTART.md | 快速开始 | ~1500字 | ✅ |
| FEATURES.md | 功能详解 | ~3000字 | ✅ |
| PROJECT_SUMMARY.md | 项目总结 | ~2500字 | ✅ |
| VERIFICATION_CHECKLIST.md | 验证清单 | ~1500字 | ✅ |
| DELIVERY_SUMMARY.md | 交付总结 | ~1000字 | ✅ |
| INDEX.md | 文档索引 | ~1000字 | ✅ |

**总计**：~12,500字

## 🔗 相关链接

### 官方资源
- [.NET 8.0 文档](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [WPF 文档](https://learn.microsoft.com/dotnet/desktop/wpf/)
- [MVVM 模式](https://learn.microsoft.com/dotnet/architecture/maui/mvvm)

### 开发工具
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Visual Studio Code](https://code.visualstudio.com/)
- [.NET CLI](https://learn.microsoft.com/dotnet/core/tools/)

## 📞 获取帮助

### 常见问题
- 查看 [QUICKSTART.md](QUICKSTART.md) 的"故障排除"部分
- 查看 [README.md](README.md) 的"常见操作"部分

### 技术问题
- 查看 [FEATURES.md](FEATURES.md) 的相关功能说明
- 查看源代码的注释

### 功能建议
- 查看 [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md) 的"未来改进方向"

### 问题报告
- 查看 [VERIFICATION_CHECKLIST.md](VERIFICATION_CHECKLIST.md) 的"已知问题"

## 🎓 学习路径

### 初级用户
1. 阅读 [QUICKSTART.md](QUICKSTART.md)
2. 运行应用程序
3. 加载测试文件
4. 尝试各项功能

### 中级用户
1. 阅读 [README.md](README.md)
2. 阅读 [FEATURES.md](FEATURES.md)
3. 查看源代码
4. 尝试自定义配置

### 高级用户
1. 阅读 [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md)
2. 研究 [BinaryCompareService.cs](Services/BinaryCompareService.cs)
3. 学习 MVVM 架构
4. 考虑功能扩展

## 📋 项目信息

- **项目名称**：二进制文件对比工具
- **版本**：1.0.0
- **状态**：✅ 完成
- **质量评级**：⭐⭐⭐⭐⭐
- **最后更新**：2025年11月21日

## 🚀 快速命令

### 编译
```bash
dotnet build
```

### 运行
```bash
dotnet run
```

### 发布
```bash
dotnet publish -c Release
```

### 清理
```bash
dotnet clean
```

## 📝 文件清单

### 源代码文件（9个）
- [x] App.xaml.cs
- [x] MainWindow.xaml.cs
- [x] MainWindow.xaml
- [x] App.xaml
- [x] Models/BinaryFileData.cs
- [x] ViewModels/MainWindowViewModel.cs
- [x] ViewModels/ViewModelBase.cs
- [x] ViewModels/RelayCommand.cs
- [x] Services/BinaryCompareService.cs
- [x] Converters/DifferenceColorConverter.cs

### 文档文件（7个）
- [x] README.md
- [x] QUICKSTART.md
- [x] FEATURES.md
- [x] PROJECT_SUMMARY.md
- [x] VERIFICATION_CHECKLIST.md
- [x] DELIVERY_SUMMARY.md
- [x] INDEX.md（本文件）

### 配置文件（2个）
- [x] BinCompare.csproj
- [x] BinCompare.csproj.user

### 测试文件（2个）
- [x] TestFileA.bin
- [x] TestFileB.bin

---

**最后更新**：2025年11月21日  
**维护者**：Cascade AI  
**许可证**：MIT
