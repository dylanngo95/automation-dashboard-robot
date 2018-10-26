using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        AutomationDashboardDbContext Init();
    }
}
