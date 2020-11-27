using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Dotnetcnblog.TagHandlers;
using Dotnetcnblog.Utils;
using McMaster.Extensions.CommandLineUtils;
using Console = Colorful.Console;

namespace Dotnetcnblog.Command
{
    [Command(Name = "upload", Description = "上传单个图片")]
    public class CommandUploadImg : ICommand
    {
        private static readonly Dictionary<string, string> ReplaceDic = new Dictionary<string, string>();


        [Option("-f|--file", Description = "需要上传的图片路径")]
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

                    var imgUrl = ImageUploadHelper.Upload(FilePath);
                    ConsoleHelper.PrintMsg($"{FilePath} 上传成功. {imgUrl}");
                    
                }
            }
            catch (Exception e)
            {
                ConsoleHelper.PrintError(e.Message);
            }
        }
    }
}