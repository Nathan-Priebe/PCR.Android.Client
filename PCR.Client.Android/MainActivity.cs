using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using ModernHttpClient;
using Newtonsoft.Json;
using PCR.Common;

namespace PCR.Client.Android
{
    [Activity(Label = "PC Remote", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var connectButton = FindViewById<Button>(Resource.Id.Connect);
            var ipAddressInput = FindViewById<EditText>(Resource.Id.IPAddressInput);

            const string localSettingFile = "localData.json";

            //Enable once saving of ip address is working
            //Code ot read back from file
            //using (var streamReader = new StreamReader(localSettingFile))
            //{
            //    string content = streamReader.ReadToEnd();
            //    System.Diagnostics.Debug.WriteLine(content);
            //}

            connectButton.Click += async (sender, args) =>
            {
                var ipAddress = "http://" + ipAddressInput.Text + ":4222/";
                string[] splitValues = ipAddress.Split('.');
                string serverVersion = "";
                var clientVersion = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                                    Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                                    Assembly.GetExecutingAssembly().GetName().Version.Build;

                try
                {
                    if (string.IsNullOrEmpty(ipAddress) || splitValues.Length != 4)
                    {
                        throw new Exception("IP address is required and must be valid");
                    }

                    var response = await WebRequest.GetWebRequest(ipAddress, "System/Version");
                    var responseBody = await response.Content.ReadAsStringAsync();
                    serverVersion = JsonConvert.DeserializeObject<string>(responseBody);

                    if (serverVersion != clientVersion)
                    {
                        throw new Exception(
                            "Server and client version are not equal, please ensure both server and client versions are the same");

                    }

                    var localData = new LocalData { IpAddress = ipAddress };

                    var jsonLocalData = JsonConvert.SerializeObject(localData);
                    var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    var filePath = Path.Combine(path, localSettingFile);
                    using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
                    {
                        using (var steamWriter = new StreamWriter(file))
                        {
                            steamWriter.Write(jsonLocalData);
                        }
                    }

                    var audioMixer = new Intent(this, typeof(AudioMixer));
                    audioMixer.PutExtra("IPAddress", ipAddress);
                    StartActivity(audioMixer);
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Short).Show();
                }
            };
        }
    }
}

