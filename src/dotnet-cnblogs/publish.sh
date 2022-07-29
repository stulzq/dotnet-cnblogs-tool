set -e

dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IsTrimmable=true -p:TrimMode=link --self-contained true
dotnet publish -c Release -r win-x86 -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IsTrimmable=true -p:TrimMode=link  --self-contained true
dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IsTrimmable=true -p:TrimMode=link  --self-contained true
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IsTrimmable=true -p:TrimMode=link  --self-contained true
dotnet publish -c Release -r linux-arm -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IsTrimmable=true -p:TrimMode=link  --self-contained true
