@echo off
echo Mission preparation..

dotnet publish ..\CnBlogPublishTool.sln  -c Release -r win10-x86 -o ../Publish

echo mission accomplished!

pause

