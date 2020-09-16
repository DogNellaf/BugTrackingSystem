using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackingSystem
{
    class Project
    {
        private List<Task> tasksInProject;

        public Project()
        {
            tasksInProject = new List<Task>();
        }

        public void AddTask(Task temp)
        {
            tasksInProject.Add(temp);
        }
    }
}
