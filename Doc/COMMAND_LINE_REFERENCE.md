# 命令行参数参考指南

## 快速参考

| 用法 | 说明 | 示例 |
|------|------|------|
| `BinCompare.exe` | 启动程序（无文件） | `BinCompare.exe` |
| `BinCompare.exe <file>` | 加载单个文件到文件A | `BinCompare.exe file.bin` |
| `BinCompare.exe <file1> <file2>` | 加载两个文件进行对比 | `BinCompare.exe a.bin b.bin` |

## 详细说明

### 1. 无参数启动
```bash
BinCompare.exe
```
- 启动程序，显示空白界面
- 用户可以手动选择文件进行对比

### 2. 单文件启动
```bash
BinCompare.exe "C:\path\to\file.bin"
```
- 启动程序并自动加载文件到**文件A**
- 用户可以手动加载文件B进行对比
- 如果文件路径包含空格，需要用引号括起来

### 3. 双文件启动（自动对比）
```bash
BinCompare.exe "C:\path\to\fileA.bin" "C:\path\to\fileB.bin"
```
- 启动程序并自动加载两个文件
- 自动进行对比，显示所有差异
- 这是最常用的用法

### 4. 多文件启动（只使用前两个）
```bash
BinCompare.exe file1.bin file2.bin file3.bin file4.bin
```
- 只有前两个文件会被加载
- `file1.bin` → 文件A
- `file2.bin` → 文件B
- `file3.bin` 和 `file4.bin` 被忽略

## 实际应用示例

### 示例 1：对比两个固件文件
```bash
BinCompare.exe "C:\firmware\v1.0.bin" "C:\firmware\v1.1.bin"
```

### 示例 2：对比下载的文件和本地文件
```bash
BinCompare.exe "C:\Downloads\downloaded.bin" "C:\backup\original.bin"
```

### 示例 3：对比内存转储文件
```bash
BinCompare.exe "C:\dumps\dump1.bin" "C:\dumps\dump2.bin"
```

## 文件路径处理

### 相对路径
```bash
# 当前目录的文件
BinCompare.exe file.bin

# 子目录的文件
BinCompare.exe data\file.bin

# 父目录的文件
BinCompare.exe ..\file.bin
```

### 绝对路径
```bash
# Windows 绝对路径
BinCompare.exe "C:\Users\Username\Documents\file.bin"

# UNC 网络路径
BinCompare.exe "\\server\share\file.bin"
```

### 特殊字符处理
```bash
# 路径包含空格（使用引号）
BinCompare.exe "C:\My Documents\file.bin"

# 路径包含特殊字符（使用引号）
BinCompare.exe "C:\data\file (1).bin"
```

## 批处理脚本示例

### 简单批处理
```batch
@echo off
REM 对比两个文件
BinCompare.exe "C:\data\file1.bin" "C:\data\file2.bin"
```

### 带错误处理的批处理
```batch
@echo off
setlocal enabledelayedexpansion

set FILE1=C:\data\file1.bin
set FILE2=C:\data\file2.bin

if not exist "%FILE1%" (
    echo 错误: 文件1不存在
    exit /b 1
)

if not exist "%FILE2%" (
    echo 错误: 文件2不存在
    exit /b 1
)

BinCompare.exe "%FILE1%" "%FILE2%"
```

### 对比多个文件对
```batch
@echo off
REM 对比多个文件对
BinCompare.exe "C:\data\v1.bin" "C:\data\v2.bin"
timeout /t 5
BinCompare.exe "C:\data\v2.bin" "C:\data\v3.bin"
```

## PowerShell 脚本示例

### 基本用法
```powershell
# 启动程序并加载两个文件
& "C:\Program Files\BinCompare\BinCompare.exe" "C:\data\file1.bin" "C:\data\file2.bin"
```

### 带参数验证
```powershell
param(
    [string]$File1,
    [string]$File2
)

if (-not (Test-Path $File1)) {
    Write-Error "文件1不存在: $File1"
    exit 1
}

if (-not (Test-Path $File2)) {
    Write-Error "文件2不存在: $File2"
    exit 1
}

& "C:\Program Files\BinCompare\BinCompare.exe" $File1 $File2
```

### 对比文件夹中的所有文件
```powershell
$folder = "C:\data"
$files = Get-ChildItem -Path $folder -Filter "*.bin" | Sort-Object Name

for ($i = 0; $i -lt $files.Count - 1; $i++) {
    $file1 = $files[$i].FullName
    $file2 = $files[$i + 1].FullName
    
    Write-Host "对比: $($files[$i].Name) vs $($files[$i + 1].Name)"
    & "C:\Program Files\BinCompare\BinCompare.exe" $file1 $file2
    
    Read-Host "按 Enter 继续..."
}
```

## 拖拽文件到程序图标

### 工作原理
当拖拽文件到程序图标时，Windows 会自动调用：
```bash
BinCompare.exe "C:\path\to\file1.bin" "C:\path\to\file2.bin"
```

### 支持的拖拽方式
- 拖拽单个文件 → 加载到文件A
- 拖拽两个文件 → 分别加载到文件A和文件B
- 拖拽多个文件 → 只使用前两个

## 环境变量

### 添加到 PATH（可选）
```batch
REM 添加 BinCompare 到 PATH
setx PATH "%PATH%;C:\Program Files\BinCompare"

REM 之后可以直接使用
BinCompare.exe file1.bin file2.bin
```

### 创建快捷方式
```batch
REM 创建快捷方式脚本
powershell -Command "$WshShell = New-Object -ComObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('$env:USERPROFILE\Desktop\BinCompare.lnk'); $Shortcut.TargetPath = 'C:\Program Files\BinCompare\BinCompare.exe'; $Shortcut.Save()"
```

## 错误处理

### 常见错误

| 错误 | 原因 | 解决方案 |
|------|------|---------|
| 文件不存在 | 指定的文件路径错误 | 检查文件路径是否正确 |
| 访问被拒绝 | 没有文件读取权限 | 检查文件权限 |
| 无效的文件格式 | 文件不是有效的二进制文件 | 确保文件是二进制格式 |

### 调试技巧
```batch
@echo off
REM 启用调试输出
echo 启动 BinCompare...
echo 文件1: %1
echo 文件2: %2

BinCompare.exe "%1" "%2"

if errorlevel 1 (
    echo 程序执行失败
    pause
)
```

## 性能提示

- 大文件加载可能需要几秒钟
- 对比大文件可能需要更长时间
- 建议在 SSD 上运行以获得最佳性能

## 兼容性

- **操作系统**：Windows 7 及以上
- **文件系统**：NTFS, FAT32, exFAT 等
- **文件大小**：无限制（受系统内存限制）

---

**版本**：v1.6.0+  
**最后更新**：2025-11-22
