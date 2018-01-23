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
using PCR.Common;

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

            UpdateUi(url);
        }

        private async Task UpdateUi(string url)
        {
            while (true)
            {
                var appList = new List<AppDetails>();
                try
                {
                    appList = await Audio.GetAudioStatus<AppDetails>(url);
                }
                catch(Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                    WebRequest.PostWebRequest(url, "System/UpdateLog", ex.Message);
                }

                var adapter = new AudioListAdapter(this, appList, url);
                var audioListView = FindViewById<ListView>(Resource.Id.AudioListView);
                audioListView.Adapter = adapter;
                await Task.Delay(30000);
            }
        }
    }
}