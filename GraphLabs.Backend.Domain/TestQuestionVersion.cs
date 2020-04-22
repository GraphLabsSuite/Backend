using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class TestQuestionVersion
    {
        public virtual long Id { get; set; }

        public virtual string PlainText { get; set; }

        public virtual Difficulty Difficulty { get; set; }

        public virtual int MaxScore { get; set; }

        public virtual TestQuestion TestQuestion { get; set; }

        public virtual ICollection<TestStudentAnswer> TestStudentAnswers { get; set; }
    
        public virtual ICollection<TestAnswer> TestAnswers { get; set; }
    }
}
