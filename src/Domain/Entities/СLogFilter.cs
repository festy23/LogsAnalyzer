namespace Domain.Entities;

/// <summary>
///     Набор критериев для фильтрации логов
/// </summary>
/// <param name="Level">Уровень важности</param>
/// <param name="StartDate">Начало лога</param>
/// <param name="EndDate">Конец лога</param>
/// <param name="Keyword">Ключевое слово в сообщении</param>
/// <param name="CaseSensitive">Чувствительность ключевого слова к регистру</param>
public record CLogFilter(
    string? Level = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? Keyword = null,
    bool CaseSensitive = false
);