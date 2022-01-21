using System;
using System.Collections.Generic;
//using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
//using System.Web;
using Kayak.Http;

namespace HttpMock
{
    public class EndpointMatchingRule : IMatchingRule
    {
        private readonly HeaderMatch _headerMatch;
        private readonly QueryParamMatch _queryParamMatch;

        public EndpointMatchingRule()
        {
            _headerMatch = new HeaderMatch();
            _queryParamMatch = new QueryParamMatch();
        }

        public bool IsEndpointMatch(IRequestHandler requestHandler, HttpRequestHead request)
        {
            if (requestHandler.QueryParams == null)
                throw new ArgumentException("requestHandler QueryParams cannot be null");

            var requestQueryParams = GetQueryParams(request);
            var requestHeaders = GetHeaders(request);

            bool uriStartsWith = MatchPath(requestHandler, request);

            bool httpMethodsMatch = requestHandler.Method == request.Method;

            bool queryParamMatch = true;
            bool shouldMatchQueryParams = (requestHandler.QueryParams.Count > 0);

            if (shouldMatchQueryParams)
            {
                queryParamMatch = _queryParamMatch.MatchQueryParams(requestHandler, requestQueryParams);
            }

            bool headerMatch = true;
            bool shouldMatchHeaders = requestHandler.RequestHeaders != null
                                      && requestHandler.RequestHeaders.Count > 0;

            if (shouldMatchHeaders)
            {
                headerMatch = _headerMatch.MatchHeaders(requestHandler, requestHeaders);
            }

            return uriStartsWith && httpMethodsMatch && queryParamMatch && headerMatch;
        }

        private static bool MatchPath(IRequestHandler requestHandler, HttpRequestHead request)
        {
            var pathToMatch = request.Uri;
            int positionOfQueryStart = GetStartOfQueryString(request.Uri);
            if (positionOfQueryStart > -1)
            {
                pathToMatch = request.Uri.Substring(0, positionOfQueryStart);
            }

            var requestHandlerHasParam = requestHandler is IRequestHandlerWithParam { HasParam: true };
            
            
            var regex = requestHandlerHasParam ? @"^{0}\/[0-9a-zA-Z\-_.]+$" : @"^{0}\/*$";

            var pathMatch = new Regex(string.Format(regex, Regex.Escape(requestHandler.Path)));
            return pathMatch.IsMatch(pathToMatch);
        }

        

        private static Dictionary<string, string> GetQueryParams(HttpRequestHead request)
        {
            /*string queryString = request.Uri.Substring(positionOfQueryStart);
            NameValueCollection valueCollection = HttpUtility.ParseQueryString(queryString);
            var requestQueryParams = valueCollection.AllKeys
                .Where(k => !string.IsNullOrEmpty(k))
                .ToDictionary(k => k, k => valueCollection[k]);*/
            var requestQueryParams = request.DecodeQueryParameters();
            return requestQueryParams;
        }

        private static IDictionary<string, string> GetHeaders(HttpRequestHead request)
        {
            return request.Headers ?? new Dictionary<string, string>();
        }
        
        private static int GetStartOfQueryString(string uri)
        {
            return uri.LastIndexOf('?');
        }
    }

    public static class UriHelper
    {
        public static Dictionary<string, string> DecodeQueryParameters(this HttpRequestHead request)
        {
            var positionOfQueryStart = request.Uri.LastIndexOf('?');
            if (positionOfQueryStart < 1)
                return new Dictionary<string, string>();

            var paramString = request.Uri.Substring(positionOfQueryStart);
            
            if (paramString.Length == 0)
                return new Dictionary<string, string>();

            return paramString.TrimStart('?')
                .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(parts => parts[0],
                    parts => parts.Length > 2
                        ? string.Join("=", parts, 1, parts.Length - 1)
                        : (parts.Length > 1 ? parts[1] : ""))
                .ToDictionary(grouping => grouping.Key,
                    grouping => string.Join(",", grouping).ToLower());
        }
    }
}