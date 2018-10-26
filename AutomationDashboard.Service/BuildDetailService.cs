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
    public interface IBuildDetailService {
        BuildDetail Add(BuildDetail buildDetail);

        void Update(BuildDetail buildDetail);

        BuildDetail Delete(int id);

        IEnumerable<BuildDetail> GetAll();

        BuildDetail GetById(int id);

        BuildDetail GetLastBuildDetail(int subProjectId);

        void Save();
    }

    public class BuildDetailService : IBuildDetailService
    {

        private IBuildDetailRepository buildDetailRepository;

        private IBuildRepository buildRepository;

        private IUnitOfWork unitOfWork;

        public BuildDetailService(IBuildDetailRepository buildDetailRepository, IBuildRepository buildRepository, IUnitOfWork unitOfWork)
        {
            this.buildDetailRepository = buildDetailRepository;
            this.buildRepository = buildRepository;
            this.unitOfWork = unitOfWork;
        }

        public BuildDetail Add(BuildDetail buildDetail)
        {
            var result = this.buildDetailRepository.Add(buildDetail);
            unitOfWork.Commit();
            return result;
        }

        public BuildDetail Delete(int id)
        {
            return this.buildDetailRepository.Delete(id);
        }

        public IEnumerable<BuildDetail> GetAll()
        {
            return this.buildDetailRepository.GetAll();
        }

        public BuildDetail GetById(int id)
        {
            return this.buildDetailRepository.GetSingleById(id);
        }

        public BuildDetail GetLastBuildDetail(int subProjectId)
        {
            var lastBuild = this.buildRepository.GetLastBuild(subProjectId);
            if (lastBuild != null)
                return this.buildDetailRepository.GetSingleById(lastBuild.Id);
            return null;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public void Update(BuildDetail buildDetail)
        {
            this.buildDetailRepository.Update(buildDetail);
        }
    }
}
