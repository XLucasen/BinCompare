# 字节级高亮功能快速参考

## 功能概述

✅ **单字节差异显示**：只有差异的字节显示为浅红色
✅ **点击高亮**：点击差异列表时，对应字节以黄色临时高亮
✅ **自动取消**：2秒后自动取消高亮效果

## 颜色说明

| 状态 | 背景色 | 文字色 | RGB值 |
|------|--------|--------|--------|
| 正常字节 | 白色 | 黑色 | (255,255,255) / (0,0,0) |
| 差异字节 | 浅红色 | 红色 | (255,200,200) / (209,52,56) |
| 高亮字节 | 黄色 | 黑色 | (255,255,0) / (0,0,0) |

## 使用流程

```
1. 加载文件A和文件B
   ↓
2. 自动对比，显示差异
   ↓
3. 观察左侧显示区
   - 差异字节：浅红色背景
   - 正常字节：白色背景
   ↓
4. 点击右侧差异列表项
   ↓
5. 对应字节高亮为黄色（2秒）
   ↓
6. 自动恢复为浅红色
```

## 代码修改总结

### 关键类和方法

#### ByteSegment（Models/BinaryFileData.cs）
```csharp
public class ByteSegment : INotifyPropertyChanged
{
    public string Text { get; set; }           // 字节文本
    public bool IsDifference { get; set; }     // 是否为差异
    public bool IsHighlighted { get; set; }    // 是否为高亮（新增）
}
```

#### 转换器（Converters/DifferenceColorConverter.cs）
```csharp
// 背景色转换器（IMultiValueConverter）
public class ByteSegmentBackgroundConverter : IMultiValueConverter
{
    // 优先级：IsHighlighted > IsDifference > 正常
}

// 前景色转换器（IMultiValueConverter）
public class ByteSegmentForegroundConverter : IMultiValueConverter
{
    // 优先级：IsHighlighted > IsDifference > 正常
}
```

#### 主窗口（MainWindow.xaml.cs）
```csharp
private void ViewModel_JumpToDifferenceRequested(object sender, DifferenceInfo diff)
{
    // 1. 计算字节位置
    int byteIndexInRow = (int)(diff.ByteOffset % _viewModel.BytesPerRow);
    
    // 2. 清除之前的高亮
    ClearPreviousHighlight();
    
    // 3. 设置新的高亮
    row.ByteSegments[byteIndexInRow].IsHighlighted = true;
    
    // 4. 启动计时器（2秒后自动取消）
    _highlightTimer.Start();
}
```

## 配置修改

### 修改高亮时间
文件：`MainWindow.xaml.cs`
```csharp
// 第32行，改为需要的毫秒数
_highlightTimer = new System.Timers.Timer(2000); // 改为其他值，如 3000 = 3秒
```

### 修改高亮颜色
文件：`Converters/DifferenceColorConverter.cs`
```csharp
// ByteSegmentBackgroundConverter 中
if (isHighlighted)
{
    return new SolidColorBrush(Color.FromRgb(255, 255, 0)); // 改为其他RGB值
}
```

## 编译和运行

```bash
# 编译
dotnet build

# 运行
dotnet run

# 发布
dotnet publish -c Release
```

## 测试检查清单

- [ ] 加载两个不同的二进制文件
- [ ] 验证差异字节显示为浅红色
- [ ] 点击差异列表项，验证字节高亮为黄色
- [ ] 等待2秒，验证高亮自动取消
- [ ] 快速点击多个差异项，验证高亮正确切换
- [ ] 切换显示模式（十六进制/二进制），验证高亮功能正常

## 常见问题

**Q: 高亮没有显示？**
A: 检查是否点击了差异列表中的项目。确保差异列表不为空。

**Q: 高亮时间太短/太长？**
A: 修改 `MainWindow.xaml.cs` 第32行的计时器间隔。

**Q: 高亮颜色看不清？**
A: 修改转换器中的 RGB 值，选择对比度更高的颜色。

**Q: 为什么有些字节没有高亮？**
A: 检查字节是否确实有差异。只有差异字节才会被高亮。
