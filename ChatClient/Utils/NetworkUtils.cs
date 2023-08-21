using System.Net;
using System.Net.Http.Headers;

namespace ChatClient.Utils;
public class NetworkUtils
{


    private async Task<HttpResponseMessage> PostAsync(string url,string json)
    {
        HttpClient client = new HttpClient();
        HttpContent content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await client.PostAsync(url, content);
        return response;
    }

    public async Task<HttpStatusCode> SendRegister(string json)
    {
        App.Current.Resources.TryGetValue("serverLocation", out object loc);
        string path = loc.ToString() + "/api/user/register";
        var res = await PostAsync(path, json);
        return res.StatusCode;
    }
}


