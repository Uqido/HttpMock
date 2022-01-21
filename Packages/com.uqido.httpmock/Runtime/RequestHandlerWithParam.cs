namespace HttpMock
{
    public class RequestHandlerWithParam : RequestHandler, IRequestHandlerWithParam
    {
        public bool HasParam { get; set; }
        
        public RequestHandlerWithParam(string path, IRequestProcessor requestProcessor) : base(path, requestProcessor)
        {
        }
    }
}