using System;

namespace GraphLabs.Backend.Domain
{
    /// <summary> Модуль-задание </summary>
    public class TaskModule
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Version { get; set; }
    }
}