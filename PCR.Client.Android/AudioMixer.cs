using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ModernHttpClient;
using Newtonsoft.Json;
using PCR.Common.Models;

namespace PCR.Client.Android
{
    [Activity(Label = "PC Remote")]
    public class AudioMixer : Activity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AudioMixer);

            var url = Intent.GetStringExtra("IPAddress") ?? "Data not available";

            //var url = "http://192.168.1.210:4222/";
            //var url = "http://10.10.11.149:4222/";

            UpdateUi(url);
        }

        private async Task UpdateUi(string url)
        {
            while (true)
            {
                var appList = new List<AppDetails>();

                try
                {
                    var response = await Common.GetWebRequest(url, "Audio/All");
                    var responseBody = await response.Content.ReadAsStringAsync();
                    appList = JsonConvert.DeserializeObject<List<AppDetails>>(responseBody);
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, "Unable to retrieve audio session, check server is running and try again",
                        ToastLength.Short).Show();
                    Common.PostWebRequest(url, "System/UpdateLog", e.Message);
                }

                var adapter = new AudioListAdapter(this, appList, url);
                var audioListView = FindViewById<ListView>(Resource.Id.AudioListView);
                audioListView.Adapter = adapter;
                await Task.Delay(30000);
            }
        }
    }
}