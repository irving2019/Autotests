![.NET](https://img.shields.io/badge/.NET%2010.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

# Autotests — набор шаблонов автоматизированных тестов

## Структура репозитория

- `UnitTests/` — модульные тесты (проверка функций без внешних зависимостей)
- `IntegrationTests/` — интеграционные тесты (взаимодействие с БД и репозиториями)

### Запуск тестов

```bash
# Модульные тесты
dotnet test UnitTests

# Интеграционные тесты
dotnet test IntegrationTestsTemplate

# Все тесты решения
dotnet test Autotests.sln
```

👤 Автор

**[Voldemar Pilipenko](https://github.com/irving2019)** —  автоматизация тестирования.  

[![GitHub irving2019](https://img.shields.io/badge/GitHub-irving2019-24292f?logo=github)](https://github.com/irving2019)
