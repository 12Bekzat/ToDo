using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; }
        public bool IsDo { get; set; } = false;
    }
}
