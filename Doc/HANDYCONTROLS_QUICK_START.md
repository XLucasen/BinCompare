# HandyControls 快速开始指南

## 安装状态

✅ **已完成**
- HandyControl 3.5.1 已安装
- App.xaml 已配置
- 项目编译成功

## 快速使用

### 1. 在XAML中使用HandyControls控件

```xml
<!-- 添加命名空间 -->
xmlns:hc="https://handyorg.github.io/handycontrol"

<!-- 使用HandyControls按钮 -->
<hc:Button Content="点击我" 
          Width="100" Height="32"
          Foreground="White"
          Background="{DynamicResource PrimaryBrush}"/>

<!-- 使用HandyControls文本框 -->
<hc:TextBox Watermark="请输入内容" Width="200" Height="32"/>

<!-- 使用HandyControls下拉框 -->
<hc:ComboBox ItemsSource="{Binding Items}" 
            SelectedItem="{Binding SelectedItem}"/>
```

### 2. 在C#中使用HandyControls

```csharp
using HandyControl.Controls;
using HandyControl.Themes;

// 显示成功通知
Growl.Success("操作成功！");

// 显示错误通知
Growl.Error("操作失败！");

// 显示警告通知
Growl.Warning("请注意！");

// 显示信息通知
Growl.Info("提示信息");

// 显示消息框
MessageBox.Show("确定删除？", "确认", 
    MessageBoxButton.YesNo, MessageBoxImage.Question);
```

### 3. 主题切换

```csharp
// 设置为浅色主题
ThemeManager.Current.UsingSystemTheme = false;
ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;

// 设置为深色主题
ThemeManager.Current.UsingSystemTheme = false;
ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;

// 使用系统主题
ThemeManager.Current.UsingSystemTheme = true;
```

### 4. 自定义颜色

```csharp
// 修改主色调
ThemeManager.Current.PrimaryColor = 
    System.Windows.Media.Color.FromRgb(0, 120, 212);

// 修改强调色
ThemeManager.Current.AccentColor = 
    System.Windows.Media.Color.FromRgb(255, 100, 0);
```

## 常用控件列表

| 控件 | 说明 | 示例 |
|------|------|------|
| `hc:Button` | 按钮 | `<hc:Button Content="确定"/>` |
| `hc:TextBox` | 文本框 | `<hc:TextBox Watermark="输入"/>` |
| `hc:ComboBox` | 下拉框 | `<hc:ComboBox ItemsSource="{Binding Items}"/>` |
| `hc:CheckBox` | 复选框 | `<hc:CheckBox Content="同意"/>` |
| `hc:RadioButton` | 单选框 | `<hc:RadioButton Content="选项"/>` |
| `hc:Slider` | 滑块 | `<hc:Slider Minimum="0" Maximum="100"/>` |
| `hc:ProgressBar` | 进度条 | `<hc:ProgressBar Value="50"/>` |
| `hc:Loading` | 加载动画 | `<hc:Loading IsRunning="True"/>` |
| `hc:Notification` | 通知 | 代码中使用 |
| `hc:MessageBox` | 消息框 | 代码中使用 |

## 常用资源

### 颜色资源

```xml
<!-- 主色调 -->
<SolidColorBrush x:Key="PrimaryBrush" Color="{DynamicResource Primary}"/>

<!-- 强调色 -->
<SolidColorBrush x:Key="AccentBrush" Color="{DynamicResource Accent}"/>

<!-- 前景色 -->
<SolidColorBrush x:Key="ForegroundBrush" Color="{DynamicResource Foreground}"/>

<!-- 背景色 -->
<SolidColorBrush x:Key="BackgroundBrush" Color="{DynamicResource Background}"/>
```

### 使用示例

```xml
<Button Background="{DynamicResource PrimaryBrush}" 
       Foreground="{DynamicResource ForegroundBrush}"/>
```

## 通知系统

### 显示通知

```csharp
// 成功
Growl.Success("文件已保存");

// 错误
Growl.Error("保存失败");

// 警告
Growl.Warning("即将超时");

// 信息
Growl.Info("正在处理中");
```

### 自定义通知

```csharp
Growl.Ask("确定删除吗？", isConfirmed =>
{
    if (isConfirmed)
    {
        // 执行删除操作
        Growl.Success("已删除");
    }
});
```

## 对话框

### 消息框

```csharp
// 简单消息
MessageBox.Show("操作完成");

// 带标题的消息
MessageBox.Show("确定删除？", "确认");

// 带按钮的消息
var result = MessageBox.Show("确定删除？", "确认", 
    MessageBoxButton.YesNo, MessageBoxImage.Question);

if (result == MessageBoxResult.Yes)
{
    // 执行删除
}
```

### 输入框

```csharp
// 显示输入对话框
var input = InputBox.Show("请输入文件名", "新建文件");
if (!string.IsNullOrEmpty(input))
{
    // 处理输入
}
```

## 样式定制

### 修改按钮样式

```xml
<hc:Button Content="自定义按钮"
          Width="100" Height="32"
          Foreground="White"
          Background="{DynamicResource PrimaryBrush}"
          hc:BorderElement.CornerRadius="4"/>
```

### 修改文本框样式

```xml
<hc:TextBox Watermark="请输入内容"
           Width="200" Height="32"
           hc:BorderElement.CornerRadius="4"
           Padding="10,0"/>
```

## 动画效果

### 加载动画

```xml
<hc:Loading IsRunning="True" 
           Width="50" Height="50"/>
```

### 过渡动画

```xml
<hc:TransitioningContentControl Transition="LeftReplace">
    <TextBlock Text="动画内容"/>
</hc:TransitioningContentControl>
```

## 最佳实践

### 1. 使用动态资源

```xml
<!-- ✅ 推荐 -->
<Button Background="{DynamicResource PrimaryBrush}"/>

<!-- ❌ 不推荐 -->
<Button Background="#0078D4"/>
```

### 2. 使用主题颜色

```csharp
// ✅ 推荐
var color = (Color)Application.Current.Resources["Primary"];

// ❌ 不推荐
var color = Color.FromRgb(0, 120, 212);
```

### 3. 响应式设计

```xml
<!-- 使用相对大小 -->
<Button Width="100" Height="32"/>

<!-- 使用自动大小 -->
<Button Padding="20,10" Content="自适应"/>
```

## 常见问题

### Q: 如何改变主题？
A: 使用`ThemeManager.Current.ApplicationTheme`属性。

### Q: 如何自定义颜色？
A: 修改`ThemeManager.Current.PrimaryColor`或在App.xaml中定义资源。

### Q: 如何隐藏通知？
A: 通知会自动消失，或使用`Growl.Clear()`手动清除。

### Q: 如何使用HandyControls的样式？
A: 在App.xaml中已自动加载，直接使用即可。

## 相关文档

- [官方文档](https://handyorg.github.io/handycontrol/)
- [GitHub仓库](https://github.com/HandyOrg/HandyControl)
- [NuGet包](https://www.nuget.org/packages/HandyControl/)

## 下一步

1. 在MainWindow.xaml中使用HandyControls控件
2. 集成通知系统替代状态栏
3. 美化按钮和对话框
4. 实现主题切换功能

---

**准备就绪！开始使用HandyControls吧！** 🚀
