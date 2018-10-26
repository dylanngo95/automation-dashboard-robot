using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutomationDashboard.Common
{
    public class ConstVariable
    {
        public static List<SelectListItem> GetSizeBuilds()
        {
            return new List<SelectListItem>() {
               new SelectListItem {
                    Text = "30",
                    Value = "30"
                },
                new SelectListItem {
                    Text = "50",
                    Value = "50"
                },
                new SelectListItem {
                    Text = "100",
                    Value = "100"
                },
                new SelectListItem {
                    Text = "500",
                    Value = "500"
                },
               new SelectListItem {
                    Text = "1000",
                    Value = "1000"
                },
            };
        }
    }
}
