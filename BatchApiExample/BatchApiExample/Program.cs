using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;

namespace BatchApiExample
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
           var data = new List<Dictionary<string, object>>();
            for (var i = 0; i < 10; i++)
            {
                var obj = new Dictionary<string, object>
                {
                    ["Id"] = Guid.NewGuid(),
                    ["Type"] = "PageContent",
                    ["Title"] = Faker.Lorem.Sentence(),
                    ["Description"] = Faker.Lorem.Sentence(),
                    ["Array"] = new[] {Faker.Lorem.Sentence(), Faker.Lorem.Sentence(), Faker.Lorem.Sentence()}
                };
                data.Add(obj);
            }
            var customerId = -1; // YOUR_CUSTOMER_ID
            var contentId = -1; // YOUR_CUSTOMER_ID                
            var api = new CludoApi(customerId);
            api.PushData(contentId, data);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            
        }
    }

    public class CludoApi
    {
        private readonly int _customerId;

        public CludoApi(int customerId)
        {
            this._customerId = customerId;
        }

        private const string CludoSearchApiUrl = "http://search.local/"; // Add this url to your config file as well

        private HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(CludoSearchApiUrl)
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(_customerId + ":" + CustomerKey)));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private string CustomerKey
        {
            get { return ConfigurationManager.AppSettings["Cludo.CustomerKey"]; }
        }

        public void PushData(int contentId, List<Dictionary<string, object>> data)
        {
            using (var client = GetClient())
            {
                var requestUrl = string.Format("/api/{0}/content/{1}/push", _customerId, contentId);
                var result = client.PostAsync(requestUrl,
                        new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")).Result;
                if (!result.IsSuccessStatusCode)
                {
                    if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        //TODO: handle invalid objects List<KeyValuePair<Dictionary<string, object>, string>>
                    }
                    else
                    {
                        //TODO: log exeption
                    }
                }
                
            }
        }
    }
}
