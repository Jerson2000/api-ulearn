using System.Net.WebSockets;

namespace ULearn.Api.Middlewares
{
    public class WebsocketMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/ws", StringComparison.OrdinalIgnoreCase))
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    await HandleWebSocketRequest(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
                return; 
            }

            await _next(context);
        }

        private static async Task HandleWebSocketRequest(HttpContext context)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await EchoAsync(webSocket);
        }

        private static async Task EchoAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
                else
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
            }
        }
    }

    public static class WebsocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebsocketMiddleware(this IApplicationBuilder app)
        {
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            };

            app.UseWebSockets(webSocketOptions);

            return app.UseMiddleware<WebsocketMiddleware>();
        }
    }
}
