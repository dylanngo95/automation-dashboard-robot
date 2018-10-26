using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private AutomationDashboardDbContext dbContext;

        public UnitOfWork(IDbFactory dbFactory) {
            this.dbFactory = dbFactory;
        }

        public AutomationDashboardDbContext DbContext {
            get {
                return dbContext ?? (dbContext = dbFactory.Init());
            }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
