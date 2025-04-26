# **LogAnalyzer**

## **Описание проекта**

LogAnalyzer – это консольное приложение и REST API сервер для управления логами с использованием **SQLite**. Проект построен на принципе Clean Architecture.
Шаблон для архитектуры взят с https://github.com/jasontaylordev/CleanArchitecture/tree/main

 **Разработал:** Коновалов Иван Андреевич, студент БПИ 2410 ВШЭ ФКН ПИ

---

## **1. Архитектура проекта**

Проект следует **чистой архитектуре (Clean Architecture)** и разделён на 4 основных слоя:

### **1.1. Domain (Слой предметной области)**

- содержит основные бизнес-модели и интерфейсы, определяющие логику работы с логами.

Основные файлы:

- `CLog.cs` – сущность лог-сообщения (дата, уровень важности, сообщение).
- `CLogFilter.cs` – **фильтр** для поиска логов.
- `LogStatistics.cs` – **модель статистики** логов.
---

### **1.2. Application (Слой бизнес-логики)**

- отвечает за бизнес-логику проекта.

**Основные файлы:**

- `UseCases/` – **UseCase-сценарии работы с логами**:
  - `AddLogUseCase.cs` – **добавление лога в БД**.
  - `GetLogsUseCase.cs` – **получение логов (с фильтрацией по времени)**.
  - `GetLogsStatisticsUseCase.cs` – **подсчёт статистики логов**.
- `DTO/` – Data Transfer Objects **(DTO)** для передачи данных.
- `Mappings/` – классы для преобразования в DTO.
- `Events/` - не успел до конца реализовать. На логику программы не влияет сейчас.
---

### **1.3. Infrastructure (Слой хранения данных)**

– реализация **хранения данных

**Основной файл:**

- `SqliteLogsRepository.cs` – реализация с помощью SQLite.

### **1.4. Web (Слой взаимодействия с пользователем)**

– реализация **REST API сервера**.

**Основные файлы:**

- `RestServer.cs` – **HTTP сервер на **``.
- `LogsController.cs` – **контроллер API** для работы с логами.

## **2. Эндпоинты REST API**

Сервер работает по адресу ``.

### **2.1. Добавление лога**

```sh
curl -X POST http://localhost:5000/logs \
     -H "Content-Type: application/json" \
     -d '{"DateAndTime": "2024-03-01T12:30:00", "Level": "INFO", "Message": "Сервер запущен успешно"}'
```

### **2.2. Получение всех логов**

```sh
curl -X GET http://localhost:5000/logs
```

### **2.3. Получение логов за период (пример)**

```sh
curl -X GET "http://localhost:5000/logs?from=2024-03-01T00:00:00&to=2024-03-01T23:59:59"
```

### **2.4. Получение статистики**

```sh
curl -X GET http://localhost:5000/logs/statistics
```
---

## **3. Как запустить сервер?**

### **Шаг 1. Собрать проект**

```sh
LogAnalyzer/cmd/CmdWebApi/Program.cs dotnet build
```

### **Шаг 2. Запустить сервер**

```sh
LogAnalyzer/cmd/CmdConsoleApp/Program.cs dotnet run
```

### **Шаг 3. Отправлять запросы**

Используйте **curl** или Postman.

---

## **4. Технологии**

- **C# (.NET 8.0)**
- **SQLite (Microsoft.Data.Sqlite)**
- **HttpListener**
- **Чистая архитектура** (Clean Architecture)
- **Dependency Injection** (Microsoft.Extensions.DependencyInjection)
- **Spectre.Console** – для улучшенного интерфейса консольного приложения.
- **ScottPlot** – для построения графиков по логам.

---

## **5. Контакты**

**Коновалов Иван Андреевич**

**Email:** [festteil1406@gmail.com]
**GitHub:** [https://github.com/festy23]


---
@ MIT license


