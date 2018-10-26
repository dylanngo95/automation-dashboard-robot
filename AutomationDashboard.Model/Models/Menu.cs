using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationDashboard.Model.Models
{
    public class Menu
    {
        public Project Project { get; set; }

        public List<SubProject> SubProjects { get; set; }

        public Menu()
        {
            this.SubProjects = new List<SubProject>();
        }

        public void SetProject(Project project)
        {
            project.Password = "";
            project.UserName = "";
            this.Project = project;
        }
    }
}