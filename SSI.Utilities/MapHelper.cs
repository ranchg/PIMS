using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace SSI.Utilities
{
    public class MapHelper
    {
        public MapHelper() { }

        public static readonly string AMAP_URI_DEGEO = ConfigHelper.AppSettings("MapAnalyzeUrl") ?? "http://61.183.84.103:8081/rgeocode/simple?sid=7001&amp;region={0}&amp;poinum=1&amp;range={1}&amp;resType=xml&amp;rid=123&amp;encode=UTF-8&amp;roadnum={2}&amp;show_near_districts=false&amp;crossnum=5&amp;key=e269da2b031f296f47bea05141da1aa7156f34017320828c9153b2a9848d6bc45d3544236b22fc48";

        public static int RANGE = 1000;//点半径 单位米

        public static int ROADNUM = 2;//道路条数

        public static string Pos2Addess(double lon, double lat)
        {
            string address = "";
            try
            {
                string points = lon + "," + lat;
                string format = string.Format(AMAP_URI_DEGEO, points, RANGE, ROADNUM);
                WebClient client = new WebClient();
                client.Headers.Add("Accept-Language", "zh-cn");
                byte[] bytes = client.DownloadData(format);
                string xml = Encoding.UTF8.GetString(bytes);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                #region xml解析
                if (document.SelectNodes("/searchresult/status").Item(0).InnerText == "E0")
                {
                    string strProvince = "";
                    string strCity = "";
                    string strDistrict = "";
                    string strStreet = "";
                    XmlNode provinceNode = document.SelectNodes("/searchresult/list/spatial/province/name").Item(0);
                    if (provinceNode != null)
                    {
                        strProvince = provinceNode.InnerText;
                    }
                    XmlNode cityNode = document.SelectNodes("/searchresult/list/spatial/city/name").Item(0);
                    if (cityNode != null)
                    {
                        strCity = cityNode.InnerText;
                    }
                    XmlNode districtNode = document.SelectNodes("/searchresult/list/spatial/district/name").Item(0);
                    if (districtNode != null)
                    {
                        strDistrict = districtNode.InnerText;
                    }

                    XmlNode streetNode = document.SelectNodes("/searchresult/list/spatial/roadlist/road/name").Item(0);
                    if (streetNode != null)
                    {
                        strStreet = streetNode.InnerText;
                    }
                    address = strProvince + strCity + strDistrict + strStreet;
                }
                #endregion
            }
            catch (Exception ex)
            {
                address = "获取地址失败：" + ex.Message;
            
            }
            return address;
        }
    }
}
