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
    public interface IProjectService {
        Project Add(Project project);

        void Update(Project project);

        Project Delete(int id);

        IEnumerable<Project> GetAll();

        Project GetById(int id);

        void Save();
    }

    public class ProjectService : IProjectService
    {
        private IProjectRepository projectRepository;

        private IUnitOfWork unitOfWork;

        public ProjectService(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            this.projectRepository = projectRepository;
            this.unitOfWork = unitOfWork;
        }

        public Project Add(Project project)
        {
            var result = this.projectRepository.Add(project);
            unitOfWork.Commit();
            return result;
        }

        public Project Delete(int id)
        {
            return this.projectRepository.Delete(id);
        }

        public IEnumerable<Project> GetAll()
        {
            return this.projectRepository.GetAll();
        }

        public Project GetById(int id)
        {
            return this.projectRepository.GetSingleById(id);
        }

        public void Save()
        {
            this.unitOfWork.Commit();
        }

        public void Update(Project project)
        {
            this.projectRepository.Update(project);
        }
    }
}
