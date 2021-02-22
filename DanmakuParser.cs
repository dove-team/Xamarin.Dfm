using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Dfm.Extensions;
using Xamarin.Dfm.Model;

namespace Xamarin.Dfm
{
    public sealed class DanmakuParser
    {
        private HttpClientEx HttpClient { get; }
        public DanmakuParser()
        {
            HttpClient = new HttpClientEx();
        }
        public async Task<List<DanmakuModel>> ParseBiliBili(long cid)
        {
            try
            {
                string danmuStr = await HttpClient.GetResultsDeflate(new Uri(string.Format("https://api.bilibili.com/x/v1/dm/list.so?oid={0}", cid)));
                return ParseBiliBiliXml(danmuStr);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ParseBiliBili" + ex.Message);
                return null;
            }
        }
        public List<DanmakuModel> ParseBiliBili(string xml)
        {
            return ParseBiliBiliXml(xml);
        }
        private List<DanmakuModel> ParseBiliBiliXml(string xmlStr)
        {
            List<DanmakuModel> danmakus = new List<DanmakuModel>();
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xmlStr = Regex.Replace(xmlStr, @"[\x00-\x08]|[\x0B-\x0C]|[\x0E-\x1F]", "");
                xdoc.LoadXml(xmlStr);
                foreach (XmlNode item in xdoc.DocumentElement.ChildNodes)
                {
                    if (item.Attributes["p"] != null)
                    {
                        try
                        {
                            string node = item.Attributes["p"].Value;
                            string[] danmaku = node.Split(',');
                            var location = danmaku[1] switch
                            {
                                "7" => DanmakuLocation.Position,
                                "4" => DanmakuLocation.Bottom,
                                "5" => DanmakuLocation.Top,
                                _ => DanmakuLocation.Roll,
                            };
                            danmakus.Add(new DanmakuModel
                            {
                                time = double.Parse(danmaku[0]),
                                location = location,
                                size = double.Parse(danmaku[2]),
                                color = danmaku[3].ToColor(true),
                                sendTime = danmaku[4],
                                pool = danmaku[5],
                                sendID = danmaku[6],
                                rowID = danmaku[7],
                                text = item.InnerText,
                                source = item.OuterXml,
                                fromSite = DanmakuSite.Bilibili
                            });
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("ParseBiliBiliXml" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ParseBiliBiliXml" + ex.Message);
            }
            return danmakus;
        }
    }
}