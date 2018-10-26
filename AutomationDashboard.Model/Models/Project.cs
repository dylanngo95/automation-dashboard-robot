using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Model.Models
{
    [Table("Projects")]
    public class Project
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string DisplayName { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        public bool Activate { get; set; }

        [ForeignKey("ProjectId")]
        public ICollection<SubProject> SubProjects { get; set; }

        public int SettingId { get; set; }
        public Setting Setting { get; set; }
    }
}
