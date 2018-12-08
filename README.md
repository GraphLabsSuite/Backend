# Backend сайта GraphLabs

## Реализованные и запланированные фичи
- [x] Список модулей
- [x] Список вариантов
- [x] Выдача случайного варианта
- [x] Выдача файлов модуля
- [x] База общих изображений
- [ ] Выдача псевдо-случайного варианта
- [ ] Пользователи
- [ ] Лог выполнения

## Технологии
* Asp.Net Core
* OData Core
* EntityFramework Core

## Подготовка к первому запуску
* Установите Visual Studio или Rider последней версии
* Установите [Net Core SDK 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
* Установите сертификат для отладки на localhost'е через https, выполнив:
`dotnet dev-certs https --trust`

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
