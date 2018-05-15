using System;
using System.IO;
using CnBlogPublishTool.Util;
using MetaWeblogClient;

namespace CnBlogPublishTool
{
    public class ImageUploader
    {
        public static Client BlogClient;

        public static void Init(BlogConnectionInfo info)
        {
            BlogClient=new Client(info);
        }

        public static string Upload(string filePath)
        {
            FileInfo fileinfo = new FileInfo(filePath);
            var mediaObjectInfo = BlogClient.NewMediaObject(fileinfo.Name, MimeMapping.GetMimeMapping(filePath), File.ReadAllBytes(filePath));
            return mediaObjectInfo.URL;
        }
    }
}