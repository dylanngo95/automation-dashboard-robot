using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Model.Models
{
    [Table("SubProjects")]
    public class SubProject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string DisplayName { get; set; }

        public bool Activate { get; set; }

        [ForeignKey("SubProjectId")]
        public ICollection<Build> Builds { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
