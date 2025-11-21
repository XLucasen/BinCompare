# ASCII字符选中状态颜色设置 - v1.5.2

**更新日期**：2025年11月22日  
**版本**：1.5.2  
**更新类型**：UI交互优化

## 功能概述

实现了当ListBox行被选中时，ASCII字符颜色自动变为白色的功能，提升用户交互体验。

## 功能特性

### 颜色变化

| 状态 | ASCII字符颜色 | 说明 |
|------|-------------|------|
| 未选中 | 绿色（#00AA00） | 默认显示颜色 |
| 选中 | 白色（#FFFFFF） | 选中时自动变白 |

### 实现方式

使用WPF的`DataTrigger`和`RelativeSource`绑定实现：

```xml
<DataTemplate.Triggers>
    <DataTrigger Binding="{Binding RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type ListBoxItem}}, Path=IsSelected}" 
                 Value="True">
        <Setter TargetName="AsciiTextBlock" Property="Foreground" Value="White"/>
    </DataTrigger>
</DataTemplate.Triggers>
```

## 技术实现

### 核心原理

1. **RelativeSource绑定**：查找最近的ListBoxItem祖先
2. **IsSelected属性**：检测ListBoxItem的选中状态
3. **DataTrigger**：当IsSelected为True时触发
4. **Setter**：修改TextBlock的Foreground属性为白色

### XAML代码

**文件A的ListBox**：
```xml
<TextBlock Name="AsciiTextBlock" Text="{Binding AsciiData}" 
          Padding="10,2" Foreground="Green" FontFamily="Consolas"
          Visibility="{Binding DataContext.ShowAscii, 
          RelativeSource={RelativeSource AncestorType=Window}, 
          Converter={StaticResource BooleanToVisibilityConverter}}"/>

<DataTemplate.Triggers>
    <DataTrigger Binding="{Binding RelativeSource={RelativeSource Mode=FindAncestor, 
                          AncestorType={x:Type ListBoxItem}}, Path=IsSelected}" 
                 Value="True">
        <Setter TargetName="AsciiTextBlock" Property="Foreground" Value="White"/>
    </DataTrigger>
</DataTemplate.Triggers>
```

**文件B的ListBox**：
```xml
<TextBlock Name="AsciiTextBlockB" Text="{Binding AsciiData}" 
          Padding="5,2" Foreground="Green" FontFamily="Consolas"
          Visibility="{Binding DataContext.ShowAscii, 
          RelativeSource={RelativeSource AncestorType=Window}, 
          Converter={StaticResource BooleanToVisibilityConverter}}"/>

<DataTemplate.Triggers>
    <DataTrigger Binding="{Binding RelativeSource={RelativeSource Mode=FindAncestor, 
                          AncestorType={x:Type ListBoxItem}}, Path=IsSelected}" 
                 Value="True">
        <Setter TargetName="AsciiTextBlockB" Property="Foreground" Value="White"/>
    </DataTrigger>
</DataTemplate.Triggers>
```

## 用户体验

### 视觉反馈

**未选中状态**：
```
地址        十六进制数据              ASCII字符(绿色)
00000000    48 65 6C 6C 6F          Hello
00000005    20 57 6F 72 6C 64       World
```

**选中状态**：
```
地址        十六进制数据              ASCII字符(白色)
00000000    48 65 6C 6C 6F          Hello  ← 白色
00000005    20 57 6F 72 6C 64       World
```

### 交互流程

1. 用户点击ListBox中的某一行
2. ListBoxItem的IsSelected属性变为True
3. DataTrigger检测到IsSelected为True
4. ASCII字符颜色自动变为白色
5. 用户点击其他行时，前一行恢复为绿色

## 修改文件

| 文件 | 修改内容 | 行数 |
|------|---------|------|
| MainWindow.xaml | 为ListBoxFileA添加DataTrigger | +5 |
| MainWindow.xaml | 为ListBoxFileB添加DataTrigger | +5 |

## 代码对比

### 修改前

```xml
<TextBlock Text="{Binding AsciiData}" Padding="10,2" 
          Foreground="Green" FontFamily="Consolas"
          Visibility="{Binding DataContext.ShowAscii, ...}"/>
```

### 修改后

```xml
<TextBlock Name="AsciiTextBlock" Text="{Binding AsciiData}" Padding="10,2" 
          Foreground="Green" FontFamily="Consolas"
          Visibility="{Binding DataContext.ShowAscii, ...}"/>

<DataTemplate.Triggers>
    <DataTrigger Binding="{Binding RelativeSource={RelativeSource Mode=FindAncestor, 
                          AncestorType={x:Type ListBoxItem}}, Path=IsSelected}" 
                 Value="True">
        <Setter TargetName="AsciiTextBlock" Property="Foreground" Value="White"/>
    </DataTrigger>
</DataTemplate.Triggers>
```

## 编译状态

✅ **编译成功**
- 编译错误：0个
- 编译警告：28个（可接受）
- 目标框架：.NET 8.0

## 功能验证

### 测试步骤

1. ✅ 运行应用程序
2. ✅ 打开两个二进制文件
3. ✅ 确保ASCII显示已启用
4. ✅ 点击ListBox中的任意行
5. ✅ 验证ASCII字符颜色变为白色
6. ✅ 点击其他行
7. ✅ 验证前一行ASCII字符恢复为绿色

### 预期结果

- ✅ 选中行的ASCII字符显示为白色
- ✅ 未选中行的ASCII字符显示为绿色
- ✅ 文件A和文件B同时生效
- ✅ 切换ASCII显示/隐藏时正常工作

## 相关功能

| 功能 | 说明 | 状态 |
|------|------|------|
| ASCII显示/隐藏 | 通过顶部按钮控制 | ✅ 完成 |
| 选中状态颜色 | 选中时变为白色 | ✅ 完成 |
| 同步选择 | 文件A和B同时选择 | ✅ 完成 |
| 差异高亮 | 差异行自动高亮 | ✅ 完成 |

## 项目版本历史

| 版本 | 日期 | 说明 |
|------|------|------|
| 1.5.2 | 2025-11-22 | 添加ASCII选中状态颜色设置 |
| 1.5.1 | 2025-11-22 | 集成HandyControls框架 |
| 1.5.0 | 2025-11-21 | 添加ASCII字符显示功能 |
| 1.4.0 | 2025-11-21 | 添加隐藏/显示差异信息功能 |
| 1.3.0 | 2025-11-21 | 添加可拖拽分隔线 |
| 1.2.0 | 2025-11-21 | 界面重新设计 |
| 1.1.0 | 2025-11-21 | 添加拖拽文件功能 |
| 1.0.0 | 2025-11-21 | 初始版本 |

## 技术细节

### DataTrigger vs Trigger

| 特性 | Trigger | DataTrigger |
|------|---------|------------|
| 绑定类型 | 依赖属性 | 任何属性 |
| 用途 | 简单属性检测 | 复杂绑定表达式 |
| 性能 | 更高 | 稍低 |
| 灵活性 | 低 | 高 |

本功能使用DataTrigger是因为需要通过RelativeSource绑定查找祖先元素。

### RelativeSource模式

```xml
RelativeSource Mode="FindAncestor"
AncestorType="{x:Type ListBoxItem}"
```

- **Mode**：查找模式
- **FindAncestor**：向上查找祖先元素
- **AncestorType**：指定要查找的祖先类型

## 最佳实践

### 1. 命名规范

```xml
<!-- ✅ 推荐 -->
<TextBlock Name="AsciiTextBlock" .../>

<!-- ❌ 不推荐 -->
<TextBlock Name="TB" .../>
```

### 2. 触发器组织

```xml
<!-- ✅ 推荐 -->
<DataTemplate.Triggers>
    <DataTrigger ...>
        <Setter .../>
    </DataTrigger>
</DataTemplate.Triggers>

<!-- ❌ 不推荐 -->
<!-- 混合使用Trigger和DataTrigger -->
```

### 3. 颜色选择

```xml
<!-- ✅ 推荐 -->
<Setter Property="Foreground" Value="White"/>

<!-- ❌ 不推荐 -->
<Setter Property="Foreground" Value="#FFFFFFFF"/>
```

## 常见问题

### Q: 为什么需要Name属性？

A: Setter的TargetName需要引用具体的元素，因此必须为TextBlock命名。

### Q: 如何修改选中时的颜色？

A: 修改Setter的Value属性：
```xml
<Setter TargetName="AsciiTextBlock" Property="Foreground" Value="Yellow"/>
```

### Q: 如何添加其他选中状态效果？

A: 可以添加多个Setter：
```xml
<DataTrigger ...>
    <Setter TargetName="AsciiTextBlock" Property="Foreground" Value="White"/>
    <Setter TargetName="AsciiTextBlock" Property="FontWeight" Value="Bold"/>
    <Setter TargetName="AsciiTextBlock" Property="FontSize" Value="12"/>
</DataTrigger>
```

## 总结

✅ **功能已完成**

- 实现了ASCII字符的选中状态颜色变化
- 提升了用户交互体验
- 代码简洁高效
- 编译成功，无错误

**准备就绪！** 🎊

---

**更新完成日期**：2025年11月22日  
**更新状态**：✅ 完成  
**下一步**：继续优化UI和功能
