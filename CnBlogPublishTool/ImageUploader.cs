using System;
using System.IO;
using MetaWeblogClient;

namespace CnBlogPublishTool
{
    public class ImageUploader
    {
        public static Client BlogClient;

        static ImageUploader()
        {
            BlogClient=new Client(new BlogConnectionInfo("http://www.cnblogs.com/stulzq", "https://rpc.cnblogs.com/metaweblog/stulzq", "stulzq","stulzq",""));
        }

        public static string Upload(string filePath)
        {
            FileInfo fileinfo = new FileInfo(filePath);
            var mediaObjectInfo = BlogClient.NewMediaObject(fileinfo.Name, MimeMapping.GetMimeMapping(filePath), File.ReadAllBytes(filePath));
            return mediaObjectInfo.URL;
        }
    }
}