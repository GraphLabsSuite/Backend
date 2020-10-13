using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class TestQuestion
    {
        public virtual long Id { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<TestQuestionVersion> TestQuestionVersions { get; set; }
    }
}
