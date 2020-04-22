using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class TestStudentAnswer
    {
        public virtual long Id { get; set; }

        public virtual long AnswerId { get; set; }

        public virtual string Answer { get; set; }

        public virtual bool IsRight { get; set; }

        public virtual TestResult TestResult { get; set; }

        public virtual TestQuestionVersion TestQuestionVersion { get; set; }
    }
}
