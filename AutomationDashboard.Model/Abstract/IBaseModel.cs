using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Model.Abstract
{
    public interface IBaseModel
    {
        DateTime? CreateDate { get; set; }

        DateTime? UpdateDate { get; set; }

    }
}
