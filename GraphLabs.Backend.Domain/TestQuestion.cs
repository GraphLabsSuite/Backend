using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class TestQuestion
    {
        public virtual long Id { get; set; }

        public virtual string Description { get; set; }

        public enum Difficulty : int { Three, Four, Five }

        public virtual int MaxScore { get; set; }

        public virtual string ImageURL { get; set; }

        public virtual TestResult TestResult { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<TestAnswer> Answers { get; set; }
    }
}
