# 帮助窗口功能 - v1.5.3

**更新日期**：2025年11月22日  
**版本**：1.5.3  
**更新类型**：用户帮助系统

## 功能概述

实现了一个完整的帮助窗口系统，包含版本信息、作者信息、核心功能、快速开始、快捷键、使用提示和常见问题等内容。

## 功能特性

### 圆形帮助按钮

**位置**：顶部控制区右侧  
**样式**：圆形按钮，蓝色背景  
**大小**：36×36像素  
**图标**：问号（?）  
**提示**：鼠标悬停显示"帮助信息"

### 帮助窗口内容

| 章节 | 内容 |
|------|------|
| 版本信息 | 应用名称、版本号、开发框架、架构模式、发布日期 |
| 作者信息 | 开发者、项目类型、用途 |
| 核心功能 | 8项主要功能的详细说明 |
| 快速开始 | 4个步骤的使用指南 |
| 快捷键 | 5个常用快捷键 |
| 使用提示 | 5条实用建议 |
| 常见问题 | 4个常见问题的解答 |
| 版本历史 | 9个版本的发布记录 |

## 技术实现

### 圆形按钮样式

**App.xaml中的样式定义**：
```xml
<Style x:Key="CircleButtonStyle" TargetType="Button">
    <Setter Property="Width" Value="36"/>
    <Setter Property="Height" Value="36"/>
    <Setter Property="Foreground" Value="White"/>
    <Setter Property="FontSize" Value="18"/>
    <Setter Property="FontWeight" Value="Bold"/>
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="Button">
                <Grid>
                    <Ellipse Fill="{TemplateBinding Background}" 
                            Stroke="{TemplateBinding BorderBrush}" 
                            StrokeThickness="1"/>
                    <ContentPresenter HorizontalAlignment="Center" 
                                    VerticalAlignment="Center" 
                                    Content="{TemplateBinding Content}"/>
                </Grid>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
    <Style.Triggers>
        <Trigger Property="IsMouseOver" Value="True">
            <Setter Property="Background" Value="#005A9E"/>
        </Trigger>
        <Trigger Property="IsPressed" Value="True">
            <Setter Property="Background" Value="#003D6B"/>
        </Trigger>
    </Style.Triggers>
</Style>
```

### 帮助按钮XAML

**MainWindow.xaml**：
```xml
<Button Name="BtnHelp" Margin="5,0"
       Click="BtnHelp_Click"
       Background="#0078D4"
       ToolTip="帮助信息"
       Style="{StaticResource CircleButtonStyle}">
    ?
</Button>
```

### 事件处理

**MainWindow.xaml.cs**：
```csharp
private void BtnHelp_Click(object sender, RoutedEventArgs e)
{
    try
    {
        HelpWindow helpWindow = new HelpWindow();
        helpWindow.Owner = this;
        helpWindow.ShowDialog();
    }
    catch (Exception ex)
    {
        _viewModel.StatusMessage = $"打开帮助窗口失败: {ex.Message}";
    }
}
```

### 帮助窗口类

**HelpWindow.xaml.cs**：
```csharp
public partial class HelpWindow : Window
{
    public HelpWindow()
    {
        InitializeComponent();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
```

## 窗口设计

### 窗口属性

| 属性 | 值 |
|------|-----|
| 标题 | 帮助信息 |
| 宽度 | 700px |
| 高度 | 600px |
| 启动位置 | 中心 |
| 可调整大小 | 是 |
| 背景色 | #F5F5F5 |

### 布局结构

```
┌─────────────────────────────────────┐
│  标题栏（蓝色背景）                  │
│  二进制文件对比工具 - 帮助           │
├─────────────────────────────────────┤
│                                     │
│  内容区（可滚动）                    │
│  ├─ 版本信息                        │
│  ├─ 作者信息                        │
│  ├─ 核心功能                        │
│  ├─ 快速开始                        │
│  ├─ 快捷键                          │
│  ├─ 使用提示                        │
│  └─ 常见问题                        │
│                                     │
├─────────────────────────────────────┤
│  [关闭]                             │
└─────────────────────────────────────┘
```

## 帮助内容详解

### 版本信息

- 应用名称：二进制文件对比工具
- 版本号：v1.5.3
- 开发框架：WPF (.NET 8.0)
- 架构模式：MVVM
- 发布日期：2025年11月22日

### 作者信息

- 开发者：Lucas
- 项目类型：二进制文件对比工具
- 用途：快速对比两个二进制文件的差异

### 核心功能

1. **双文件对比** - 同时打开和对比两个二进制文件
2. **十六进制/二进制显示** - 支持两种显示模式切换
3. **ASCII字符显示** - 显示对应的ASCII字符
4. **差异高亮** - 自动检测并高亮显示差异字节
5. **可配置行长度** - 支持8/16/32/64字节每行
6. **差异导出** - 将差异信息导出为文本文件
7. **拖拽打开** - 支持拖拽文件到显示区打开
8. **可拖拽分隔线** - 调整文件显示区的宽度比例

### 快速开始

**1. 打开文件**
- 点击'选择文件A'和'选择文件B'按钮选择要对比的文件
- 或直接拖拽文件到对应的显示区域

**2. 查看对比结果**
- 差异字节会自动高亮显示（红色背景）
- 右侧差异信息区显示详细的差异位置和内容

**3. 调整显示**
- 使用'切换为'按钮切换十六进制/二进制显示
- 使用下拉框选择每行显示的字节数（8/16/32/64）
- 点击'隐藏ASCII'按钮显示/隐藏ASCII字符

**4. 导出结果**
- 点击'导出差异'按钮将差异信息保存为文本文件

### 快捷键

| 快捷键 | 功能 |
|--------|------|
| Ctrl+O | 打开文件A |
| Ctrl+Shift+O | 打开文件B |
| Ctrl+M | 切换显示模式（十六进制/二进制） |
| Ctrl+E | 导出差异信息 |
| Ctrl+C | 清除所有数据 |

### 使用提示

- 选中某一行时，ASCII字符会变为红色，便于识别
- 可以拖拽中间的分隔线调整文件A和文件B的显示宽度
- 差异信息区可以隐藏，为文件显示区腾出更多空间
- 支持大文件对比，但处理时间会相应增加
- 差异导出的文件包含地址、类型、文件A值、文件B值等信息

### 常见问题

**Q: 如何对比两个文件？**
A: 分别选择文件A和文件B，工具会自动进行对比。

**Q: 如何导出差异结果？**
A: 点击'导出差异'按钮，选择保存位置即可导出为文本文件。

**Q: 支持哪些文件格式？**
A: 支持所有二进制文件格式（.bin, .exe, .dll, 等）。

**Q: 如何调整显示比例？**
A: 拖拽中间的分隔线可以调整文件A和文件B的显示宽度。

### 版本历史

| 版本 | 日期 | 功能说明 |
|------|------|---------|
| v1.5.3 | 2025-11-22 | ✅ 帮助窗口功能（新增） |
| v1.5.2 | 2025-11-22 | ✅ ASCII选中状态颜色设置 |
| v1.5.1 | 2025-11-22 | ✅ HandyControls框架集成 |
| v1.5.0 | 2025-11-21 | ✅ ASCII字符显示功能 |
| v1.4.0 | 2025-11-21 | ✅ 隐藏/显示差异信息功能 |
| v1.3.0 | 2025-11-21 | ✅ 可拖拽分隔线功能 |
| v1.2.0 | 2025-11-21 | ✅ 界面重新设计 |
| v1.1.0 | 2025-11-21 | ✅ 拖拽文件功能 |
| v1.0.0 | 2025-11-21 | ✅ 初始版本发布 |

## 编译状态

✅ **编译成功**
- 编译错误：0个
- 编译警告：28个（可接受）
- 目标框架：.NET 8.0

## 文件清单

| 文件 | 说明 |
|------|------|
| HelpWindow.xaml | 帮助窗口UI定义 |
| HelpWindow.xaml.cs | 帮助窗口代码逻辑 |
| App.xaml | 圆形按钮样式定义 |
| MainWindow.xaml | 帮助按钮UI |
| MainWindow.xaml.cs | 帮助按钮事件处理 |

## 用户体验

### 打开帮助

1. 用户点击顶部右侧的圆形"?"按钮
2. 帮助窗口以对话框形式打开
3. 窗口在主窗口中央显示
4. 用户可以滚动查看所有帮助内容
5. 点击"关闭"按钮或关闭窗口即可退出

### 视觉反馈

- **默认状态**：蓝色圆形按钮
- **鼠标悬停**：深蓝色（#005A9E）
- **点击状态**：更深的蓝色（#003D6B）
- **提示文本**：鼠标悬停显示"帮助信息"

## 项目版本历史

| 版本 | 日期 | 说明 |
|------|------|------|
| 1.5.3 | 2025-11-22 | 添加帮助窗口功能 |
| 1.5.2 | 2025-11-22 | ASCII选中状态颜色设置 |
| 1.5.1 | 2025-11-22 | HandyControls框架集成 |
| 1.5.0 | 2025-11-21 | ASCII字符显示功能 |
| 1.4.0 | 2025-11-21 | 隐藏/显示差异信息功能 |
| 1.3.0 | 2025-11-21 | 可拖拽分隔线 |
| 1.2.0 | 2025-11-21 | 界面重新设计 |
| 1.1.0 | 2025-11-21 | 拖拽文件功能 |
| 1.0.0 | 2025-11-21 | 初始版本 |

## 扩展可能性

### 未来改进

1. **多语言支持** - 添加英文、日文等语言版本
2. **在线帮助** - 添加链接到在线文档
3. **视频教程** - 嵌入使用视频
4. **搜索功能** - 在帮助内容中搜索
5. **主题定制** - 支持不同的帮助窗口主题

## 总结

✅ **功能已完成**

- 实现了圆形帮助按钮
- 创建了完整的帮助窗口
- 包含详细的使用说明和常见问题
- 提供了良好的用户体验
- 编译成功，无错误

**准备就绪！** 🎊

---

**更新完成日期**：2025年11月22日  
**更新状态**：✅ 完成  
**下一步**：继续优化功能和UI
