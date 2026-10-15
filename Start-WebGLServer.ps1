$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $projectRoot
Write-Host 'WebGL server: http://127.0.0.1:8765/'
Write-Host 'Keep this PowerShell window open while playing.'
python -m http.server 8765 --directory (Join-Path $projectRoot 'Builds/WebGL')
