using System.IO;
using Dotnetcnblog.Utils;
using McMaster.Extensions.CommandLineUtils;

namespace Dotnetcnblog.Command
{
    [Command(Name = "reset", Description = "重置配置")]
    public class CommandReset : ICommand
    {
        public int OnExecute(CommandLineApplication app)
        {
            Execute(CommandContextStore.Get());
            return 0;
        }

        public void Execute(CommandContext context)
        {
            File.Delete(context.AppConfigFilePath);
            ConsoleHelper.PrintMsg("配置重置成功！");
        }
    }
}