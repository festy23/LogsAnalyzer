namespace Web.Server;

/// <summary>
///     Реализация веб сервера
/// </summary>
public class RestServer : IRestServer
{
    private readonly HttpListener _listener;
    private readonly ILogsController _logsController;
    private bool _isRunning;
    
    /// <summary>
    /// Конструктор рест сервера 
    /// </summary>
    /// <param name="adress"></param>
    /// <param name="logsController"></param>
    public RestServer(string adress, ILogsController logsController)
    {
        _logsController = logsController;
        _listener = new HttpListener();
        _listener.Prefixes.Add(adress);
    }

    /// <summary>
    ///     Асинхронный запуск сервера
    /// </summary>
    public async Task StartAsync()
    {
        _listener.Start();
        _isRunning = true;
        Console.WriteLine("Сервер запущен на: " + string.Join(", ", _listener.Prefixes));

        while (_isRunning)
        {
            var context = await _listener.GetContextAsync();
            _ = Task.Run(() => ProcessRequest(context));
        }
    }

    /// <summary>
    ///     Остановка сервера
    /// </summary>
    public void Stop()
    {
        _isRunning = false;
        _listener.Stop();
    }

    /// <summary>
    ///     Обработка запроса
    /// </summary>
    /// <param name="context"></param>
    private async Task ProcessRequest(HttpListenerContext context)
    {
        try
        {
            var path = context.Request.Url?.AbsolutePath;
            var method = context.Request.HttpMethod.ToUpperInvariant();

            if (path != null && path.Equals("/logs", StringComparison.OrdinalIgnoreCase))
            {
                if (method == "POST")
                    await _logsController.HandlePost(context);
                else if (method == "GET")
                    await _logsController.HandleGet(context);
                else
                    context.Response.StatusCode = 405;
            }
            else if (path != null && path.Equals("/logs/statistics", StringComparison.OrdinalIgnoreCase) && method == "GET")
            {
                await _logsController.HandleGetStatistics(context);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            var error = "Internal server error: " + ex.Message;
            var buffer = Encoding.UTF8.GetBytes(error);


            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = buffer.Length;


            await context.Response.OutputStream.WriteAsync(buffer);
        }
        finally
        {
            context.Response.OutputStream.Close();
        }
    }
}