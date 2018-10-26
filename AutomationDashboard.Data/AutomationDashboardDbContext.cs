using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data
{
    public class AutomationDashboardDbContext : DbContext
    {
        public AutomationDashboardDbContext() : base("name=AutomationDashboard")
        {
            Database.SetInitializer(new AutomationDashboardInitializer());
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<SubProject> SubProjects { get; set; }
        public DbSet<Build> Builds { get; set; }
        public DbSet<BuildDetail> BuildDetails { get; set; }
        public DbSet<CronJobSetting> CronJobSettings { get; set; }
        public DbSet<RequestBuildDetailLog> RequestBuildDetailLogs { get; set; }

    }

    public static class EntityExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
