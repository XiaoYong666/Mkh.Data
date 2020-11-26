# 定义变量用于接收发布时用到的Key
$key = Read-Host 'Enter NuGet Api Key'

# 删除_packages目录
if(Test-Path ./_packages){
    Remove-Item ./_packages -recurse
}
# 编译项目
dotnet build -c Release

# 遍历_packages目录发布包
Get-ChildItem -Path ./_packages | ForEach-Object -Process{
    dotnet nuget push $_.fullname -s https://api.nuget.org/v3/index.json -k $key
}

write-host 'Publish success~'
pause