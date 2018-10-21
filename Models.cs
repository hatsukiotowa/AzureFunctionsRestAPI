using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessFuncs
{
    public class Todo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("n");
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public string MissionDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class TodoCreateModel
    {
        public string MissionDescription { get; set; }
    }

    public class TodoUpdateModel
    {
        public string MissionDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
    
}
