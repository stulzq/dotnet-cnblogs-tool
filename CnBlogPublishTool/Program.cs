using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CnBlogPublishTool.Processor;
using CnBlogPublishTool.Util;
using MetaWeblogClient;
using Newtonsoft.Json;

namespace CnBlogPublishTool
{
    class Program
    {
        private static string _filePath;
        private static string _fileDir;
        private static string _fileContent;
        private static byte[] _teaKey=new byte[]{21,52,33,78,52,45};
        private const string ConfigFilePath = "config.json";
        private static BlogConnectionInfo _connInfo;
        private static readonly Dictionary<string,string> ReplaceDic=new Dictionary<string, string>();
        static void Main(string[] args)
        {
            Console.Title = "晓晨-博客快捷上传图片工具";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\n\t\t\t欢迎使用晓晨-博客快捷上传图片工具，使用问题或者建议请联系QQ群4656606\n\n");
            //加载配置
            LoadConfig();

            if (args.Length == 0)
            {
                Console.WriteLine("请输入文件路径！");
            }
            else
            {
                ProcessFile(args);
            }

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        static void LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                _connInfo = JsonConvert.DeserializeObject<BlogConnectionInfo>(File.ReadAllText(ConfigFilePath));
                _connInfo.Password =
                    Encoding.UTF8.GetString(TeaHelper.Decrypt(Convert.FromBase64String(_connInfo.Password), _teaKey));
                ImageUploader.Init(_connInfo);
            }
            else
            {
                SetConfig();
            }
        }

        static void SetConfig()
        {
            Console.WriteLine("您是第一次运行本程序，请配置以下参数：");

            Console.WriteLine("请输入博客ID：（如：https://www.cnblogs.com/stulzq 的博客id为 stulzq ）");
            string blogid = Console.ReadLine();

            Console.WriteLine("请输入用户名：");
            string uname = Console.ReadLine();

            Console.WriteLine("请输入密  码：");
            string pwd = Console.ReadLine();

            _connInfo =new BlogConnectionInfo(
                "https://www.cnblogs.com/"+blogid,
                "https://rpc.cnblogs.com/metaweblog/"+blogid,
                blogid,
                uname,
                Convert.ToBase64String(TeaHelper.Encrypt(Encoding.UTF8.GetBytes(pwd), _teaKey)));

            File.WriteAllText(ConfigFilePath,JsonConvert.SerializeObject(_connInfo));

            _connInfo.Password = pwd;

            ImageUploader.Init(_connInfo);
        }

        static void ProcessFile(string[] args)
        {
            try
            {
                _filePath = args[0];
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("指定的文件不存在！");
                }
                else
                {
                    _fileDir = new FileInfo(_filePath).DirectoryName;
                    _fileContent = File.ReadAllText(_filePath);
                    var imgProcessor = new ImageProcessor();
                    var imgList = imgProcessor.Process(_fileContent);
                    Console.WriteLine($"提取图片成功，共{imgList.Count}个.");

                    //循环上传图片
                    foreach (var img in imgList)
                    {
                        if (img.StartsWith("http"))
                        {
                            Console.WriteLine($"{img} 跳过.");
                            continue;
                        }

                        try
                        {
                            string imgPhyPath = Path.Combine(_fileDir, img);
                            if (File.Exists(imgPhyPath))
                            {
                                var imgUrl = ImageUploader.Upload(imgPhyPath);
                                if (!ReplaceDic.ContainsKey(img))
                                {
                                    ReplaceDic.Add(img, imgUrl);
                                }
                                Console.WriteLine($"{img} 上传成功. {imgUrl}");
                            }
                            else
                            {
                                Console.WriteLine($"{img} 未发现文件.");
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    //替换
                    foreach (var key in ReplaceDic.Keys)
                    {
                        _fileContent = _fileContent.Replace(key, ReplaceDic[key]);
                    }

                    string newFileName = _filePath.Substring(0, _filePath.LastIndexOf('.')) + "-cnblog" +
                                         new FileInfo(_filePath).Extension;
                    File.WriteAllText(newFileName, _fileContent, EncodingType.GetType(_filePath));

                    Console.WriteLine($"处理完成！文件保存在：{newFileName}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
