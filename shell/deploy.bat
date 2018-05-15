@echo off
set sendtopath=C:\Users\%username%\AppData\Roaming\Microsoft\Windows\SendTo

move ..\Publish\CnBlogPublishTool.exe.lnk %sendtopath%\CnBlogPublishTool.lnk

pause