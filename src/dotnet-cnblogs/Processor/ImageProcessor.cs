using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CnBlogPublishTool.Processor
{
    public class ImageProcessor:ITagProcessor
    {
        const string MatchRule= @"!\[.*?\]\((.*?)\)";
        public List<string> Process(string content)
        {
            List<string> result=new List<string>();

            var matchs = Regex.Matches(content, MatchRule, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);

            foreach (Match match in matchs)
            {
                result.Add(match.Groups[1].Value);
            }

            return result;
        }
    }
}