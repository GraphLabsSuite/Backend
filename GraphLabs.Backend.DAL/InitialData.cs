using System.Collections.Generic;
using System.Linq.Expressions;
using GraphLabs.Backend.Domain;

namespace GraphLabs.Backend.DAL
{
    public class InitialData
    {
        public IEnumerable<TaskModule> GetTaskModules()
        {
            // taskModule #1
            yield return new TaskModule
            {
                Id = 1,
                Name = "Изоморфизм",
                Description = "Даны два графа. Доказать их изоморфность путём наложения вершин одного графа на вершины другого, или обосновать, почему это невозможно.",
                Version = "2.0"
            };

            // taskModule #2
            yield return  new TaskModule
            {
                Id = 2,
                Name = "КСС",
                Description = "Дан граф. Найти все компоненты сильной связности.",
                Version = "2.0"
            };
        }

        public IEnumerable<TaskVariant> GetTaskVariants(IEnumerable<TaskModule> modules)
        {
            var idCounter = 0;
            
            foreach (var taskModule in modules)
            {
                for (var i = 0; i < 3; i++)
                {
                    idCounter++;
                    yield return new TaskVariant
                    {
                        Id = idCounter,
                        Name = $"Вариант {idCounter}",
                        TaskModule = taskModule,
                        VariantData = 
@"[{ 
    ""vertices"": [ ""1"", ""2"", ""3"", ""4"", ""5"" ], 
    ""edges"": [ { ""source"": ""1"", ""target"": ""2"" }, { ""source"": ""2"", ""target"": ""3"" }, { ""source"": ""3"", ""target"": ""4"" }, { ""source"": ""4"", ""target"": ""5"" }, { ""source"": ""5"", ""target"": ""1"" } ] 
}]"

                    };
                }
            }
        }

        public IEnumerable<User> GetUsers()
        {
            var idCounter = 0;
            yield return new Teacher
            {
                Id = ++idCounter,
                Email = "admin@graphlabs.ru",
                Name = "Администратор"
            };
            
            yield return new Student
            {
                Id = ++idCounter,
                Email = "student-1@graphlabs.ru",
                Name = "Студент Первый Тестовый",
                Group = "Первая Тестовая"
            };
            
            yield return new Student
            {
                Id = ++idCounter,
                Email = "student-2@graphlabs.ru",
                Name = "Студент Второй Тестовый",
                Group = "Первая Тестовая"
            };
            
            yield return new Student
            {
                Id = ++idCounter,
                Email = "student-3@graphlabs.ru",
                Name = "Студент Третий Тестовый",
                Group = "Вторая Тестовая"
            };
            
            yield return new Student
            {
                Id = ++idCounter,
                Email = "student-4@graphlabs.ru",
                Name = "Студент Четвёртый Тестовый",
                Group = "Вторая Тестовая"
            };
        }
    }
}