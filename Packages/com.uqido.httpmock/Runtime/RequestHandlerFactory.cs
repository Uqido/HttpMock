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
            return CreateHandler(path, "GET", withParam);
        }

        public RequestHandler Patch(string path, bool withParam = false)
        {
            return CreateHandler(path, "PATCH", withParam);
        }

        public RequestHandler Post(string path, bool withParam = false)
        {
            return CreateHandler(path, "POST", withParam);
        }

        public RequestHandler Put(string path, bool withParam = false)
        {
            return CreateHandler(path, "PUT", withParam);
        }

        public RequestHandler Delete(string path, bool withParam = false)
        {
            return CreateHandler(path, "DELETE", withParam);
        }

        public RequestHandler Head(string path, bool withParam = false)
        {
            return CreateHandler(path, "HEAD", withParam);
        }

        public RequestHandler CustomVerb(string path, string verb, bool withParam = false)
        {
            return CreateHandler(path, verb, withParam);
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