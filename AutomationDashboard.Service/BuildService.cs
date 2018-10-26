using AutomationDashboard.Common;
using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Data.Repositories;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutomationDashboard.Service
{
    public interface IBuildService {
        Build Add(Build build);

        void Update(Build build);

        Build Delete(int id);

        IEnumerable<Build> GetAll();

        Build GetById(int id);

        BuildList GetAllBuild(int subProjectId, int size);

        void Save();
    }

    public class BuildService : IBuildService
    {

        private IBuildRepository buildRepository;
        private ISubProjectRepository subProjectRepository;

        private IUnitOfWork unitOfWork;

        public BuildService(IBuildRepository buildRepository, ISubProjectRepository subProjectRepository, IUnitOfWork unitOfWork)
        {
            this.buildRepository = buildRepository;
            this.subProjectRepository = subProjectRepository;
            this.unitOfWork = unitOfWork;
        }

        public Build Add(Build build)
        {
            var result = this.buildRepository.Add(build);
            unitOfWork.Commit();
            return result;
        }

        public Build Delete(int id)
        {
            return this.buildRepository.Delete(id);
        }

        public IEnumerable<Build> GetAll()
        {
            return this.buildRepository.GetAll();
        }

        public BuildList GetAllBuild(int subProjectId, int size)
        {
            var subProject = this.subProjectRepository.GetSubProjectById(subProjectId);

            var buildModelView = new BuildList()
            {
                Builds = this.buildRepository.GetAllBuild(subProjectId, size),
                Number = "30",
                SubProjectId = subProjectId,
                SubProjectName = subProject.DisplayName,
                SelectListItems = ConstVariable.GetSizeBuilds()
            };

            return buildModelView;
        }

        public Build GetById(int id)
        {
            return this.buildRepository.GetSingleById(id);
        }

        public void Save()
        {
            this.unitOfWork.Commit();
        }

        public void Update(Build build)
        {
            this.buildRepository.Update(build);
        }
    }
}
