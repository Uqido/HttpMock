namespace HttpMock
{
    public class RequestHandlerWithParam : RequestHandler
    {
        public RequestHandlerWithParam(string path, IRequestProcessor requestProcessor) : base(path, requestProcessor)
        {
        }
    }
}