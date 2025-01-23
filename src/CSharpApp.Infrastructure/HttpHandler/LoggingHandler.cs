namespace CSharpApp.Infrastructure.HttpHandler
{
    public class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            // Log request details
            Console.WriteLine($"[Request] Method: {request.Method}, URL: {request.RequestUri}");

            if (request.Content != null)
            {
                var requestContent = await request.Content.ReadAsStringAsync();
                Console.WriteLine($"[Request] Content: {requestContent}");
            }

            // Send the request
            var response = await base.SendAsync(request, cancellationToken);

            stopwatch.Stop();

            // Log response details
            Console.WriteLine($"[Response] Status Code: {response.StatusCode}, Elapsed Time: {stopwatch.ElapsedMilliseconds}ms");

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Response] Content: {responseContent}");
            }

            return response;
        }
    }
}
