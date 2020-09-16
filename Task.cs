using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackingSystem
{
    class Task
    {
        /*
         * текущий проект
         * тема
         * тип
         * приоритет
         * исполнитель
         * описание
         */

        private Project currentProject;
        private User contractor;

        private string topic;
        private string type;
        private string priority;
        private string description;

        public Task(Project project, User user, string[] args = null)
        {
            currentProject = project;
            contractor = user;
            if (args != null)
            {
                try
                {
                    topic = args[0];
                    type = args[1];
                    priority = args[2];
                    description = args[3];
                }
                catch { }
                
            }
        }

    }
}
