using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dotnetcnblog.Utils;
using McMaster.Extensions.CommandLineUtils;
using MetaWeblogClient;
using Newtonsoft.Json;
using Console = Colorful.Console;

namespace Dotnetcnblog.Command
{
    [Command(Name = "set", Description = "设置配置")]
    public class CommandSetConfig : ICommand
    {

        public int OnExecute(CommandLineApplication app)
        {
            Execute(CommandContextStore.Get());
            return 0;
        }

        public void Execute(CommandContext context)
        {
            ConsoleHelper.PrintMsg("请输入博客ID：（如：https://www.cnblogs.com/stulzq 的博客id为 stulzq ）");
            var blogId = Console.ReadLine();

            ConsoleHelper.PrintMsg("请输入用户名：");
            var userName = Console.ReadLine();

            ConsoleHelper.PrintMsg("请输入密  码：");
            var pwd = Console.ReadLine();

            var config = new BlogConnectionInfo(
                "https://www.cnblogs.com/" + blogId,
                "https://rpc.cnblogs.com/metaweblog/" + blogId,
                blogId,
                userName,
                Convert.ToBase64String(TeaHelper.Encrypt(Encoding.UTF8.GetBytes(pwd), context.EncryptKey)));

            File.WriteAllText(context.AppConfigFilePath, JsonConvert.SerializeObject(config));

            ConsoleHelper.PrintMsg("配置设置成功！");
        }
    }
}