using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Model.Models
{
    [Table("CronJobSettings")]
    public class CronJobSetting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CronTime { get; set; }

        public bool IsDownloadEveryDay { get; set; }

        public bool IsActivate { get; set; }

        [MaxLength(250)]
        public String UrlCronJob { get; set; }
    }
}
