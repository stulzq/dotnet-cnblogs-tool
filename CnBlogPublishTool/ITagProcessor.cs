using System.Collections.Generic;

namespace CnBlogPublishTool
{
    public interface ITagProcessor
    {
        List<string> Process(string content);
    }
}