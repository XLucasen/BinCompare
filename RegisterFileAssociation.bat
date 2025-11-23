@echo off
REM BinCompare 文件关联注册脚本（批处理版本）
REM 此脚本需要管理员权限运行

setlocal enabledelayedexpansion

REM 检查管理员权限
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo 错误: 此脚本需要管理员权限运行
    echo 请右键点击此文件并选择 "以管理员身份运行"
    pause
    exit /b 1
)

echo.
echo ========================================
echo BinCompare 文件关联注册脚本
echo ========================================
echo.

REM 设置程序路径
set "PROGRAM_PATH=C:\Program Files\BinCompare\BinCompare.exe"

REM 检查程序是否存在
if not exist "%PROGRAM_PATH%" (
    echo 错误: 找不到 BinCompare 程序
    echo 预期路径: %PROGRAM_PATH%
    echo.
    echo 请确保 BinCompare 已安装到 C:\Program Files\BinCompare\
    pause
    exit /b 1
)

echo 程序路径: %PROGRAM_PATH%
echo.
echo 开始注册文件关联...
echo.

REM 导入注册表文件
REM 注意: 需要修改 RegisterFileAssociation.reg 中的路径为实际安装路径
reg import "%~dp0RegisterFileAssociation.reg" >nul 2>&1

if %errorlevel% equ 0 (
    echo ✓ 文件关联注册成功
) else (
    echo ✗ 文件关联注册失败
    pause
    exit /b 1
)

echo.
echo ========================================
echo 注册完成!
echo ========================================
echo.
echo 现在您可以:
echo 1. 拖拽 .bin, .dat, .hex, .rom, .img 文件到 BinCompare 程序图标
echo 2. 在文件管理器中右键点击文件，选择 "用 BinCompare 打开"
echo 3. 双击二进制文件直接用 BinCompare 打开
echo.
echo 提示: 如果拖拽仍然不工作，请尝试:
echo 1. 重启计算机
echo 2. 重新运行此脚本
echo 3. 检查程序路径是否正确
echo.
pause
