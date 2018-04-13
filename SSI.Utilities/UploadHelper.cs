using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace SSI.Utilities
{
    public class UploadHelper
    {
        private static string uploadPath = ConfigHelper.AppSettings("UploadRoot") ?? "~/Upload/";

        public static string GetAbsUrl(string url)
        {
            string path = uploadPath.TrimStart('~') + url;
            return path.Replace(@"\", @"/").Replace(@"//", @"/");
        }
        public static string GetUrl(string path)
        {
            string root = HttpContext.Current.Server.MapPath(uploadPath);
            return path.Replace(root, "").Replace(@"\", @"/");
        }
        public static string GetPath(string url)
        {
            string root = HttpContext.Current.Server.MapPath(uploadPath);
            return root + url.Replace(@"/", @"\");
        }

        public static string ImageSaveSquare(HttpPostedFileBase file, string dir, int side)
        {
            string fileDir = string.IsNullOrEmpty(dir) ? "" : dir.Trim('/') + "/";
            string fileName = Guid.NewGuid().ToString("N") + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string fullPath = HttpContext.Current.Server.MapPath(uploadPath + fileDir + fileName);
            ImageHelper.CutForSquare(file.InputStream, fullPath, side);
            return fileDir + fileName;
        }

        public static string ImageSaveCustom(HttpPostedFileBase file, string dir, int width, int height)
        {
            string fileDir = string.IsNullOrEmpty(dir) ? "" : dir.Trim('/') + "/";
            string fileName = Guid.NewGuid().ToString("N") + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string fullPath = HttpContext.Current.Server.MapPath(uploadPath + fileDir + fileName);
            ImageHelper.CutForCustom(file.InputStream, fullPath, width, height);
            return fileDir + fileName;
        }

        public static string ImageSaveZoom(HttpPostedFileBase file, string dir, int width, int height)
        {
            string fileDir = string.IsNullOrEmpty(dir) ? "" : dir.Trim('/') + "/";
            string fileName = Guid.NewGuid().ToString("N") + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string fullPath = HttpContext.Current.Server.MapPath(uploadPath + fileDir + fileName);
            ImageHelper.ZoomAuto(file.InputStream, fullPath, width, height);
            return fileDir + fileName;
        }

        public static string DocSave(HttpPostedFileBase file, string dir)
        {
            string fileDir = string.IsNullOrEmpty(dir) ? "" : dir.Trim('/') + "/";
            string fileName = Guid.NewGuid().ToString("N") + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string fullPath = HttpContext.Current.Server.MapPath(uploadPath + fileDir + fileName);
            return fileDir + fileName;
        }
    }
}
