# 字节级差异高亮功能测试指南

## 功能说明

本次更新实现了**单字节差异高亮显示**功能，使用户能够快速定位和识别具体的差异字节。

## 核心改进

### 1. 差异字节的精确显示
- **之前**：整行变色，无法区分哪个字节有差异
- **现在**：只有差异的字节显示为浅红色背景 + 红色文字

### 2. 点击差异列表时的临时高亮
- 点击右侧"差异信息"列表中的任意差异项
- 对应的差异字节会以**黄色背景 + 黑色文字**临时高亮显示
- 2秒后自动取消高亮

## 测试步骤

### 前置条件
- 编译成功：`dotnet build`
- 运行应用：`dotnet run`

### 测试场景 1：查看差异字节
1. 加载两个不同的二进制文件（文件A和文件B）
2. 观察左侧显示区域
3. **验证**：有差异的字节应该显示为浅红色背景，其他字节为白色背景

### 测试场景 2：点击差异列表进行高亮
1. 在右侧"差异信息"列表中点击任意一个差异项
2. **验证**：
   - 左侧对应的差异字节变为黄色背景
   - 两个文件的对应字节都被高亮
   - 2秒后自动取消黄色高亮，恢复为浅红色

### 测试场景 3：快速点击多个差异
1. 快速点击右侧列表中的多个差异项
2. **验证**：
   - 每次点击时，之前的高亮被清除
   - 新的差异字节被高亮显示
   - 计时器被重置，2秒计时重新开始

## 实现细节

### 修改的文件

#### 1. Models/BinaryFileData.cs
- `ByteSegment` 类：
  - 添加 `IsHighlighted` 属性（bool）
  - 实现 `INotifyPropertyChanged` 接口
  - 支持属性变更通知

#### 2. Services/BinaryCompareService.cs
- `GenerateHexRows()` 和 `GenerateBinaryRows()`：
  - 为每个字节创建 `ByteSegment` 对象
  - 初始化 `IsDifference` 为 false
  
- `MarkDifferences()`：
  - 标记有差异的字节段
  - 设置 `ByteSegment.IsDifference = true`

#### 3. Converters/DifferenceColorConverter.cs
- `ByteSegmentBackgroundConverter`：
  - 改为 `IMultiValueConverter`
  - 支持 `IsDifference` 和 `IsHighlighted` 两个绑定值
  - 优先级：高亮 > 差异 > 正常
  
- `ByteSegmentForegroundConverter`：
  - 改为 `IMultiValueConverter`
  - 同样支持两个绑定值

#### 4. MainWindow.xaml
- 文件A和文件B的 ListBox 模板：
  - 将整行数据改为 `ItemsControl` + `ByteSegments` 集合
  - 使用 `MultiBinding` 绑定转换器
  - 每个字节段单独渲染

#### 5. MainWindow.xaml.cs
- 添加 `_highlightTimer` 计时器
- 修改 `ViewModel_JumpToDifferenceRequested()` 方法：
  - 计算差异字节位置
  - 清除之前的高亮
  - 设置新的高亮
  - 启动自动取消计时器
  
- 添加辅助方法：
  - `ClearPreviousHighlight()`：清除所有高亮
  - `HighlightTimer_Elapsed()`：处理计时器事件

## 性能考虑

- 每行最多显示 64 个字节，每个字节一个 `ByteSegment` 对象
- 高亮操作只修改 `IsHighlighted` 属性，通过数据绑定更新 UI
- 计时器使用 2 秒延迟，自动清除高亮状态

## 已知限制

- 高亮时间固定为 2 秒（可在代码中修改 `_highlightTimer` 的间隔）
- 高亮颜色固定为黄色（可在转换器中修改 RGB 值）

## 未来改进方向

1. 添加配置选项，允许用户自定义高亮时间和颜色
2. 添加键盘快捷键快速导航差异
3. 添加"上一个差异"和"下一个差异"按钮
4. 支持多选差异进行批量操作
