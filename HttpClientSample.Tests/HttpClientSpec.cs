using System;
using NUnit.Framework;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using ModernHttpClient;

namespace HttpClientSample.Tests
{
	class Single
	{
		public string Title { set; get; }
		public string Genre { set; get; }
	}

	[TestFixture]
	public class HttpClientSpec
	{
		private IEnumerable<Single>  ConnectToSoundcloud (HttpClientHandler handler)
		{
			var url = "http://api.soundcloud.com/users/67393202/tracks.json?client_id=0be8085a39603d77fbf672a62a7929ea";
			var client = new HttpClient (handler);
			var result = client.GetStringAsync (url).Result;
			var obj = JsonConvert.DeserializeObject<List<Dictionary<string, object>>> (result).Select (x => new Single {
				Title = string.Format ("{0}", x ["title"] ?? ""),
				Genre = string.Format ("{0}", x ["genre"] ?? "")
			});
			return obj;
		}

		[Test]
		public void ShouldConnectoToSoundcloud ()
		{
			var handler = new HttpClientHandler { };
			var result = ConnectToSoundcloud (handler);
			var genesis = result.Where (x => x.Genre.ToString() == "GENESIS").Count ();
			Assert.AreEqual (1, genesis);
			Assert.IsTrue (result.Count () > 0);
		}

		[Test]
		public void ShouldConnectToSoundcloudWithModernHttpClient ()
		{
			var handler = new NativeMessageHandler ();
			var result = ConnectToSoundcloud (handler);
			var genesis = result.Where (x => x.Genre.ToString() == "GENESIS").Count ();
			Assert.AreEqual (1, genesis);
			Assert.IsTrue (result.Count () > 0);
		}

		[Test]
		public void ShouldConnectToGoogle()
		{
			var url = "https://www.google.com";
			var client = new HttpClient ();
			var rs = client.GetAsync (url).Result;
			var status = rs.StatusCode;
			Assert.AreEqual (status, HttpStatusCode.OK);
		}

		[Test]
		public void ShouldConnectToGoogleWithModernHttpClient ()
		{
			var url = "https://www.google.com";
			var client = new HttpClient (new NativeMessageHandler ());
			var rs = client.GetAsync (url).Result;
			var status = rs.StatusCode;
			Assert.AreEqual (status, HttpStatusCode.OK);
		}
	}
}

