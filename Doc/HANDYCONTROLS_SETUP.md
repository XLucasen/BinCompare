# HandyControls 部署说明 - v1.5.1

**更新日期**：2025年11月22日  
**版本**：1.5.1  
**更新类型**：UI框架集成

## 概述

成功安装并配置了HandyControls UI框架，为应用提供现代化的WPF控件库和主题支持。

## 安装步骤

### 1. NuGet包安装

```bash
dotnet add package HandyControl
```

**安装结果**：
- 包名：HandyControl
- 版本：3.5.1
- 状态：✅ 成功安装

### 2. App.xaml 配置

在`App.xaml`中添加HandyControls命名空间和主题资源：

```xml
<Application x:Class="BinCompare.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:BinCompare"
             xmlns:hc="https://handyorg.github.io/handycontrol"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <!-- HandyControl主题资源 -->
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- HandyControl主题 -->
                <hc:ThemeResources />
                <!-- HandyControl默认样式 -->
                <hc:Theme />
            </ResourceDictionary.MergedDictionaries>
            
            <!-- 自定义转换器 -->
            <local:DifferenceColorConverter x:Key="DifferenceColorConverter"/>
            <local:DifferenceBackgroundConverter x:Key="DifferenceBackgroundConverter"/>
            <local:HexModeConverter x:Key="HexModeConverter"/>
            <BooleanToVisibilityConverter x:Key="BooleanToVisibilityConverter"/>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

### 3. 项目文件更新

`.csproj`文件已自动更新，包含HandyControl依赖：

```xml
<ItemGroup>
    <PackageReference Include="HandyControl" Version="3.5.1" />
</ItemGroup>
```

## 配置详解

### 命名空间声明

```xml
xmlns:hc="https://handyorg.github.io/handycontrol"
```

- 前缀：`hc`
- 用途：访问HandyControls组件和资源

### 主题资源

#### ThemeResources

```xml
<hc:ThemeResources />
```

- 提供主题颜色定义
- 支持浅色/深色主题切换
- 定义系统颜色变量

#### Theme

```xml
<hc:Theme />
```

- 提供默认控件样式
- 包括按钮、文本框、列表等样式
- 确保UI一致性

## 可用组件

### 常用控件

| 组件 | 说明 | 用途 |
|------|------|------|
| `hc:Button` | 增强按钮 | 替代标准Button |
| `hc:TextBox` | 增强文本框 | 替代标准TextBox |
| `hc:ComboBox` | 增强下拉框 | 替代标准ComboBox |
| `hc:MessageBox` | 消息框 | 显示对话框 |
| `hc:Loading` | 加载动画 | 显示加载状态 |
| `hc:Notification` | 通知 | 显示通知消息 |

### 高级功能

- 主题切换
- 动画效果
- 自定义样式
- 国际化支持

## 使用示例

### 在XAML中使用HandyControls按钮

```xml
<hc:Button Content="确定" 
          Width="100" Height="32" 
          Foreground="White"
          Background="{DynamicResource PrimaryBrush}"/>
```

### 显示通知

```csharp
HandyControl.Controls.Growl.Success("操作成功！");
HandyControl.Controls.Growl.Error("操作失败！");
HandyControl.Controls.Growl.Warning("警告信息");
HandyControl.Controls.Growl.Info("提示信息");
```

### 显示消息框

```csharp
HandyControl.Controls.MessageBox.Show("确定删除？", "确认", 
    MessageBoxButton.YesNo, MessageBoxImage.Question);
```

## 主题定制

### 浅色主题（默认）

```csharp
// 设置为浅色主题
HandyControl.Themes.ThemeManager.Current.UsingSystemTheme = false;
HandyControl.Themes.ThemeManager.Current.ApplicationTheme = 
    HandyControl.Themes.ApplicationTheme.Light;
```

### 深色主题

```csharp
// 设置为深色主题
HandyControl.Themes.ThemeManager.Current.UsingSystemTheme = false;
HandyControl.Themes.ThemeManager.Current.ApplicationTheme = 
    HandyControl.Themes.ApplicationTheme.Dark;
```

## 编译状态

✅ **编译成功**
- 编译错误：0个
- 编译警告：28个（可接受）
- 目标框架：.NET 8.0

## 项目结构

```
BinCompare/
├── App.xaml                 ← HandyControls主题配置
├── MainWindow.xaml          ← UI界面
├── Models/
├── ViewModels/
├── Services/
├── Converters/
└── BinCompare.csproj        ← HandyControl依赖
```

## 依赖关系

```
BinCompare
└── HandyControl 3.5.1
    ├── System.Windows.Interactivity
    └── System.Xaml
```

## 兼容性

| 框架 | 版本 | 状态 |
|------|------|------|
| .NET Framework | 4.5+ | ✅ 支持 |
| .NET Core | 3.1+ | ✅ 支持 |
| .NET 5+ | 5.0+ | ✅ 支持 |
| .NET 8.0 | 8.0 | ✅ 支持 |

## 常见问题

### Q: 如何更改主题颜色？

A: 在App.xaml中修改主题资源，或在代码中动态设置：

```csharp
HandyControl.Themes.ThemeManager.Current.PrimaryColor = 
    System.Windows.Media.Color.FromRgb(0, 120, 212);
```

### Q: 如何使用HandyControls的MessageBox？

A: 使用HandyControl命名空间中的MessageBox类：

```csharp
using HandyControl.Controls;

MessageBox.Show("消息内容", "标题");
```

### Q: 如何禁用HandyControls主题？

A: 从App.xaml中移除主题资源，使用标准WPF样式。

## 文件修改清单

| 文件 | 修改内容 | 行数 |
|------|---------|------|
| App.xaml | 添加HandyControls命名空间和主题 | +10 |
| BinCompare.csproj | 添加HandyControl NuGet依赖 | +1 |

## 下一步

### 可选优化

1. **集成通知系统**
   - 使用`Growl`显示操作反馈
   - 替代状态栏消息

2. **美化按钮**
   - 使用`hc:Button`替代标准Button
   - 应用HandyControls样式

3. **增强对话框**
   - 使用HandyControls MessageBox
   - 提供更好的用户体验

4. **主题切换**
   - 添加主题切换功能
   - 支持浅色/深色主题

## 参考资源

- **官方网站**：https://handyorg.github.io/handycontrol/
- **GitHub**：https://github.com/HandyOrg/HandyControl
- **NuGet**：https://www.nuget.org/packages/HandyControl/
- **文档**：https://handyorg.github.io/handycontrol/native_en/home

## 版本历史

| 版本 | 日期 | 说明 |
|------|------|------|
| 1.5.1 | 2025-11-22 | 集成HandyControls框架 |
| 1.5.0 | 2025-11-21 | 添加ASCII字符显示功能 |
| 1.4.0 | 2025-11-21 | 添加隐藏/显示差异信息功能 |
| 1.3.0 | 2025-11-21 | 添加可拖拽分隔线 |
| 1.2.0 | 2025-11-21 | 界面重新设计 |
| 1.1.0 | 2025-11-21 | 添加拖拽文件功能 |
| 1.0.0 | 2025-11-21 | 初始版本 |

## 总结

✅ **HandyControls已成功部署**

- NuGet包已安装（版本3.5.1）
- App.xaml已配置主题资源
- 项目编译成功
- 可以开始使用HandyControls组件

**准备就绪！** 🎊

---

**部署完成日期**：2025年11月22日  
**部署状态**：✅ 完成  
**下一步**：开始使用HandyControls组件美化UI
