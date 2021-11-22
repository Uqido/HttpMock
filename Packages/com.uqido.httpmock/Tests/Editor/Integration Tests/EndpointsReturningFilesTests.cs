using System;
using System.IO;
using System.Net;
using NUnit.Framework;
using UnityEngine;

namespace HttpMock.Integration.Tests
{
	[TestFixture]
	public class EndpointsReturningFilesTests
	{
		private const string FILE_PATH = "Packages/com.uqido.httpmock/Tests/Resources/MyTestFile.txt";

		[Test]
		public void A_Setting_return_file_return_the_correct_content_length() {
			var stubHttp = HttpMockRepository.At("http://localhost.:9191");

			var file = Path.GetFullPath(FILE_PATH);

			UnityEngine.Debug.Log(file);
			stubHttp.Stub(x => x.Get("/afile"))
				.ReturnFile(file)
				.OK();

			Console.WriteLine(stubHttp.WhatDoIHave());
            
			var fileLength = new FileInfo(file).Length;

			var webRequest = (HttpWebRequest) WebRequest.Create("http://localhost.:9191/afile");
			using (var response = webRequest.GetResponse())
			using(var responseStream = response.GetResponseStream())
			{
			
				var bytes = new byte[response.ContentLength];
				responseStream.Read(bytes, 0, (int) response.ContentLength);
				
				Assert.That(response.ContentLength, Is.EqualTo(fileLength));
			}
		}

	}
}