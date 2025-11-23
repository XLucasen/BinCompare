# BinCompare 快速设置指南

## 🚀 快速开始（3 步）

### 步骤 1：安装程序
```bash
# 将 BinCompare 安装到标准位置
C:\Program Files\BinCompare\BinCompare.exe
```

### 步骤 2：注册文件关联
选择以下任意一种方法：

#### 方法 A：PowerShell（推荐）
```powershell
# 右键点击 RegisterFileAssociation.ps1
# 选择 "用 PowerShell 运行"
```

#### 方法 B：批处理
```bash
# 右键点击 RegisterFileAssociation.bat
# 选择 "以管理员身份运行"
```

#### 方法 C：注册表
```bash
# 双击 RegisterFileAssociation.reg
# 点击 "是" 确认
```

### 步骤 3：重启计算机（可选但推荐）
```bash
# 重启以确保更改生效
```

## ✅ 验证设置

### 测试 1：拖拽文件
1. 找到任意 `.bin` 文件
2. 拖拽到 BinCompare 程序图标
3. 程序应该启动并加载文件

### 测试 2：右键菜单
1. 右键点击 `.bin` 文件
2. 应该看到 "用 BinCompare 打开"
3. 点击打开

### 测试 3：双击打开
1. 双击 `.bin` 文件
2. 程序应该启动并加载文件

## 🔧 常见问题

### Q: 拖拽仍然不工作？
A: 
1. 确保程序安装在 `C:\Program Files\BinCompare\`
2. 重新运行注册脚本
3. 重启计算机

### Q: 看不到 "用 BinCompare 打开"？
A:
1. 重新运行注册脚本
2. 检查注册表是否正确
3. 重启计算机

### Q: 权限不足错误？
A:
1. 右键点击脚本
2. 选择 "以管理员身份运行"

## 📋 支持的文件类型

- `.bin` - 二进制文件
- `.dat` - 数据文件
- `.hex` - 十六进制文件
- `.rom` - ROM 文件
- `.img` - 镜像文件

## 🎯 使用方式

### 拖拽文件
```
拖拽 file.bin 到 BinCompare 图标 → 程序启动并加载文件
```

### 右键打开
```
右键 file.bin → "用 BinCompare 打开" → 程序启动并加载文件
```

### 双击打开
```
双击 file.bin → 程序启动并加载文件
```

### 命令行
```bash
BinCompare.exe "C:\path\to\file.bin"
```

## 📚 详细文档

- `FILE_ASSOCIATION_SETUP.md` - 完整的文件关联设置指南
- `DRAG_DROP_GUIDE.md` - 拖拽功能详细指南
- `COMMAND_LINE_REFERENCE.md` - 命令行参数参考

---

**版本**：v1.6.0+  
**最后更新**：2025-11-22
