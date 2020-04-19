using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class Subject
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
        
        public virtual string Description { get; set; }

        public virtual ICollection<TaskModule> TaskModules { get; set; }

        public virtual ICollection<TestQuestion> TestQuestions { get; set; }

        public virtual ICollection<TestResultQuestion> TestResultQuestions { get; set; }
    }
}
