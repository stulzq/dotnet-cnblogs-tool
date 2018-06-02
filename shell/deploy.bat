@echo off
set sendtopath=C:\Users\%username%\AppData\Roaming\Microsoft\Windows\SendTo

move ..\Publish\dotnet-cnblog.exe.lnk %sendtopath%\dotnet-cnblog.lnk

pause