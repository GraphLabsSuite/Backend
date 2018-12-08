using System.Collections.Generic;

namespace GraphLabs.Backend.Domain
{
    /// <summary> Вариант задания </summary>
    public class TaskVariant
    {
        public virtual long Id { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual string VariantData { get; set; }
        
        public virtual TaskModule TaskModule { get; set; }
        
        public virtual ICollection<TaskVariantLog> Logs { get; set; }
    }
}