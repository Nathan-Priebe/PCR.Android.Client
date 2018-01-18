﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace PCR.Client.Android
{
    class Common
    {
        public static async Task<HttpResponseMessage> GetWebRequest(string ipAddress, string endPoint)
        {
            var httpClient = new HttpClient(new NativeMessageHandler());
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            var reqTask = httpClient.GetAsync(ipAddress + endPoint, cts.Token);
            HttpResponseMessage response;
            try
            {
                if (Task.WaitAny(new Task[] { reqTask }, 30000) < 0)
                {
                    cts.Cancel(); // attempt to cancel the HTTP request
                    throw new Exception("The network request timed out. Please check your network connection.");
                }
                response = reqTask.GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static async Task<HttpResponseMessage> PostWebRequest<T>(string ipAddress, string endPoint, T body)
        {
            var output = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(output, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient(new NativeMessageHandler());
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            var reqTask = httpClient.PostAsync(ipAddress + endPoint, httpContent, cts.Token);
            HttpResponseMessage response;

            try
            {
                if (Task.WaitAny(new Task[] { reqTask }, 30000) < 0)
                {
                    cts.Cancel(); // attempt to cancel the HTTP request
                    throw new Exception("The network request timed out. Please check your network connection.");
                }
                response = reqTask.GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}