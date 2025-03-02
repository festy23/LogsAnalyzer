namespace Web.Controllers;
/// <summary>
/// Класс - реализация контроллера для сервера
/// </summary>
/// <param name="addLogUseCase"></param>
/// <param name="getLogsUseCase"></param>
/// <param name="getLogsStatisticsUseCase"></param>
public class LogsController(
    AddLogUseCase addLogUseCase,
    GetLogsUseCase getLogsUseCase,
    GetLogsStatisticsUseCase getLogsStatisticsUseCase)
    : ILogsController
{

    public async Task HandlePost(HttpListenerContext context)
    {
        string body;
        using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
        {
            body  = await reader.ReadToEndAsync();
        }

        var logDto = JsonConvert.DeserializeObject<CLogDto>(body);
        if (logDto == null)
        {
            context.Response.StatusCode = 400;
            return;
        }
        addLogUseCase.Execute(logDto);

        context.Response.StatusCode = 201;
        context.Response.ContentLength64 = 0;
    }

    public async Task HandleGet(HttpListenerContext context)
    {
        var query = HttpUtility.ParseQueryString(context.Request.Url?.Query ?? string.Empty);
        DateTime? from = null, to = null;
        if (DateTime.TryParse(query["from"], out DateTime fromDate))
            from = fromDate;
        if (DateTime.TryParse(query["to"], out DateTime toDate))
            to = toDate;

        var logsDto = getLogsUseCase.Execute(from, to);
        string json = JsonConvert.SerializeObject(logsDto);
        byte[] buffer = Encoding.UTF8.GetBytes(json);

        context.Response.ContentType = "application/json";
        context.Response.ContentLength64 = buffer.Length;
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }
    
    public async Task HandleGetStatistics(HttpListenerContext context)
    {
        var query = HttpUtility.ParseQueryString(context.Request.Url?.Query ?? string.Empty);
        DateTime? from = null, to = null;
    
        if (DateTime.TryParse(query["from"], out DateTime fromDate))
            from = fromDate;
        if (DateTime.TryParse(query["to"], out DateTime toDate))
            to = toDate;
    
        var dto = new CLogsStatisticsDto(from, to);
    
        var stats = await getLogsStatisticsUseCase.Execute(dto);
        string json = JsonConvert.SerializeObject(stats);
        byte[] buffer = Encoding.UTF8.GetBytes(json);
    
        context.Response.ContentType = "application/json";
        context.Response.ContentLength64 = buffer.Length;
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }
}