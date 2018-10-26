using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Model.Models
{
    [Table("RequestBuildDetailLogs")]
    public class RequestBuildDetailLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BuidId { get; set; }

        public DateTime StartDate { get; set; }

        public String Note { get; set; }
    }
}
