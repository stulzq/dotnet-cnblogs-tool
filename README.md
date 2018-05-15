# 博客园快捷发布工具

## 一.准备环境

安装.NET Core SDK: https://www.microsoft.com/net/learn/get-started/windows

## 二.编译Release包

进入`shell`文件夹，运行`publish.bat`

>可能会引起杀毒软件误报，请允许。

## 三.创建快捷方式

进入 `项目根目录\Publish` 文件夹，选中 `CnBlogPublishTool.exe`，【右键菜单】->【创建快捷方式】

然后进入 `项目根目录\shell` 文件夹，运行`deploy.bat`

## 四.使用

选中一个MarkDown文件，【右键菜单】->【发送到】->【CnBlogPublishTool】，便会开始解析图片并自动上传到博客园。

第一次使用会让您配置博客ID和博客园的用户名密码，密码采用tea加密存储，请放心使用。

## 五.使用演示

![](assets/ys.gif)

## 六.说明

- 程序未加过多的容错机制，请勿暴力测试。比如发送一个非MarkDown文件到程序。

- 上传图片具有重试机制，重试三次。

- 只有本地路径的图片才会上传，所有http/https远程图片都会过滤

- 图片上传完毕以后，会自动转换md内容保存到带`cnblog`后缀的文件里面

- 密码错误请到程序根目录删除`config.json`后重新运行程序，将会让你设置密码



