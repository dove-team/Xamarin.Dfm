using bilibili_lib.Extensions;
using bilibili_lib.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace bilibili_lib.Providers
{
    public sealed class DanmakuParse
    {
        private HttpClientEx HttpClient
        {
            get
            {
                return StaticValue.HttpClient;
            }
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
                LogManager.Instance.LogError("ParseBiliBili", ex);
                return null;
            }
        }
        public async Task<string> GetBiliBili(long cid)
        {
            return await HttpClient.GetResultsDeflate(new Uri(string.Format("https://api.bilibili.com/x/v1/dm/list.so?oid={0}", cid)));
        }
        public List<DanmakuModel> ParseBiliBili(string xml)
        {
            return ParseBiliBiliXml(xml);
        }
        public async Task<List<DanmakuModel>> ParseBiliBili(Uri url)
        {
            string danmuStr = await HttpClient.GetResults(url);
            return ParseBiliBiliXml(danmuStr);
        }
        public async Task<List<DanmakuModel>> ParseBiliBili(FileInfo file)
        {
            using var stream = file.OpenRead();
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            string danmuStr = Encoding.UTF8.GetString(bytes);
            return ParseBiliBiliXml(danmuStr);
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
                                time_s = Convert.ToInt32(double.Parse(danmaku[0])),
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
                            LogManager.Instance.LogError("ParseBiliBiliXml", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("ParseBiliBiliXml", ex);
            }
            return danmakus;
        }
    }
}