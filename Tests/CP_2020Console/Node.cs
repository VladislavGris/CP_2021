using System;
using System.Collections.Generic;
using System.Text;

namespace DataTree
{
    class TaskTree
    {
        public class Node
        {
            private Tasks.ProductionTask Task { get; set; }
            private List<Tasks.ProductionTask> ChildTasks { get; set; }

            public Tasks.ProductionTask GetTask() => this.Task;
            public List<Tasks.ProductionTask> GetTaskChilds() => this.ChildTasks;


        }

    }
}
