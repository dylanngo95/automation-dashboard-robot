using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationDashboard.Model.Models
{
    public class GridLineChart
    {
        public List<int> Fails { get; set; }
        public List<int> Pass { get; set; }
        public List<int> NotRun { get; set; }
        public List<string> Labels { get; set; }

        public GridLineChart()
        {
            this.Fails = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
            this.Pass = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
            this.NotRun = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
            this.Labels = new List<string>();
        }
    }
}