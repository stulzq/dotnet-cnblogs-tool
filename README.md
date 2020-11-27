# dotNet 博客园工具 

[![](https://img.shields.io/nuget/v/dotnet-cnblog.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/dotnet-cnblog)

## 1.安装

（1）具有 .NET Core/.NET 5 环境可以直接使用命令安装：

````shell
dotnet tool install --global dotnet-cnblog
````

（2）如果没有上面的环境，那么可以直接下载二进制文件 

下载地址： https://github.com/stulzq/dotnet-cnblogs-tool/releases

> 因为本工具是开源的，而且使用过程中需要输入密码，所以不要相信任何第三方下载，因为它们有可能被植入恶意代码，仅提供上面两种方式。

## 2.使用

第一次运行需要配置博客ID，账号密码等，按照提示输入即可，对信息采用tea加密算法进行加密存储。

![first-config](./assets/first-config.png)

> 需要账号密码是因为调用 MetaWeblog API 需要此信息

如果安装成功，但是无法正常运行:

![error](./assets/error.png)

原因是因为你没有配置path环境变量，我们可以查看下C:\Users\用户名\\.dotnet\tools 看看是否存在 dotnet-cnblog.exe。

![ls](./assets/ls.png)

如果存在就把这个目录添加到path环境变量即可。

![add_path](./assets/add_path.png)

### 重置配置

使用下面的命令重置配置:
````shell
dotnet-cnblog reset
````

![reset](./assets/reset.png)

## 3.处理 Markdown 文件中的图片

使用命令对Markdown文件里的图片进行解析，上传到博客园，并且转换内容保存到新的文件中。

````shell
dotnet-cnblog proc -f <markdown文件路径>
````
![test](./assets/test.gif)

## 4.说明

- 程序未加过多的容错机制，请勿暴力测试。比如发送一个非MarkDown文件到程序。

- 上传图片具有重试机制，重试三次。

- 只有本地路径的图片才会上传，所有http/https远程图片都会过滤

- 图片上传完毕以后，会自动转换md内容保存到带`cnblog`后缀的文件里面

- 密码错误请重置配置



