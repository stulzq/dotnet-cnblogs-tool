using System.Collections.Generic;

namespace Dotnetcnblog.TagHandlers
{
    public interface ITagHandler
    {
        List<string> Process(string content);
    }
}