using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Data.Repositories;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Service
{
    public interface IRequestBuildDetailLogService {
        RequestBuildDetailLog Add(RequestBuildDetailLog requestBuildDetailLog);

        void Update(RequestBuildDetailLog requestBuildDetailLog);

        RequestBuildDetailLog Delete(int id);

        IEnumerable<RequestBuildDetailLog> GetAll();

        RequestBuildDetailLog GetById(int id);

        void Save();
    }

    public class RequestBuildDetailLogService : IRequestBuildDetailLogService
    {

        private IRequestBuildDetailLogRepository requestBuildDetailLogRepository;

        private IUnitOfWork unitOfWork;

        public RequestBuildDetailLogService(IRequestBuildDetailLogRepository requestBuildDetailLogRepository, IUnitOfWork unitOfWork)
        {
            this.requestBuildDetailLogRepository = requestBuildDetailLogRepository;
            this.unitOfWork = unitOfWork;
        }

        public RequestBuildDetailLog Add(RequestBuildDetailLog requestBuildDetailLog)
        {
            var result = this.requestBuildDetailLogRepository.Add(requestBuildDetailLog);
            unitOfWork.Commit();
            return result;
        }

        public RequestBuildDetailLog Delete(int id)
        {
            return this.requestBuildDetailLogRepository.Delete(id);
        }

        public IEnumerable<RequestBuildDetailLog> GetAll()
        {
            return this.requestBuildDetailLogRepository.GetAll();
        }

        public RequestBuildDetailLog GetById(int id)
        {
            return this.requestBuildDetailLogRepository.GetSingleById(id);
        }

        public void Save()
        {
            this.unitOfWork.Commit();
        }

        public void Update(RequestBuildDetailLog requestBuildDetailLog)
        {
            this.requestBuildDetailLogRepository.Update(requestBuildDetailLog);
        }
    }
}
