using System.Collections.Generic;
using GraphLabs.Backend.Domain;

namespace GraphLabs.Backend.DAL
{
    public class InMemoryInitialData
    {
        private IList<TaskModule> _taskModules { get; set; }

        public IList<TaskModule> GetTaskModules()
        {
            if (_taskModules != null)
            {
                return _taskModules;
            }

            _taskModules = new List<TaskModule>();

            // taskModule #1
            var taskModule = new TaskModule
            {
                Id = 1,
                Name = "Изоморфизм",
                Description = "Даны два графа. Доказать их изоморфность путём наложения вершин одного графа на вершины другого, или обосновать, почему это невозможно.",
                Version = "2.0"
            };
            _taskModules.Add(taskModule);

            // taskModule #2
            taskModule = new TaskModule
            {
                Id = 2,
                Name = "КСС",
                Description = "Дан граф. Найти все компоненты сильной связности.",
                Version = "2.0"
            };
            _taskModules.Add(taskModule);

            return _taskModules;
        }
    }
}