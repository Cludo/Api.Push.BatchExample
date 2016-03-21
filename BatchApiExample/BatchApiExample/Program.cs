using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Faker;
using Newtonsoft.Json;

namespace BatchApiExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var data = new List<Dictionary<string, object>>();
            for (var i = 0; i < 10; i++)
            {
                var obj = new Dictionary<string, object>
                {
                    ["Id"] = Guid.NewGuid(),
                    ["Type"] = "PageContent",
                    ["Title"] = Lorem.Sentence(),
                    ["Description"] = Lorem.Sentence(),
                    ["Array"] = new[] {Lorem.Sentence(), Lorem.Sentence(), Lorem.Sentence()}
                };
                data.Add(obj);
            }
            var customerId = 1; // YOUR_CUSTOMER_ID
            var contentId = 1; // YOUR_CUSTOMER_ID       
            var customerKey = "CUSTOMER_KEY_GOES_HERE"; //Customer key
            var api = new CludoApi(customerId, customerKey);
            api.PushData(contentId, data);
            api.DeleteData(contentId, data.Select(d=> new Dictionary<string, string>()
            {
                { d["Id"].ToString(), d["Type"].ToString()}
            }));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }
    }

    public class CludoApi
    {
        private const string CludoSearchApiUrl = "https://api.cludo.com/"; 
        private readonly int _customerId;

        private readonly string _customerKey;

        public CludoApi(int customerId, string customerKey)
        {
            _customerId = customerId;
            _customerKey = customerKey;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(CludoSearchApiUrl)
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(_customerId + ":" + _customerKey)));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public void DeleteData(int contentId, IEnumerable<Dictionary<string, string>> data)
        {
            using (var client = GetClient())
            {
                var requestUrl = string.Format("/api/v3/{0}/content/{1}/delete", _customerId, contentId);
                var result = client.PostAsync(requestUrl,
                    new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")).Result;
                if (!result.IsSuccessStatusCode)
                {
                    if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        //TODO: handle invalid objects List<KeyValuePair<Dictionary<string, object>, string>>
                    }
                }
            }
        }

        public void PushData(int contentId, List<Dictionary<string, object>> data)
        {
            using (var client = GetClient())
            {
                var requestUrl = string.Format("/api/v3/{0}/content/{1}/push", _customerId, contentId);
                var result = client.PostAsync(requestUrl,
                    new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")).Result;
                if (!result.IsSuccessStatusCode)
                {
                    if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        //TODO: handle invalid objects List<KeyValuePair<Dictionary<string, object>, string>>
                    }
                }
            }
        }
    }
}