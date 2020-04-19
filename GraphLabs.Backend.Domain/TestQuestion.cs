using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class TestQuestion
    {
        public virtual long Id { get; set; }

        public virtual string PlainText { get; set; }

        public virtual Difficulty Difficulty { get; set; }

        public virtual int MaxScore { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<TestAnswer> Answers { get; set; }
    }
}
