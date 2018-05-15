@echo off
echo 准备发布

dotnet publish ..\CnBlogPublishTool.sln  -c Release -r win10-x86 -o ../Publish

echo 发布完成

pause
