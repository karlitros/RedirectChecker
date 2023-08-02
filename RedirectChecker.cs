using System.Net.Http.Headers;
using System.Xml;

namespace RedirectChecker
{
    public class RedirectChecker
    {
        readonly string? _redirectFilePath;
        readonly string? _rootUrl;

        public RedirectChecker(string redirectFilePath, string rootUrl)
        {
            _redirectFilePath = redirectFilePath;
            _rootUrl = rootUrl;
        }        

        public void ProcessRepositories()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var file = File.Open(_redirectFilePath, FileMode.Open);

            XmlDocument xmlDoc = new XmlDocument();

            Console.WriteLine($"Loading your redirect file {_redirectFilePath}\r\n\r\n");
            xmlDoc.Load(file);

            var list = xmlDoc.GetElementsByTagName("add");

            Console.WriteLine($"Checking your redirects on {_rootUrl}\r\n\r\n");

            for (int x = 0; x < list.Count; x++)
            {
                var item = list[x];

                if(item == null)
                {
                    continue;
                }

                var originalUrl = item.Attributes["key"];
                var destinationurl = item.Attributes["value"];

                if(originalUrl == null || destinationurl == null)
                {
                    continue;
                }

                var statusCode = client.GetAsync(_rootUrl + originalUrl.Value).Result.StatusCode;

                if(statusCode.ToString().ToLower().Equals("notfound"))
                {
                    Console.Write($"Couldn't redirect {originalUrl.Value} to {destinationurl.Value}\r\n");
                }
            }
        }
    }
}
