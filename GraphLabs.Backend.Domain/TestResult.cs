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

        public enum MarkEU : int { F, E, D, C, B, A }

        public enum MarkRU : int { Two = 2, Three = 3, Four = 4, Five = 5 }

        public virtual Student Student { get; set; }

        public virtual ICollection<TestQuestion> Questions { get; set; }
    }
}
