using System;
using System.Net;
using System.Text;

namespace Magma
{
	public class Web
	{
		public string GET(string url)
		{
			string result;
			using (WebClient webClient = new WebClient())
			{
				result = webClient.DownloadString(url);
			}
			return result;
		}

		public string POST(string url, string data)
		{
			string @string;
			using (WebClient webClient = new WebClient())
			{
				webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				byte[] bytes = webClient.UploadData(url, "POST", Encoding.UTF8.GetBytes(data));
				@string = Encoding.UTF8.GetString(bytes);
			}
			return @string;
		}

		public bool DownloadFile(string address, string filename)
		{
			bool flag;
			bool result;
			using (WebClient webClient = new WebClient())
			{
				try
				{
					webClient.DownloadFile(address, filename);
				}
				catch (Exception)
				{
					flag = false;
					result = flag;
					return result;
				}
				flag = true;
			}
			result = flag;
			return result;
		}

		public string UploadFile(string address, string filename)
		{
			string @string;
			using (WebClient webClient = new WebClient())
			{
				byte[] bytes = webClient.UploadFile(address, filename);
				@string = Encoding.UTF8.GetString(bytes);
			}
			return @string;
		}
	}
}
