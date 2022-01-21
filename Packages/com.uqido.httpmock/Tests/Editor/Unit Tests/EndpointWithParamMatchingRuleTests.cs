using System;
using System.Collections.Generic;
using Kayak.Http;
using NUnit.Framework;
using Rhino.Mocks;
using UnityEngine;

namespace HttpMock.Unit.Tests
{
	[TestFixture]
	public class EndpointWithParamMatchingRuleTests
	{
		[Test]
		public void urls_with_alphanumeric_param_match_it_returns_true()
		{
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.HasParam = true;
			requestHandler.QueryParams = new Dictionary<string, string>();

			var httpRequestHead = new HttpRequestHead { Uri = "test/iamstringparam" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}
		
		[Test]
		public void urls_with_alphanumeric_and_special_separators_param_match_it_returns_true()
		{
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.HasParam = true;
			requestHandler.QueryParams = new Dictionary<string, string>();

			var httpRequestHead = new HttpRequestHead { Uri = "test/i-am_string__param--with_-separators" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}
		
		[Test]
		public void urls_with_integer_param_match_it_returns_true()
		{
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.HasParam = true;
			requestHandler.QueryParams = new Dictionary<string, string>();

			var httpRequestHead = new HttpRequestHead { Uri = "test/1234" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}
		
		[Test]
		public void urls_with_float_param_match_it_returns_true()
		{
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.HasParam = true;
			requestHandler.QueryParams = new Dictionary<string, string>();

			var httpRequestHead = new HttpRequestHead { Uri = "test/1234.567" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}
		
		[Test]
		public void urls_with_guid_param_match_it_returns_true()
		{
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.HasParam = true;
			requestHandler.QueryParams = new Dictionary<string, string>();
			var httpRequestHead = new HttpRequestHead { Uri = $"test/{Guid.NewGuid().ToString()}" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}
	}
	
	
	
	[TestFixture]
	public class EndpointWithParamMatchingRuleNonRegressionTests
	{
		[Test]
		public void urls_match_it_returns_true()
		{
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.QueryParams = new Dictionary<string, string>();

			var httpRequestHead = new HttpRequestHead { Uri = "test" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void urls_and_methods_the_same_it_returns_true() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "PUT";
			requestHandler.QueryParams = new Dictionary<string, string>();

			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "PUT" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void urls_and_methods_differ_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>();
			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "PUT" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}

		[Test]
		public void urls_differ_and_methods_match_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "pest";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>();
			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}

		[Test]
		public void urls_and_methods_match_queryparams_differ_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "myParam", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}

		[Test]
		public void urls_and_methods_match_and_queryparams_exist_it_returns_true() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "myParam", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test?oauth_consumer_key=test-api&elvis=alive&moonlandings=faked&myParam=one", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void urls_and_methods_match_and_queryparams_does_not_exist_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "myParam", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test?oauth_consumer_key=test-api&elvis=alive&moonlandings=faked", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}


		[Test]
		public void urls_and_methods_match_and_no_query_params_are_set_but_request_has_query_params_returns_true()
		{
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> ();

			var httpRequestHead = new HttpRequestHead { Uri = "test?oauth_consumer_key=test-api&elvis=alive&moonlandings=faked", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();

			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.True);
		}

		[Test]
		public void urls_and_methods_and_queryparams_match_it_returns_true() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>{{"myParam", "one"}};

			var httpRequestHead = new HttpRequestHead { Uri = "test?myParam=one", Method = "GET" };
			
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void urls_and_methods_match_headers_differ_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>();
			requestHandler.RequestHeaders = new Dictionary<string, string> { { "myHeader", "one" } };

			var httpRequestHead = new HttpRequestHead
			{
				Uri = "test",
				Method = "GET",
				Headers = new Dictionary<string, string>
				{
					{ "myHeader", "two" }
				}
			};
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}

		[Test]
		public void urls_and_methods_match_and_headers_match_it_returns_true() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>();
			requestHandler.RequestHeaders = new Dictionary<string, string> { { "myHeader", "one" } };

			var httpRequestHead = new HttpRequestHead
			{
				Uri = "test",
				Method = "GET",
				Headers = new Dictionary<string, string>
				{
					{ "myHeader", "one" },
					{ "anotherHeader", "two" }
				}
			};
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void urls_and_methods_match_and_header_does_not_exist_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>();
			requestHandler.RequestHeaders = new Dictionary<string, string> { { "myHeader", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}

		[Test]
		public void should_do_a_case_insensitive_match_on_query_string_parameter_values() {

			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "myParam", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test?myParam=OnE", Method = "GET" };

			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void should_do_a_case_insensitive_match_on_header_names_and_values() {

			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>();
			requestHandler.RequestHeaders = new Dictionary<string, string> { { "myHeader", "one" } };

			var httpRequestHead = new HttpRequestHead
			{
				Uri = "test",
				Method = "GET",
				Headers = new Dictionary<string, string>
				{
					{ "MYheaDER", "OnE" }
				}
			};

			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void should_match_when_the_query_string_has_a_trailing_ampersand()
		{

			var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "a", "b" } ,{"c","d"}};

			var httpRequestHead = new HttpRequestHead { Uri = "test?a=b&c=d&", Method = "GET" };

			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
			
		}

        [Test]
        public void should_match_urls_containings_regex_reserved_characters()
        {
            var requestHandler = MockRepository.GenerateStub<IRequestHandlerWithParam>();
            requestHandler.Path = "/test()";
            requestHandler.QueryParams = new Dictionary<string, string>();

            var httpRequestHead = new HttpRequestHead { Uri = "/test()" };
            var endpointMatchingRule = new EndpointMatchingRule();
            Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
        }
	}
}