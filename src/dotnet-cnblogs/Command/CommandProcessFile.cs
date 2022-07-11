using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using Dotnetcnblog.TagHandlers;
using Dotnetcnblog.Utils;
using McMaster.Extensions.CommandLineUtils;
using Console = Colorful.Console;

namespace Dotnetcnblog.Command
{
    [Command(Name = "proc", Description = "处理文件")]
    public class CommandProcessFile : ICommand
    {
        private static readonly Dictionary<string, string> ReplaceDic = new Dictionary<string, string>();


        [Option("-f|--file", Description = "需要处理的文件路径")]
        [Required]
        public string FilePath { get; set; }

        public int OnExecute(CommandLineApplication app)
        {
            if (app.Options.Count == 1 && app.Options[0].ShortName == "h")
            {
                app.ShowHelp();
            }

            Execute(CommandContextStore.Get());
            return 0;
        }

        public void Execute(CommandContext context)
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    ConsoleHelper.PrintError($"文件不存在：{FilePath}");
                }
                else
                {
                    var fileDir = new FileInfo(FilePath).DirectoryName;
                    var fileContent = File.ReadAllText(FilePath);
                    var imgHandler = new ImageHandler();
                    var imgList = imgHandler.Process(fileContent);

                    ConsoleHelper.PrintMsg($"提取图片成功，共 {imgList.Count} 个。");

                    //循环上传图片
                    foreach (var img in imgList)
                    {
                        if (img.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                        {
                            ConsoleHelper.PrintMsg($"图片跳过：{img} ");
                            continue;
                        }

                        try
                        {
                            var imgPhyPath = HttpUtility.UrlDecode(Path.Combine(fileDir!, img));
                            if (File.Exists(imgPhyPath))
                            {
                                var imgUrl = ImageUploadHelper.Upload(imgPhyPath);
                                if (!ReplaceDic.ContainsKey(img)) ReplaceDic.Add(img, imgUrl);
                                ConsoleHelper.PrintMsg($"{img} 上传成功. {imgUrl}");
                            }
                            else
                            {
                                ConsoleHelper.PrintMsg($"{img} 未发现文件.");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    //替换
                    fileContent = ReplaceDic.Keys.Aggregate(fileContent, (current, key) => current.Replace(key, ReplaceDic[key]));

                    var newFileName = FilePath.Substring(0, FilePath.LastIndexOf('.')) + "-cnblog" + Path.GetExtension(FilePath);
                    File.WriteAllText(newFileName, fileContent, FileEncodingType.GetType(FilePath));

                    ConsoleHelper.PrintMsg($"处理完成！文件保存在：{newFileName}");
                }
            }
            catch (Exception e)
            {
                ConsoleHelper.PrintError(e.Message);
            }
        }
    }
}