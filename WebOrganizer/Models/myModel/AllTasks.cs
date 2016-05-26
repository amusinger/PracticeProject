using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOrganizer.Models.myModel
{
    public class AllTasks
    {
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<FinishedTask> FinishedTasks { get; set; }
    }
}