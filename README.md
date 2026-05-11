# Autotests — набор шаблонов автоматизированных тестов

## Структура репозитория

- `UnitTests/` — модульные тесты (проверка функций без внешних зависимостей)
- `IntegrationTests/` — интеграционные тесты (взаимодействие с БД и репозиториями)

## Запуск тестов

```bash
# Модульные тесты
dotnet test UnitTests

# Интеграционные тесты
dotnet test IntegrationTestsTemplate

# Все тесты решения
dotnet test Autotests.sln