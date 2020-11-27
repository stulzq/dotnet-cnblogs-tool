dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
dotnet publish -r win-x86 -p:PublishSingleFile=true --self-contained true
dotnet publish -r osx-x64 -p:PublishSingleFile=true --self-contained true
dotnet publish -r linux-x64 -p:PublishSingleFile=true --self-contained true
dotnet publish -r linux-arm -p:PublishSingleFile=true --self-contained true