# BinCompare 文件关联注册脚本
# 此脚本需要管理员权限运行

# 检查是否以管理员身份运行
if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Host "错误: 此脚本需要管理员权限运行" -ForegroundColor Red
    Write-Host "请右键点击 PowerShell 并选择 '以管理员身份运行'" -ForegroundColor Yellow
    exit 1
}

# 获取 BinCompare 程序路径
$programPath = "C:\Program Files\BinCompare\BinCompare.exe"

# 检查程序是否存在
if (-not (Test-Path $programPath)) {
    Write-Host "错误: 找不到 BinCompare 程序: $programPath" -ForegroundColor Red
    Write-Host "请确保 BinCompare 已安装到 C:\Program Files\BinCompare\" -ForegroundColor Yellow
    exit 1
}

Write-Host "开始注册文件关联..." -ForegroundColor Green
Write-Host "程序路径: $programPath" -ForegroundColor Cyan

# 要注册的文件扩展名
$extensions = @(".bin", ".dat", ".hex", ".rom", ".img")

# 注册文件关联
foreach ($ext in $extensions) {
    try {
        # 注册文件类型
        $progId = "BinCompare.BinaryFile"
        
        # 创建 ProgID 主项
        $progIdPath = "HKCU:\Software\Classes\$progId"
        if (-not (Test-Path $progIdPath)) {
            New-Item -Path $progIdPath -Force | Out-Null
        }
        Set-ItemProperty -Path $progIdPath -Name "(Default)" -Value "二进制文件" -Force
        
        # 创建 DefaultIcon
        $iconPath = "$progIdPath\DefaultIcon"
        if (-not (Test-Path $iconPath)) {
            New-Item -Path $iconPath -Force | Out-Null
        }
        Set-ItemProperty -Path $iconPath -Name "(Default)" -Value "$programPath,0" -Force
        
        # 创建 shell 项
        $shellPath = "$progIdPath\shell"
        if (-not (Test-Path $shellPath)) {
            New-Item -Path $shellPath -Force | Out-Null
        }
        Set-ItemProperty -Path $shellPath -Name "(Default)" -Value "open" -Force
        
        # 创建 shell\open 项
        $openPath = "$shellPath\open"
        if (-not (Test-Path $openPath)) {
            New-Item -Path $openPath -Force | Out-Null
        }
        Set-ItemProperty -Path $openPath -Name "(Default)" -Value "用 BinCompare 打开" -Force
        
        # 创建 shell\open\command 项
        $commandPath = "$openPath\command"
        if (-not (Test-Path $commandPath)) {
            New-Item -Path $commandPath -Force | Out-Null
        }
        Set-ItemProperty -Path $commandPath -Name "(Default)" -Value "`"$programPath`" `"%1`"" -Force
        
        # 注册文件扩展名
        $extPath = "HKCU:\Software\Classes\$ext"
        if (-not (Test-Path $extPath)) {
            New-Item -Path $extPath -Force | Out-Null
        }
        Set-ItemProperty -Path $extPath -Name "(Default)" -Value $progId -Force
        
        Write-Host "✓ 已注册 $ext 文件关联" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ 注册 $ext 失败: $_" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "文件关联注册完成!" -ForegroundColor Green
Write-Host ""
Write-Host "现在您可以:" -ForegroundColor Cyan
Write-Host "1. 拖拽 .bin, .dat, .hex, .rom, .img 文件到 BinCompare 程序图标" -ForegroundColor White
Write-Host "2. 在文件管理器中右键点击文件，选择 '用 BinCompare 打开'" -ForegroundColor White
Write-Host "3. 双击二进制文件直接用 BinCompare 打开" -ForegroundColor White
Write-Host ""
Write-Host "提示: 如果拖拽仍然不工作，请尝试:" -ForegroundColor Yellow
Write-Host "1. 重启计算机" -ForegroundColor White
Write-Host "2. 重新注册文件关联" -ForegroundColor White
Write-Host "3. 检查程序路径是否正确" -ForegroundColor White
