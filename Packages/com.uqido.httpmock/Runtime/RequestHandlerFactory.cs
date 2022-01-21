using System.Collections.Generic;

namespace HttpMock
{
    public class RequestHandlerFactory
    {
        private readonly IRequestProcessor _requestProcessor;

        public RequestHandlerFactory(IRequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }

        public RequestHandler Get(string path, bool withParam = false)
        {
            return CreateHandler(path, "GET");
        }

        public RequestHandler Patch(string path, bool withParam = false)
        {
            return CreateHandler(path, "PATCH");
        }

        public RequestHandler Post(string path, bool withParam = false)
        {
            return CreateHandler(path, "POST");
        }

        public RequestHandler Put(string path, bool withParam = false)
        {
            return CreateHandler(path, "PUT");
        }

        public RequestHandler Delete(string path, bool withParam = false)
        {
            return CreateHandler(path, "DELETE");
        }

        public RequestHandler Head(string path, bool withParam = false)
        {
            return CreateHandler(path, "HEAD");
        }

        public RequestHandler CustomVerb(string path, string verb, bool withParam = false)
        {
            return CreateHandler(path, verb);
        }

        public void ClearHandlers()
        {
            new List<RequestHandler>();
        }


        private RequestHandler CreateHandler(string path, string method, bool withParam = false)
        {
            string cleanedPath = path;
            var requestHandler = withParam
                ? new RequestHandlerWithParam(cleanedPath, _requestProcessor) { Method = method, HasParam = true }
                : new RequestHandler(cleanedPath, _requestProcessor) { Method = method };
            return requestHandler;
        }
    }
}