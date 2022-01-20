using System.Collections.Generic;

namespace HttpMock
{
	public class RequestHandlerFactory
	{
		private readonly IRequestProcessor _requestProcessor;

		public RequestHandlerFactory(IRequestProcessor requestProcessor) {
			_requestProcessor = requestProcessor;
		}

		public RequestHandler Get(string path) {
			return CreateHandler(path, "GET");
		}
		
		public RequestHandler Patch(string path)
		{
			return CreateHandler(path, "PATCH");
		}

		public RequestHandler Post(string path) {
			return CreateHandler(path, "POST");
		}

		public RequestHandler Put(string path) {
			return CreateHandler(path, "PUT");
		}

		public RequestHandler Delete(string path) {
			return CreateHandler(path, "DELETE");
		}

		public RequestHandler Head(string path) {
			return CreateHandler(path, "HEAD");
		}

		public RequestHandler CustomVerb(string path, string verb) {
			return CreateHandler(path, verb);
		}
		
		public RequestHandler GetWithParam(string path) {
			return CreateHandlerWithParam(path, "GET");
		}
		
		public RequestHandler PatchWithParam(string path)
		{
			return CreateHandlerWithParam(path, "PATCH");
		}

		public RequestHandler PostWithParam(string path) {
			return CreateHandlerWithParam(path, "POST");
		}

		public RequestHandler PutWithParam(string path) {
			return CreateHandlerWithParam(path, "PUT");
		}

		public RequestHandler DeleteWithParam(string path) {
			return CreateHandlerWithParam(path, "DELETE");
		}

		public RequestHandler HeadWithParam(string path) {
			return CreateHandlerWithParam(path, "HEAD");
		}

		public RequestHandler CustomVerbWithParam(string path, string verb) {
			return CreateHandlerWithParam(path, verb);
		}

		public void ClearHandlers() {
			new List<RequestHandler>();
		}



		private RequestHandler CreateHandler(string path, string method) {
			string cleanedPath = path;
			var requestHandler = new RequestHandler(cleanedPath, _requestProcessor) {Method = method};
			return requestHandler;
		}
		
		private RequestHandler CreateHandlerWithParam(string path, string method) {
			string cleanedPath = path;
			var requestHandler = new RequestHandlerWithParam(cleanedPath, _requestProcessor) {Method = method};
			return requestHandler;
		}
	}

}