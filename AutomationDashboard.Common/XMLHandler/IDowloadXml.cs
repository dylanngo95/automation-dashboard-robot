using AutomationDashboard.Data;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Common.XMLHandler
{
    public interface IDowloadXml
    {

        /// <summary>
        /// Api load data in api TeamCity && Jenskin
        /// </summary>
        /// <param name="subproject">Subproject</param>
        /// <param name="setting">Settings</param>
        /// <param name="project">Project</param>
        /// <param name="db">DbContext</param>
        void SaveBuils(SubProject subproject, Setting setting, Project project, AutomationDashboardDbContext db);

        void SaveBuildAndCheckTime(SubProject subproject, Setting setting, Project project, AutomationDashboardDbContext db);

        /// <summary>
        /// Read file xml with name: ChromeOutput.xml
        /// </summary>
        /// <param name="build">Build</param>
        /// <param name="setting">Setting</param>
        /// <param name="project">Project</param>
        /// <param name="subProject">SubProject</param>
        /// <param name="db">DbContext</param>
        void SaveBuildDetail(Build build, Setting setting, Project project, SubProject subProject, AutomationDashboardDbContext db);

    }
}
