using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomationDashboard.Model.Models
{
    public class BuildList
    {
        [Display(Name = "SubProject Id")]
        public int SubProjectId { get; set; }

        [Display(Name = "SubProject Name")]
        public string SubProjectName { get; set; }

        [Display(Name = "Number")]
        public string Number { get; set; }

        public List<Build> Builds { get; set; }
        public List<SelectListItem> SelectListItems { get; set; }
    }
}