param ($version='latest')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../../"
$appFolder = Join-Path $slnFolder "Mazer.WebAssemblyDemo"


Write-Host "********* BUILDING Application *********" -ForegroundColor Green

$hostFolder = Join-Path $slnFolder "Mazer.WebAssemblyDemo.Host"
Set-Location $hostFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Mazer/webassemblydemo:$version .

### ALL COMPLETED
Write-Host "********* COMPLETED *********" -ForegroundColor Green
Set-Location $currentFolder
exit $LASTEXITCODE