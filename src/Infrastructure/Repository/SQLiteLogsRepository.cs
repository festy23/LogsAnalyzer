using Microsoft.Data.Sqlite;

namespace Infrastructure.Repository;
/// <summary>
/// Класс - реализация репозитория с помощью SqLite. Для написания шаблона этого класса я использовал ИИ:
/// Промпт: "Напиши мне шаблон репозитория на си шарп, используя sqlite. Он должен реализовать мой ILogsRepository интерфейс.
/// В сообщении я отправил ему содержимое ILogsRepository.
/// </summary>
public class SqliteLogsRepository : ILogsRepository
{
    private const string DefaultDbFile = "logs.db";
    private readonly string _connectionString =
        "Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), "logs.db");

    public SqliteLogsRepository()
    {
        InitializeDatabase();
    }

    /// <summary>
    ///     Получение всех логов из базы.
    /// </summary>
    public List<CLog> GetLogs()
    {
        var logs = new List<CLog>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var query = "SELECT DateAndTime, LevelOfImportance, Message FROM Logs";
        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = command.ExecuteReader();

        while (reader.Read()) logs.Add(MapLog(reader));

        return logs;
    }

    /// <summary>
    ///     Добавление одного лога.
    /// </summary>
    public void AddLog(CLog log)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var query = @"INSERT INTO Logs (DateAndTime, LevelOfImportance, Message) 
                            VALUES (@DateAndTime, @Level, @Message)";

        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@DateAndTime", log.DateAndTime.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Level", log.LevelOfImportance);
        command.Parameters.AddWithValue("@Message", log.Message);
        command.ExecuteNonQuery();
    }

    /// <summary>
    ///     Добавление списка логов.
    /// </summary>
    public void AddLogs(List<CLog> logs)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();
        foreach (var log in logs)
        {
            var query = @"
                    INSERT INTO Logs (DateAndTime, LevelOfImportance, Message)
                    VALUES (@DateAndTime, @Level, @Message)";
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@DateAndTime", log.DateAndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@Level", log.LevelOfImportance);
            command.Parameters.AddWithValue("@Message", log.Message);
            command.ExecuteNonQuery();
        }

        transaction.Commit();
    }

    /// <summary>
    ///     Фильтрация логов по заданному фильтру.
    /// </summary>
    public List<CLog> GetLogsByFilter(CLogFilter filter)
    {
        var logs = new List<CLog>();

        // Проверяем, существует ли файл БД
        if (!File.Exists(DefaultDbFile))
            throw new FileNotFoundException("База данных не найдена.", DefaultDbFile);

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var sb = new StringBuilder("SELECT DateAndTime, LevelOfImportance, Message FROM Logs WHERE 1=1");
        if (!string.IsNullOrEmpty(filter.Level))
            sb.Append(" AND LevelOfImportance = @Level");
        if (filter.StartDate.HasValue)
            sb.Append(" AND DateAndTime >= @StartDate");
        if (filter.EndDate.HasValue)
            sb.Append(" AND DateAndTime <= @EndDate");
        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            if (filter.CaseSensitive)
                sb.Append(" AND Message LIKE '%' || @Keyword || '%'");
            else
                sb.Append(" AND LOWER(Message) LIKE '%' || LOWER(@Keyword) || '%'");
        }

        using var command = connection.CreateCommand();
        command.CommandText = sb.ToString();
        if (!string.IsNullOrEmpty(filter.Level))
            command.Parameters.AddWithValue("@Level", filter.Level);
        if (filter.StartDate.HasValue)
            command.Parameters.AddWithValue("@StartDate", filter.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        if (filter.EndDate.HasValue)
            command.Parameters.AddWithValue("@EndDate", filter.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        if (!string.IsNullOrEmpty(filter.Keyword))
            command.Parameters.AddWithValue("@Keyword", filter.Keyword);

        using var reader = command.ExecuteReader();
        while (reader.Read()) logs.Add(MapLog(reader));

        return logs;
    }

    /// <summary>
    ///     Очистка всех логов из базы.
    /// </summary>
    public void ClearLogs()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM Logs";
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }

    /// <summary>
    ///     Сохранение отфильтрованных логов в новую таблицу в той же базе.
    ///     Если targetTableName пустое, используется "ExportedLogs".
    /// </summary>
    public void SaveFilteredLogs(IEnumerable<CLog> logs, string targetTableName)
    {
        if (!File.Exists(DefaultDbFile))
            throw new FileNotFoundException("База данных не найдена.", DefaultDbFile);

        if (string.IsNullOrWhiteSpace(targetTableName))
            targetTableName = "ExportedLogs";

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Создаем таблицу для сохранения отфильтрованных логов, если ее нет
        var createTableQuery = $@"
                CREATE TABLE IF NOT EXISTS {targetTableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DateAndTime TEXT NOT NULL,
                    LevelOfImportance TEXT NOT NULL,
                    Message TEXT NOT NULL
                );";
        using (var command = connection.CreateCommand())
        {
            command.CommandText = createTableQuery;
            command.ExecuteNonQuery();
        }

        // Вставляем логи в новую таблицу
        using var transaction = connection.BeginTransaction();
        foreach (var log in logs)
        {
            var insertQuery = $@"
                    INSERT INTO {targetTableName} (DateAndTime, LevelOfImportance, Message)
                    VALUES (@DateAndTime, @Level, @Message)";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = insertQuery;
                command.Parameters.AddWithValue("@DateAndTime", log.DateAndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Level", log.LevelOfImportance);
                command.Parameters.AddWithValue("@Message", log.Message);
                command.ExecuteNonQuery();
            }
        }

        transaction.Commit();
    }
    /// <summary>
    /// Получение статистики логов из БД
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public LogStatistics GetStatistics(DateTime? from = null, DateTime? to = null)
    {
        int totalMessages = 0;
        var logLevelsCount = new Dictionary<string, int>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var sbWhere = new StringBuilder("WHERE 1=1");
        if (from.HasValue)
        {
            sbWhere.Append(" AND DateAndTime >= @FromDate");
        }
        if (to.HasValue)
        {
            sbWhere.Append(" AND DateAndTime <= @ToDate");
        }
        var whereClause = sbWhere.ToString();

        var totalQuery = "SELECT COUNT(*) FROM Logs " + whereClause;
        using (var totalCommand = connection.CreateCommand())
        {
            totalCommand.CommandText = totalQuery;
            if (from.HasValue)
                totalCommand.Parameters.AddWithValue("@FromDate", from.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (to.HasValue)
                totalCommand.Parameters.AddWithValue("@ToDate", to.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            totalMessages = Convert.ToInt32(totalCommand.ExecuteScalar());
        }

        string groupQuery = "SELECT LevelOfImportance, COUNT(*) AS Count FROM Logs " + whereClause + " GROUP BY LevelOfImportance";
        using (var groupCommand = connection.CreateCommand())
        {
            groupCommand.CommandText = groupQuery;
            if (from.HasValue)
                groupCommand.Parameters.AddWithValue("@FromDate", from.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (to.HasValue)
                groupCommand.Parameters.AddWithValue("@ToDate", to.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            using var reader = groupCommand.ExecuteReader();
            while (reader.Read())
            {
                string? level = reader["LevelOfImportance"].ToString();
                int count = Convert.ToInt32(reader["Count"]);
                if (level != null) logLevelsCount[level] = count;
            }
        }

        return new LogStatistics
        {
            TotalMessages = totalMessages,
            LogLevelsCount = logLevelsCount
        };
    }


    /// <summary>
    ///     Создает таблицу Logs, если она отсутствует.
    /// </summary>
    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Logs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DateAndTime TEXT NOT NULL,
                    LevelOfImportance TEXT NOT NULL,
                    Message TEXT NOT NULL
                );";

        using var command = connection.CreateCommand();
        command.CommandText = createTableQuery;
        command.ExecuteNonQuery();
    }

    /// <summary>
    ///     Преобразует строку результата запроса в объект CLog.
    /// </summary>
    private CLog MapLog(SqliteDataReader reader)
    {
        var dateAndTime = DateTime.ParseExact(
            reader["DateAndTime"].ToString() ?? string.Empty,
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture);
        var level = reader["LevelOfImportance"].ToString();
        var message = reader["Message"].ToString();
        return new CLog(dateAndTime, level, message);
    }
}