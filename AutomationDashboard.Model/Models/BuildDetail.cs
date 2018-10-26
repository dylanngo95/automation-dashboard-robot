using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Model.Models
{
    [Table("BuildDetails")]
    public class BuildDetail
    {
        public BuildDetail()
        {
            this.Pass = 0;
            this.Fail = 0;
            this.NotRun = 0;
            this.Id = 0;
        }

        [Key]
        [ForeignKey("Build")]
        public int Id { get; set; }

        public int Pass { get; set; }

        public int Fail { get; set; }

        public int NotRun { get; set; }

        public DateTime DateTime { get; set; }

        public virtual Build Build { get; set; }
    }
}
