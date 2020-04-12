using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs.Backend.Domain
{
    public class TestAnswer
    {
        public virtual long Id { get; set; }

        public virtual string Answer { get; set; }

        public virtual bool IsRight { get; set; }

        public virtual TestQuestion Question { get; set; }
    }
}
