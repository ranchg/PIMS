using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSI.Utilities
{
    public class TreeJsonHelper
    {
        public static string DirectoryStr2Json(string[] directories)
        {
            string jsonStr = "";
            Dictionary<string, string> dic = AnalyseDirectoryStr(directories);
            jsonStr = ToJson(dic, "null", "0_");
            return jsonStr;

        }
        private static Dictionary<string, string> AnalyseDirectoryStr(string[] directories)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("终端目录", "null");
            foreach (string directory in directories)
            {
                string[] folders = directory.Split('/');
                string parentFolder = "终端目录";
                string selfFolder = "终端目录";

                foreach (string folder in folders)
                {
                    selfFolder = selfFolder + "/" + folder;
                    if (!dic.ContainsKey(selfFolder))
                    {
                        dic.Add(selfFolder, parentFolder);
                    }
                    parentFolder = selfFolder;
                }
            }
            return dic;
        }

        private static string ToJson(Dictionary<string, string> dic, string parentFolder, string no)
        {
            int n = 0;
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (string key in dic.Keys)
            {
                if (dic[key] == parentFolder)
                {
                    string text = key.Substring(key.LastIndexOf('/') + 1);
                    builder.Append("{");
                    builder.Append("\"id\":\"" + no + n + "\",");
                    builder.Append("\"text\":\"" + text + "\",");
                    builder.Append("\"value\":\"" + key + "\",");
                    string childsStr = ToJson(dic, key, no + n + "_");
                    if (childsStr.Length > 4)
                    {
                        builder.Append("\"hasChildren\":true,");
                    }
                    else
                    {
                        builder.Append("\"hasChildren\":false,");
                    }

                    //区分文件夹
                    if (text.IndexOf(".") == -1)
                    {
                        builder.Append("\"img\":\"Content\\Images\\Icon16\\folder.png");
                    }
                    else
                    {
                        builder.Append("\"img\":\"Content\\Images\\Icon16\\document_green.png");
                    }
                    builder.Append("\"ChildNodes\":" + childsStr);

                    builder.Append("},");

                    n++;
                }

            }
            if (builder.Length > 2)
            {
                builder = builder.Remove(builder.Length - 1, 1);
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}
