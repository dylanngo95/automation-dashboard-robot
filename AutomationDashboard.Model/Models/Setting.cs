using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Model.Models
{
    [Table("Settings")]
    public class Setting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Domain { get; set; }

        [MaxLength(100)]
        public string UrlBuildStart { get; set; }

        [MaxLength(100)]
        public string UrlBuildEnd { get; set; }

        [MaxLength(100)]
        public string UrlReportStart { get; set; }

        [MaxLength(100)]
        public string UrlReportEnd { get; set; }

        [ForeignKey("SettingId")]
        public ICollection<Project> Projects { get; set; }
    }
}
