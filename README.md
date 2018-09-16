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
* Установите [Net Core SDK 2.1](https://www.microsoft.com/net/download/dotnet-core/2.1)
* Установите сертификат для отладки на localhost'е через https, выполнив:
`dotnet dev-certs https --trust`

## Примеры использования
**Метаданные**
> https://localhost:5001/odata/$metadata

```xml
<edmx:Edmx Version="4.0">
    <edmx:DataServices>
        <Schema Namespace="GraphLabs.Backend.Domain">
            <EntityType Name="TaskModule">
                <Key>
                    <PropertyRef Name="Id"/>
                </Key>
                <Property Name="Id" Type="Edm.Int64" Nullable="false"/>
                <Property Name="Name" Type="Edm.String"/>
                <Property Name="Description" Type="Edm.String"/>
                <Property Name="Version" Type="Edm.String"/>
            </EntityType>
        </Schema>
        <Schema Namespace="Default">
            <EntityContainer Name="Container">
                <EntitySet Name="TaskModules" EntityType="GraphLabs.Backend.Domain.TaskModule"/>
            </EntityContainer>
        </Schema>x
    </edmx:DataServices>
</edmx:Edmx>
```

**Список всех модулей-заданий**
> **GET** https://localhost:5001/odata/TaskModules

```json
{
  "@odata.context": "https://localhost:5001/odata/$metadata#TaskModules",
  "value": [
    {
      "Id": 1,
      "Name": "Изоморфизм",
      "Description": "Даны два графа. Доказать их изоморфность путём наложения вершин одного графа на вершины другого, или обосновать, почему это невозможно.",
      "Version": "2.0"
    },
    {
      "Id": 2,
      "Name": "КСС",
      "Description": "Дан граф. Найти все компоненты сильной связанности.",
      "Version": "2.0"
    }
  ]
}
```

**Добавление нового модуля-задания**
> **POST** https://localhost:5001/odata/TaskModules

_Content-Type: application/json_

_Content:_
```json
{
  "Id": 3,
  "Name": "Подграфы",
  "Description": "Длинное-предлинное описание создаваемого модуля-задания.",
  "Version": "2.1"
}
```

**Запрос с фильтрацией**

Запросим заголовок модуля с Id = 2:
> **GET** https://localhost:5001/odata/TaskModules?$filter=Id eq 2
```json
{
  "@odata.context": "https://localhost:5001/odata/$metadata#TaskModules",
  "value": [
    {
      "Id": 2,
      "Name": "КСС",
      "Description": "Дан граф. Найти все компоненты сильной связанности.",
      "Version": "2.0"
    }
  ]
}
```

**Запрос с фильтрацией, лимитом и упорядочиванием**

Обратите внимание, у нас нужно писать $ перед каждой частью запроса (бывает упрощённый синтаксис, но у нас он пока не поддержан).
> **GET** https://localhost:5001/odata/TaskModules?$filter=version eq '2.0'&$top=3&$orderby=Name desc
```json
{
  "@odata.context": "https://localhost:5001/odata/$metadata#TaskModules",
  "value": [
    {
      "Id": 2,
      "Name": "КСС",
      "Description": "Дан граф. Найти все компоненты сильной связанности.",
      "Version": "2.0"
    },
    {
      "Id": 1,
      "Name": "Изоморфизм",
      "Description": "Даны два графа. Доказать их изоморфность путём наложения вершин одного графа на вершины другого, или обосновать, почему это невозможно.",
      "Version": "2.0"
    }
  ]
}
```
