using AutomationDashboard.Model.Models;
using AutomationDashboard.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Menu = AutomationDashboard.Model.Models.Menu;

namespace AutomationDashboard.Web.Mappings
{
    public class AutoMapperManual
    {
        public static List<MenuViewModel> ToMenuViewModel(List<Menu> menus)
        {
            List<MenuViewModel> menuViewModels = new List<MenuViewModel>();
            foreach (Menu item in menus)
            {
                MenuViewModel menuViewModel = new MenuViewModel();
                menuViewModel.Project = item.Project;
                menuViewModel.SubProjects.AddRange(item.SubProjects);

                menuViewModels.Add(menuViewModel);
            }
            return menuViewModels;
        }

        public static BuildListViewModel ToBuildListViewModel(BuildList buildLists) {
            return new BuildListViewModel()
            {
                Builds = buildLists.Builds,
                Number = buildLists.Number,
                SubProjectId = buildLists.SubProjectId,
                SubProjectName = buildLists.SubProjectName,
                SelectListItems = buildLists.SelectListItems
            };
        }


    }
}