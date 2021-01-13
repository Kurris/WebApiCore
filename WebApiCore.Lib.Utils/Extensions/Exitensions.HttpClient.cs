using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace WebApiCore.Lib.Utils.Extensions
{
    public static partial class Extensions
    {
        private static HttpClient SetDefault(this IHttpClientFactory factory)
        {
            HttpClient client = factory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(GlobalInvariant.SystemConfig.HttpClientConfig.Timeout);
            if (!string.IsNullOrEmpty(GlobalInvariant.SystemConfig.HttpClientConfig.BaseUrl))
            {
                client.BaseAddress = new Uri(GlobalInvariant.SystemConfig.HttpClientConfig.BaseUrl);
            }
            
            return client;
        }

        public static HttpClient GetClient(this IHttpClientFactory factory)
        {
            HttpClient client = factory.SetDefault();
            return client;
        }

        public static HttpClient GetClient(this IHttpClientFactory factory, IDictionary<string, string> headerConfig)
        {
            HttpClient client = factory.SetDefault();

            foreach (var item in headerConfig)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            return client;
        }
    }
}
