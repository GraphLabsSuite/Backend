using System.Collections.Generic;
using System.Linq.Expressions;
using GraphLabs.Backend.Domain;

namespace GraphLabs.Backend.DAL
{
    public class InMemoryInitialData
    {
        public IList<TaskModule> GetTaskModules()
        {
            var taskModules = new List<TaskModule>();

            // taskModule #1
            var taskModule = new TaskModule
            {
                Id = 1,
                Name = "Изоморфизм",
                Description = "Даны два графа. Доказать их изоморфность путём наложения вершин одного графа на вершины другого, или обосновать, почему это невозможно.",
                Version = "2.0"
            };
            taskModules.Add(taskModule);

            // taskModule #2
            taskModule = new TaskModule
            {
                Id = 2,
                Name = "КСС",
                Description = "Дан граф. Найти все компоненты сильной связности.",
                Version = "2.0"
            };
            taskModules.Add(taskModule);

            return taskModules;
        }

        public IList<TaskVariant> GetTaskVariants(IEnumerable<TaskModule> modules)
        {
            var taskVariants = new List<TaskVariant>();
            var idCounter = 0;
            
            foreach (var taskModule in modules)
            {
                for (var i = 0; i < 3; i++)
                {
                    idCounter++;
                    taskVariants.Add(new TaskVariant
                    {
                        Id = idCounter,
                        Name = $"Вариант {idCounter}",
                        TaskModule = taskModule,
                        VariantData = 
@"[{ 
    ""vertices"": [ ""1"", ""2"", ""3"", ""4"", ""5"" ], 
    ""edges"": [ { ""source"": ""1"", ""target"": ""2"" }, { ""source"": ""2"", ""target"": ""3"" }, { ""source"": ""3"", ""target"": ""4"" }, { ""source"": ""4"", ""target"": ""5"" }, { ""source"": ""5"", ""target"": ""1"" } ] 
}]"

                    });
                }
            }

            return taskVariants;
        }
    }
}