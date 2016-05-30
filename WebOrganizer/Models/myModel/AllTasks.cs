using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOrganizer.Models.myModel
{
    public class AllTasks
    {
        public IList<Task> Tasks { get; set; }
        public IList<FinishedTask> FinishedTasks { get; set; }
        //public IEnumerable<Task> Tasks { get; set; }
        //public IEnumerable<FinishedTask> FinishedTasks { get; set; }
    }
}