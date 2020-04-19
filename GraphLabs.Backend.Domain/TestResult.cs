using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class TestResult
    {
        public virtual long Id { get; set; }

        public virtual DateTime DateTime { get; set; }

        public virtual int Mark { get; set; }

        public virtual MarkEU MarkEU { get; set; }

        public virtual MarkRU MarkRU { get; set; }

        public virtual Student Student { get; set; }

        public virtual ICollection<TestResultQuestion> TestResultQuestions { get; set; }
    }
}
