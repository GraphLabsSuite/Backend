using System.Collections.Generic;

namespace GraphLabs.Backend.Domain
{
    public class Student : User
    {
        public virtual string Group { get; set; }
        
        public virtual ICollection<TaskVariantLog> Logs { get; set; }
    }
}