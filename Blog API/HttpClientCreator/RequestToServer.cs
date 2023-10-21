using System.Text.Json;

namespace Blog_API.HttpClientCreator
    //*** This Service not added In IoC Container
{
    public class RequestToServer
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RequestToServer(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;  
        }

        public async Task RequestToExample()
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("Example Url"),
                };
                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

                Stream stream = httpResponse.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(stream);

                string response = streamReader.ReadToEnd();

                //if u want to conver json to dictionary or something
                Dictionary<string,object> responseDictionary =  JsonSerializer.Deserialize<Dictionary<string,object>>(response);
            }
        }
    }
}
