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
    public interface ISubProjectService {
        SubProject Add(SubProject subProject);

        void Update(SubProject subProject);

        SubProject Delete(int id);

        IEnumerable<SubProject> GetAll();

        SubProject GetById(int id);

        List<SubProject> GetSubProjectAndCheckActive();

        void Save();

    }

    public class SubProjectService : ISubProjectService
    {
        private ISubProjectRepository subProjectRepository;

        private IUnitOfWork unitOfWork;

        public SubProjectService(ISubProjectRepository subProjectRepository, IUnitOfWork unitOfWork)
        {
            this.subProjectRepository = subProjectRepository;
            this.unitOfWork = unitOfWork;
        }

        public SubProject Add(SubProject subProject)
        {
            var result = this.subProjectRepository.Add(subProject);
            unitOfWork.Commit();
            return result;
        }

        public SubProject Delete(int id)
        {
            return this.subProjectRepository.Delete(id);
        }

        public IEnumerable<SubProject> GetAll()
        {
            return this.subProjectRepository.GetAll();
        }

        public SubProject GetById(int id)
        {
            return this.subProjectRepository.GetSingleById(id);
        }

        public List<SubProject> GetSubProjectAndCheckActive()
        {
            return this.subProjectRepository.GetSubProjectAndCheckActive();
        }

        public void Save()
        {
            this.unitOfWork.Commit();
        }

        public void Update(SubProject subProject)
        {
            this.subProjectRepository.Update(subProject);
        }
    }
}
