using System;
using System.Net.Http;
namespace DBManager
{
    public static class DBController
    {
        public static async void WriteToDB(string urlPath, string data)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, "https://chasers-gambit-425703-default-rtdb.firebaseio.com/"+urlPath);
            var content = new StringContent(data, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}