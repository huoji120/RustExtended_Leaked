namespace Magma
{
    using System;
    using System.Net;
    using System.Text;

    public class Web
    {
        public bool DownloadFile(string address, string filename)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(address, filename);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        public string GET(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        public string POST(string url, string data)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                byte[] bytes = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(data));
                return Encoding.UTF8.GetString(bytes);
            }
        }

        public string UploadFile(string address, string filename)
        {
            using (WebClient client = new WebClient())
            {
                byte[] bytes = client.UploadFile(address, filename);
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}

