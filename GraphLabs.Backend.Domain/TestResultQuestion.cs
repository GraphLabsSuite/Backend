using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class TestResultQuestion
    {
        public virtual long Id { get; set; }

        public virtual string Question { get; set; }

        public virtual string Subject { get; set; }

        public virtual ICollection<TestStudentAnswer> TestStudentAnswers { get; set; }
    
        public virtual ICollection<TestResultAnswer> TestResultAnswers { get; set; }
    }
}
