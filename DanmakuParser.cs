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
                var uri = new Uri($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}");
                string danmuStr = await HttpClient.GetResultsDeflate(uri);
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
                                Time = double.Parse(danmaku[0]),
                                Location = location,
                                Size = double.Parse(danmaku[2]),
                                Color = danmaku[3].ToColor(true),
                                SendTime = danmaku[4],
                                Pool = danmaku[5],
                                SendID = danmaku[6],
                                RowID = danmaku[7],
                                Text = item.InnerText,
                                Source = item.OuterXml,
                                FromSite = DanmakuSite.Bilibili
                            });
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("ParseBiliBiliXml:" + ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ParseBiliBiliXml:" + ex.ToString());
            }
            return danmakus;
        }
    }
}