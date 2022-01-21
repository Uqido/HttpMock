namespace HttpMock
{
    public interface IRequestHandlerWithParam : IRequestHandler
    {
        bool HasParam { get; set; }
    }
}