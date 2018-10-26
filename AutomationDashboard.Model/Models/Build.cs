using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Model.Models
{

    [Table("Builds")]
    public class Build
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BuildId { get; set; }

        public int Number { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [MaxLength(50)]
        public string Pinned { get; set; }

        [MaxLength(500)]
        public string WebUrl { get; set; }

        public int Count { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        [MaxLength(50)]
        public string Duration { get; set; }

        public virtual BuildDetail BuildDetails { get; set; }

        public int SubProjectId { get; set; }
        public SubProject SubProject { get; set; }
    }
}
