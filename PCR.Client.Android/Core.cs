using System;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using ModernHttpClient;
using PCR.Client.Android.Models;

namespace PCR.Client.Android
{
    class Core
    {
        // Gets weather data from the passed URL.
        public static async Task UpdateAppVolume(string url, int volume, uint processId)
        {
            var message = new AppDetails()
            {
                ProcessId = processId,
                Volume = volume
            };

            try
            {
                Common.PostWebRequest(url, "Audio/Update", message);
            }
            catch (Exception e)
            {
                Common.PostWebRequest(url, "System/UpdateLog", e.Message);
            }
        }

        public static async Task MuteApp(string url, uint processId)
        {
            try
            {
                Common.PostWebRequest(url, "Audio/Mute", processId);
            }
            catch (Exception e)
            {
                Common.PostWebRequest(url, "System/UpdateLog", e.Message);
            }
            

            //var output = JsonConvert.SerializeObject(processId);
            //var httpContent = new StringContent(output, Encoding.UTF8, "application/json");

            //var httpClient = new HttpClient(new NativeMessageHandler());

            //await httpClient.PostAsync(url + "Audio/Mute", httpContent)
        }
    }
}