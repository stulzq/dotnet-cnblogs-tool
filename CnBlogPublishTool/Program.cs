using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CnBlogPublishTool
{
    class Program
    {
        private static string _filePath;
        private static string _fileDir;
        private static string _fileContent;
        private static readonly Dictionary<string,string> ReplaceDic=new Dictionary<string, string>();
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("请输入文件路径！");
            }
            else
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
                        _fileDir=new FileInfo(_filePath).DirectoryName;
                        _fileContent = File.ReadAllText(_filePath);
                        var imgProcessor = new ImageProcessor();
                        var imgList = imgProcessor.Process(_fileContent);
                        Console.WriteLine($"提取图片成功，共{imgList.Count}个.");

                        //循环上传图片
                        foreach (var img in imgList)
                        {
                            if (img.StartsWith("http")) { continue;}

                            try
                            {
                                string imgPhyPath = Path.Combine(_fileDir, img);
                                if (File.Exists(imgPhyPath))
                                {
                                    var imgUrl = ImageUploader.Upload(imgPhyPath);
                                    ReplaceDic.Add(img,imgUrl);
                                    Console.WriteLine($"{img} upload success. {imgUrl}");
                                }
                                else
                                {
                                    Console.WriteLine($"{img} Not Found.");
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

                        string newFileName = _filePath.Substring(0, _filePath.LastIndexOf('.')) +"-cnlog-"+
                                             new FileInfo(_filePath).Extension;
                        File.WriteAllText(newFileName, _fileContent,TxtFileEncoder.GetEncoding(_filePath));

                        Console.WriteLine($"处理完成！文件保存在：{newFileName}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
