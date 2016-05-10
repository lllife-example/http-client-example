using System;
using NUnit.Framework;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HttpClientSample.Tests
{
	[TestFixture]
	public class HttpClientSpec
	{
		[Test]
		public void ShouldConnectToSoundcloud ()
		{
			var url = "http://api.soundcloud.com/users/67393202/tracks.json?client_id=0be8085a39603d77fbf672a62a7929ea";
			var handler = new HttpClientHandler { };
			var client = new HttpClient (handler);
			var result = client.GetStringAsync (url).Result;
			var obj = JsonConvert.DeserializeObject<List<Dictionary<string,object>>> (result).Select (x => new {
				Title = x["title"],
				Genre = x["genre"]
			});

			var genesis = obj.Where (x => x.Genre.ToString() == "GENESIS").Count ();
			Assert.AreEqual (1, genesis);
			Assert.IsTrue (obj.Count () > 0);
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
	}
}

