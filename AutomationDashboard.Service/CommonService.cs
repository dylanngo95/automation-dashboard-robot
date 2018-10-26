using AutomationDashboard.Common.XMLHandler;
using AutomationDashboard.Data;
using AutomationDashboard.Data.Repositories;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Service
{
    public interface ICommonService
    {

        List<Menu> getMenus();

        GridLineChart GetDataLast7Day(int subProjectId);

        bool DowloadBuildDetail();

        bool DeleteBuildDetailAndDowload();

    }

    public class CommonService : ICommonService
    {

        private IProjectRepository projectRepository;

        private ISubProjectRepository subProjectRepository;

        private IBuildRepository buildRepository;

        private IBuildDetailRepository buildDetailRepository;

        private ISettingRepository settingRepository;

        private IDowloadXml teamCity;

        private IDowloadXml jenkins;

        private AutomationDashboardDbContext db;

        public CommonService(IProjectRepository projectRepository, ISubProjectRepository subProjectRepository, 
            IBuildRepository buildRepository, IBuildDetailRepository buildDetailRepository, ISettingRepository settingRepository,
            IDowloadXml teamCity, IDowloadXml jenkins, AutomationDashboardDbContext db)
        {
            this.projectRepository = projectRepository;
            this.subProjectRepository = subProjectRepository;
            this.buildRepository = buildRepository;
            this.buildDetailRepository = buildDetailRepository;
            this.settingRepository = settingRepository;
            this.teamCity = teamCity;
            this.jenkins = jenkins;
            this.db = db;
        }

        public bool DowloadBuildDetail()
        {
            try
            {

                var subProjects = this.subProjectRepository.GetSubProjectAndCheckActive();
                foreach (var item in subProjects)
                {
                    var project = this.projectRepository.GetSingleById(item.ProjectId);
                    if (project != null)
                    {
                        var setting = this.settingRepository.GetSingleById(project.SettingId);
                        if (setting != null)
                        {
                            switch (setting.Name)
                            {
                                case "TeamCity_DN_Production":
                                    teamCity.SaveBuildAndCheckTime(item, setting, project, db);
                                    break;
                                case "TeamCity_Local":
                                    teamCity.SaveBuildAndCheckTime(item, setting, project, db);
                                    break;
                                case "Jenkins_FH":
                                    jenkins.SaveBuildAndCheckTime(item, setting, project, db);
                                    break;
                                default:
                                    Debug.WriteLine("Dowload builds error");
                                    break;

                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex);
                return false;
            }

            return true;
        }


        public bool DeleteBuildDetailAndDowload() {

            try
            {
                db.BuildDetails.Clear();
                db.Builds.Clear();

                var subProjects = this.subProjectRepository.GetSubProjectAndCheckActive();
                foreach (var item in subProjects)
                {
                    var project = db.Projects.SingleOrDefault(p => p.Id == item.ProjectId);
                    if (project != null)
                    {
                        var setting = db.Settings.SingleOrDefault(p => p.Id == project.SettingId);
                        if (setting != null)
                        {
                            switch (setting.Name)
                            {
                                case "TeamCity_DN_Production":
                                    teamCity.SaveBuils(item, setting, project, db);
                                    break;
                                case "TeamCity_Local":
                                    teamCity.SaveBuils(item, setting, project, db);
                                    break;
                                case "Jenkins_FH":
                                    jenkins.SaveBuils(item, setting, project, db);
                                    break;
                                default:
                                    Debug.WriteLine("Dowload builds error");
                                    break;

                            }
                        }
                    }

                }

                var builds = this.buildRepository.GetAll();
                foreach (var item in builds)
                {
                    var subProject = db.SubProjects.Single(p => p.Id == item.SubProjectId);
                    if (subProject != null)
                    {
                        var project = db.Projects.Single(p => p.Id == subProject.ProjectId);
                        if (project != null)
                        {
                            var setting = db.Settings.Single(p => p.Id == project.SettingId);
                            if (setting != null)
                            {
                                switch (setting.Name)
                                {
                                    case "TeamCity_DN_Production":
                                        teamCity.SaveBuildDetail(item, setting, project, null, db);
                                        break;
                                    case "TeamCity_Local":
                                        teamCity.SaveBuildDetail(item, setting, project, null, db);
                                        break;
                                    case "Jenkins_FH":
                                        jenkins.SaveBuildDetail(item, setting, project, subProject, db);
                                        break;
                                    default:
                                        Debug.WriteLine("Dowload build detail error");
                                        break;

                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                return false;
            }

        }

        public GridLineChart GetDataLast7Day(int subProjectId)
        {
            var last7Day = this.buildRepository.GetLast7Build(subProjectId);

            GridLineChart gridLineChart = new GridLineChart();

            for (int i = 0; i < last7Day.Count; i++)
            {
                var build = last7Day[i];

                var buildDetail = this.buildDetailRepository.GetSingleById(build.Id);
                if (buildDetail != null)
                {
                    gridLineChart.Fails[i] += buildDetail.Fail;
                    gridLineChart.Pass[i] += buildDetail.Pass;
                    gridLineChart.NotRun[i] += buildDetail.NotRun;
                    var date = string.Format("{0:yyyy-MM-dd}", buildDetail.DateTime);
                    gridLineChart.Labels.Add(date);
                }
                else
                {
                    Debug.WriteLine("Build detail is null, BuildId= " + build.BuildId);
                }


            }

            return gridLineChart;
        }

        public List<Menu> getMenus()
        {
            List<Menu> menus = new List<Menu>();

            var projects = projectRepository.GetProjectActives();
            foreach (Project item in projects) {
                Menu menu = new Menu();
                menu.Project = item;

                var subProjects = subProjectRepository.GetSubProjectActiveByProjectId(item.Id);
                menu.SubProjects = subProjects;

                menus.Add(menu);
            }

            return menus;
        }
    }
}
