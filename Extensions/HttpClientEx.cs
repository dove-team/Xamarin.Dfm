using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Android.Net;

namespace Xamarin.Dfm.Extensions
{
    public sealed class HttpClientEx
    {
        public async Task<string> GetResultsDeflate(Uri url)
        {
            try
            {
                var clientHandler = new AndroidClientHandler { UseCookies = true };
                clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssl) => { return true; };
                using HttpClient hc = new HttpClient(clientHandler);
                HttpResponseMessage hr = await hc.GetAsync(url).ConfigureAwait(false);
                hr.EnsureSuccessStatusCode();
                var encodeResults = await hr.Content.ReadAsStreamAsync().ConfigureAwait(false);
                DeflateStream deflateStream = new DeflateStream(encodeResults, CompressionMode.Decompress);
                StreamReader streamReader = new StreamReader(deflateStream, Encoding.UTF8);
                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetResultsDeflate" + ex.Message);
                return string.Empty;
            }
        }
    }
}