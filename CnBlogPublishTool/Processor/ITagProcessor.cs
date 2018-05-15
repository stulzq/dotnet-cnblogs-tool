using System.Collections.Generic;

namespace CnBlogPublishTool.Processor
{
    public interface ITagProcessor
    {
        List<string> Process(string content);
    }
}