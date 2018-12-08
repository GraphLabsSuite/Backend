# Backend сайта GraphLabs

## О текущей версии
Первая наипростейшая версия, собранная по сэмплам одаты.

Использует InMemory-базу данных, так что изменения между запусками сохраняться пока не будут.

Содержит единственную сущность - заголовок модуля-задания `TaskModule`.

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
> **GET** http://localhost:5000/odata/taskModules(___1___)

**Запрос с фильтрацией (название = "КСС"):**
> **GET** http://localhost:5000/odata/taskModules?$filter=Name eq 'КСС'

**Запрос с фильтрацией, лимитом и упорядочиванием:**
> **GET** http://localhost:5000/odata/taskModules?$filter=version eq '2.0'&$top=3&$orderby=Name desc

**Запрос случайного варианта задания с id = 2:**
> **GET** http://localhost:5000/odata/taskModules(___2___)/randomVariant

**Скачивание файла "service-worker.js" модуля-задания с id = 1:**
> **GET** http://localhost:5000/odata/taskModules(___1___)/download(path='service-worker.js')

**Скачивание файла "static/main.a091e228.js" модуля-задания с id = 1:**
> **GET** http://localhost:5000/odata/taskModules(1)/download(path='static%2Fjs%2Fmain.a091e228.js')

**Скачивание изображения "Complete.png" из общей библиотеки:**
> **GET** http://localhost:5000/odata/downloadImage(name='Complete.png')

**Список всех вариантов:**
> **GET** http://localhost:5000/odata/taskVariants

**Вариант с идентификатором 5:**
> **GET** http://localhost:5000/odata/taskVariants(___5___)