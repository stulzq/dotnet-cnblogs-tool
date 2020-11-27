using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dotnetcnblog.Command;
using Dotnetcnblog.Utils;
using McMaster.Extensions.CommandLineUtils;
using MetaWeblogClient;
using Newtonsoft.Json;
using Console = Colorful.Console;

namespace Dotnetcnblog
{
    [Command(Name = "dotnet-cnblog", Description = "dotNet 博客园工具")]
    [Subcommand(typeof(CommandReset))]
    [Subcommand(typeof(CommandSetConfig))]
    [Subcommand(typeof(CommandProcessFile))]
    class Program
    {
        private const string CfgFileName = "dotnet-cnblog.config.json";

        private static int Main(string[] args)
        {
            PrintTitle();
            if (Init())
            {
                return CommandLineApplication.Execute<Program>(args);
            }
            else
            {
                ConsoleHelper.PrintError("您还未设置配置，将引导你设置！");
                var setConfig=new CommandSetConfig();
                setConfig.Execute(CommandContextStore.Get());
                return 0;
            }
        }

        private int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 0;
        }

        static void PrintTitle()
        {
            Console.WriteAscii("dotNet Cnblogs Tool", Color.FromArgb(244, 212, 255));
            Console.Write("作者：", Color.FromArgb(90, 212, 255));
            Console.WriteLine("晓晨Master", Color.FromArgb(200, 212, 255));
            Console.Write("问题反馈：", Color.FromArgb(90, 212, 255));
            Console.WriteLine("https://github.com/stulzq/dotnet-cnblogs-tool/issues ", Color.FromArgb(200, 212, 255));
            Console.WriteLine("");
        }

        static bool Init()
        {
            var context=new CommandContext();
            context.AppConfigFilePath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location) ?? throw new InvalidOperationException(), CfgFileName);

            if (!File.Exists(context.AppConfigFilePath))
            {
                CommandContextStore.Set(context);
                return false;
            }

            var config = JsonConvert.DeserializeObject<BlogConnectionInfo>(File.ReadAllText(context.AppConfigFilePath));
            config.Password =
                Encoding.UTF8.GetString(TeaHelper.Decrypt(Convert.FromBase64String(config.Password), context.EncryptKey));
            context.ConnectionInfo = config;
            ImageUploadHelper.Init(config);
            CommandContextStore.Set(context);
            return true;
        }
    }
}