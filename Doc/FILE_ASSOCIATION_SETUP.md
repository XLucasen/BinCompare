# BinCompare 文件关联设置指南

## 问题说明

拖拽文件到程序图标时，程序没有自动打开。这是因为 Windows 需要知道如何处理这些文件类型。

## 解决方案

### 方案 1：自动注册（推荐）

#### 使用 PowerShell 脚本（推荐）

**步骤**：
1. 确保 BinCompare 已安装到 `C:\Program Files\BinCompare\`
2. 右键点击 `RegisterFileAssociation.ps1`
3. 选择 "用 PowerShell 运行"
4. 如果出现权限提示，选择 "是"
5. 等待脚本完成

**或者通过命令行**：
```powershell
# 以管理员身份打开 PowerShell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
& "C:\path\to\RegisterFileAssociation.ps1"
```

#### 使用批处理脚本

**步骤**：
1. 确保 BinCompare 已安装到 `C:\Program Files\BinCompare\`
2. 右键点击 `RegisterFileAssociation.bat`
3. 选择 "以管理员身份运行"
4. 等待脚本完成

#### 使用注册表文件

**步骤**：
1. 编辑 `RegisterFileAssociation.reg` 文件
2. 将所有 `C:\Program Files\BinCompare\BinCompare.exe` 替换为实际的程序路径
3. 保存文件
4. 双击 `.reg` 文件
5. 点击 "是" 确认导入

### 方案 2：手动注册

#### 通过 Windows 设置（Windows 10/11）

**步骤**：
1. 打开 "设置" → "应用" → "默认应用"
2. 向下滚动，点击 "按文件类型选择默认应用"
3. 找到 `.bin` 文件类型
4. 点击，选择 "BinCompare" 或 "浏览"
5. 选择 `BinCompare.exe`
6. 对其他文件类型（`.dat`, `.hex`, `.rom`, `.img`）重复

#### 通过文件管理器右键菜单

**步骤**：
1. 右键点击任意 `.bin` 文件
2. 选择 "打开方式" → "选择其他应用"
3. 找到 BinCompare，勾选 "始终使用此应用打开 .bin 文件"
4. 点击 "确定"
5. 对其他文件类型重复

#### 通过注册表编辑器（高级）

**步骤**：
1. 按 `Win+R`，输入 `regedit`，按 Enter
2. 导航到 `HKEY_CLASSES_ROOT\.bin`
3. 右键点击，选择 "新建" → "字符串值"
4. 输入 `BinCompare.BinaryFile`
5. 导航到 `HKEY_CLASSES_ROOT\BinCompare.BinaryFile\shell\open\command`
6. 修改默认值为：`"C:\Program Files\BinCompare\BinCompare.exe" "%1"`
7. 对其他文件类型重复

### 方案 3：使用 Windows 关联工具

#### 使用第三方工具

推荐使用以下工具：
- **FileTypesMan** - 免费的文件类型管理工具
- **SetUserFTA** - 命令行文件关联工具
- **Windows 11 Default Apps** - Windows 11 内置工具

## 支持的文件类型

| 扩展名 | 说明 | 用途 |
|--------|------|------|
| `.bin` | 二进制文件 | 通用二进制格式 |
| `.dat` | 数据文件 | 通用数据格式 |
| `.hex` | 十六进制文件 | 十六进制转储 |
| `.rom` | ROM 文件 | 固件/BIOS 文件 |
| `.img` | 镜像文件 | 磁盘/分区镜像 |

## 注册后的功能

### 拖拽文件
```
拖拽 file.bin 到 BinCompare 程序图标 → 程序启动并加载文件
```

### 右键菜单
```
右键点击 file.bin → "用 BinCompare 打开" → 程序启动并加载文件
```

### 双击打开
```
双击 file.bin → 程序启动并加载文件
```

### 命令行
```bash
BinCompare.exe "C:\path\to\file.bin"
```

## 故障排除

### 问题 1：拖拽仍然不工作

**原因**：
- 文件关联未正确注册
- 程序路径不正确
- Windows 缓存未更新

**解决方案**：
1. 重新运行注册脚本
2. 检查程序路径是否正确
3. 重启计算机
4. 清除 Windows 文件关联缓存

### 问题 2：右键菜单没有 "用 BinCompare 打开"

**原因**：
- 文件关联未正确注册
- 注册表项不完整

**解决方案**：
1. 使用 PowerShell 脚本重新注册
2. 检查注册表中的 ProgID 是否正确
3. 确保 `shell\open\command` 项存在

### 问题 3：双击文件打开了其他程序

**原因**：
- 其他程序的文件关联优先级更高
- 文件关联设置被覆盖

**解决方案**：
1. 通过 Windows 设置更改默认应用
2. 重新运行注册脚本
3. 使用 FileTypesMan 等工具重新设置

### 问题 4：权限不足错误

**原因**：
- 脚本未以管理员身份运行
- 用户没有修改注册表的权限

**解决方案**：
1. 右键点击脚本，选择 "以管理员身份运行"
2. 使用管理员账户登录
3. 检查用户权限设置

## 高级配置

### 自定义文件类型描述

编辑 `RegisterFileAssociation.reg`：
```
[HKEY_CLASSES_ROOT\BinCompare.BinaryFile]
@="BinCompare 二进制文件"
```

### 自定义右键菜单文本

编辑 `RegisterFileAssociation.reg`：
```
[HKEY_CLASSES_ROOT\BinCompare.BinaryFile\shell\open]
@="用 BinCompare 打开"
```

### 添加更多文件类型

编辑 `RegisterFileAssociation.reg`，添加新的扩展名：
```
[HKEY_CLASSES_ROOT\.iso]
@="BinCompare.BinaryFile"
```

## 卸载文件关联

### 使用 PowerShell

```powershell
# 以管理员身份运行
$extensions = @(".bin", ".dat", ".hex", ".rom", ".img")
foreach ($ext in $extensions) {
    Remove-Item -Path "HKCU:\Software\Classes\$ext" -Force -ErrorAction SilentlyContinue
}
Remove-Item -Path "HKCU:\Software\Classes\BinCompare.BinaryFile" -Force -ErrorAction SilentlyContinue
```

### 使用注册表编辑器

1. 打开 `regedit`
2. 导航到 `HKEY_CLASSES_ROOT`
3. 删除 `.bin`, `.dat`, `.hex`, `.rom`, `.img` 项
4. 删除 `BinCompare.BinaryFile` 项

## 最佳实践

1. **安装前注册**
   - 在安装 BinCompare 之前注册文件关联

2. **使用标准路径**
   - 将 BinCompare 安装到 `C:\Program Files\BinCompare\`

3. **定期检查**
   - 定期检查文件关联是否仍然有效

4. **备份注册表**
   - 修改注册表前备份

5. **使用脚本**
   - 使用提供的脚本而不是手动编辑注册表

## 相关文件

- `RegisterFileAssociation.reg` - 注册表文件
- `RegisterFileAssociation.ps1` - PowerShell 脚本
- `RegisterFileAssociation.bat` - 批处理脚本
- `DRAG_DROP_GUIDE.md` - 拖拽功能使用指南
- `COMMAND_LINE_REFERENCE.md` - 命令行参数参考

## 常见问题

### Q: 为什么需要管理员权限？
A: 修改 Windows 注册表需要管理员权限。

### Q: 可以为多个用户注册吗？
A: 可以，但需要为每个用户分别注册（在 HKEY_CURRENT_USER 下）。

### Q: 注册后需要重启吗？
A: 通常不需要，但有时重启可以确保更改生效。

### Q: 可以注册其他文件类型吗？
A: 可以，编辑注册表文件添加新的扩展名即可。

### Q: 如何恢复默认关联？
A: 删除注册表中的相关项，或通过 Windows 设置重新设置。

---

**版本**：v1.6.0+  
**最后更新**：2025-11-22
