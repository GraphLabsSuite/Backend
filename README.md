# Backend сайта GraphLabs

## Реализованные и запланированные фичи
- [x] Список модулей
- [x] Список вариантов
- [x] Выдача случайного варианта
- [x] Выдача файлов модуля
- [x] База общих изображений
- [ ] Выдача псевдо-случайного варианта
- [x] Пользователи
- [x] Аутентификация
- [ ] Авторизация по ролям
- [ ] Научить логи заданий получать пользователя из данных аутентификации
- [ ] Logout
- [x] Лог выполнения

## Технологии
* Asp.Net Core
* OData Core
* EntityFramework Core

## Подготовка к первому запуску
* Установите Visual Studio или Rider последней версии
* Установите [Net Core SDK 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
* Установите сертификат для отладки на localhost'е через https, выполнив:
`dotnet dev-certs https --trust`

## Аутентификация

> **POST** http://localhost:5000/auth/login

_Content-Type:_ application/json

_Body:_
```json 
{
    "email": "admin@graphlabs.ru",
    "password": "admin"
}
```

Ответ будет примерно такой:
```json
{
    "userId": 0,
    "firstName": "Администратор",
    "lastName": "Администратор",
    "token": "бла-бла-бла"
}
```

Во все последующие запросы необходимо включать заголовок

_Authorization_ : "Bearer бла-бла-бла", где бла-бла-бла - тот самый token, полученный в ответе 


## Примеры использования
**Метаданные:**
> **GET** http://localhost:5000/odata/$metadata

**Список всех модулей-заданий:**
> **GET** http://localhost:5000/odata/taskModules

**Модуль с идентификатором 1:**
> **GET** http://localhost:5000/odata/taskModules(1)

**Запрос с фильтрацией (название = "КСС"):**
> **GET** http://localhost:5000/odata/taskModules?$filter=Name eq 'КСС'

**Запрос с фильтрацией, лимитом и упорядочиванием:**
> **GET** http://localhost:5000/odata/taskModules?$filter=version eq '2.0'&$top=3&$orderby=Name desc

**Запрос случайного варианта задания с id = 2:**
> **GET** http://localhost:5000/odata/taskModules(2)/randomVariant

**Скачивание файла "service-worker.js" модуля-задания с id = 1:**
> **GET** http://localhost:5000/odata/taskModules(1)/download(path='service-worker.js')

**Скачивание файла "static/main.a091e228.js" модуля-задания с id = 1:**
> **GET** http://localhost:5000/odata/taskModules(1)/download(path='static%2Fjs%2Fmain.a091e228.js')

**Скачивание изображения "Complete.png" из общей библиотеки:**
> **GET** http://localhost:5000/odata/downloadImage(name='Complete.png')

**Список всех вариантов:**
> **GET** http://localhost:5000/odata/taskVariants

**Вариант с идентификатором 5:**
> **GET** http://localhost:5000/odata/taskVariants(5)

**Список всех студентов:**
> **GET** http://localhost:5000/odata/students

**Студент с идентификатором 2:**
> **GET** http://localhost:5000/odata/students(2)

**Студент с почтой "student-3@graphlabs.ru":**
> **GET** http://localhost:5000/odata/students?$filter=email eq 'student-3@graphlabs.ru'

**Список всех действий:**
> **GET** http://localhost:5000/odata/taskVariantLogs

**Зарегистрировать новое действие:**
> **POST** http://localhost:5000/odata/taskVariantLogs

_Content-Type:_ application/json

_Body:_
```json 
{
    "Action": "Тестовое действие",
    "VariantId": 1,
    "StudentId": 2
}
```