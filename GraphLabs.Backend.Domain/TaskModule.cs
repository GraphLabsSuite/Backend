using System;
using System.Collections.Generic;

namespace GraphLabs.Backend.Domain
{
    /// <summary> Модуль-задание </summary>
    public class TaskModule
    {
        public virtual long Id { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual string Description { get; set; }
        
        public virtual string Version { get; set; }

        public virtual Subject Subject { get; set; }
        
        public virtual ICollection<TaskVariant> Variants { get; set; }
    }
}