using System.Threading.Tasks;

namespace Dotnetcnblog.Command
{
    public interface ICommand
    {
        void Execute(CommandContext context);
    }
}