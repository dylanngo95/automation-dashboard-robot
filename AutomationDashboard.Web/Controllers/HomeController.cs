using AutomationDashboard.Data;
using AutomationDashboard.Model.Models;
using AutomationDashboard.Service;
using AutomationDashboard.Web.Mappings;
using AutomationDashboard.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutomationDashboard.Web.Controllers
{
    public class HomeController : Controller
    {
        ICommonService commonService;

        IBuildService buildService;

        IBuildDetailService buildDetailService;

        ISubProjectService subProjectService;

        ICronJobSettingService cronJobSettingService;

        public HomeController(ICommonService commonService, IBuildService buildService, IBuildDetailService buildDetailService, 
            ISubProjectService subProjectService, ICronJobSettingService cronJobSettingService)
        {
            this.commonService = commonService;
            this.buildService = buildService;
            this.buildDetailService = buildDetailService;
            this.subProjectService = subProjectService;
            this.cronJobSettingService = cronJobSettingService;
        }

        public ActionResult Index(int projectId = -1)
        {

            this.cronJobSettingService.Add(new CronJobSetting() {
                Id = 1,
                IsActivate = true,
                UrlCronJob = "http://jundat95.com",
                IsDownloadEveryDay = true,
                CronTime = DateTime.Now
            });

            var menus = commonService.getMenus();

            List<MenuViewModel> menuViewModels = AutoMapperManual.ToMenuViewModel(menus);

            if (menus != null)
            {
                if (projectId != -1)
                {
                    return View(menuViewModels.Where(p => p.Project.Id == projectId).ToList());
                }
                return View(menuViewModels);
            }
            return View(new List<MenuViewModel>());
        }

        public ActionResult SidebarMenu()
        {
            var menus = commonService.getMenus().ToList();

            List<MenuViewModel> menuViewModels = AutoMapperManual.ToMenuViewModel(menus);

            return PartialView("_SidebarMenu", menuViewModels);
        }

        public ActionResult GetBuild(int subProjectId)
        {
            var builds = this.buildService.GetAllBuild(subProjectId, 30);
            if (builds != null)
            {
                var buildViewModel = AutoMapperManual.ToBuildListViewModel(builds);
                return View(buildViewModel);
            }
            else {
                return View(new BuildListViewModel());
            }
        }

        [HttpPost]
        public ActionResult GetBuild(BuildListViewModel buildListViewModel)
        {
            var size = (int.TryParse(buildListViewModel.Number, out int n) ? Convert.ToInt32(buildListViewModel.Number) : 30);
            var builds = this.buildService.GetAllBuild(buildListViewModel.SubProjectId, size);

            if (builds != null)
            {
                var buildViewModel = AutoMapperManual.ToBuildListViewModel(builds);
                return View(buildViewModel);
            }
            else
            {
                return View(new BuildListViewModel());
            }
        }

        public ActionResult GetBuildDetail(string subProjectName, int buildId, string date)
        {
            var buildDetail = buildDetailService.GetById(buildId);
            ViewBag.subProjectName = subProjectName;
            ViewBag.buildId = buildId;
            ViewBag.date = date;

            if (buildDetail != null)
            {
                return View(buildDetail);
            }
            else
            {
                return View(new BuildDetail());
            }
        }

        public ActionResult Setting()
        {
            return View();
        }

        public ActionResult CronSetting()
        {
            var cronSetting = this.cronJobSettingService.GetCronJobSettingFirst();
            if (cronSetting != null)
            {
                return View(cronSetting);
            }
            else
            {
                return View(new CronJobSetting()
                {
                    Id = 1,
                    CronTime = new DateTime().ToUniversalTime(),
                    IsActivate = false,
                    IsDownloadEveryDay = false
                });
            }
        }

        [HttpPost]
        public ActionResult CronSetting(CronJobSetting cronJobSetting)
        {
            var cronSettings = this.cronJobSettingService.AddOrUpdate(cronJobSetting);
            return View(cronSettings);
        }

        [HttpGet]
        public async Task<ActionResult> RunCronJobExternal()
        {
            bool isDownloadSuccess = this.commonService.DowloadBuildDetail();
            if (isDownloadSuccess)
                return Json(new
                {
                    status = 200,
                    data = new
                    {
                        message = "Cron job success"
                    }
                }, JsonRequestBehavior.AllowGet);
            return Json(new
            {
                status = 500,
                data = new
                {
                    message = "Cron job fail"
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CleanAndRunCronJob()
        {
            bool isDownloadSuccess = this.commonService.DeleteBuildDetailAndDowload();
            if (isDownloadSuccess)
                return Json(new
                {
                    status = 200,
                    data = new
                    {
                        message = "Cronjob run success"
                    }
                });
            return Json(new
            {
                status = 500,
                data = new
                {
                    message = "Cronjob run fail"
                }
            });
        }

        [HttpPost]
        public JsonResult RunCronJobInternal()
        {
            bool isDownloadSuccess = this.commonService.DowloadBuildDetail();
            if (isDownloadSuccess)
                return Json(new
                {
                    status = 200,
                    data = new
                    {
                        message = "Cronjob run success"
                    }
                });
            return Json(new
            {
                status = 500,
                data = new
                {
                    message = "Cronjob run fail"
                }
            });
        }

        [HttpPost]
        public JsonResult GetProjectAndSubProject()
        {

            return Json(new
            {
                status = 200,
                data = new
                {
                    dashBoards = subProjectService.GetSubProjectAndCheckActive(),
                }
            });
        }

        [HttpPost]
        public JsonResult GetPieChartData(int subProjectId)
        {
            var lastBuildDetail = this.buildDetailService.GetLastBuildDetail(subProjectId);
            if (lastBuildDetail != null)
            {
                return Json(new
                {
                    status = 200,
                    data = new
                    {
                        buildId = lastBuildDetail.Id,
                        fail = lastBuildDetail.Fail,
                        pass = lastBuildDetail.Pass,
                        notRun = lastBuildDetail.NotRun,
                        date = string.Format("{0:yyyy-MM-dd HH:mm:ss tt}", lastBuildDetail.DateTime),
                    }
                });
            }
            return Json(new
            {
                status = 500,
                data = new { }
            });
        }

        [HttpPost]
        public JsonResult GetGridLineData(int subProjectId)
        {
            var last7Day = this.commonService.GetDataLast7Day(subProjectId);
            if (last7Day != null)
                return Json(new
                {
                    status = 200,
                    data = new
                    {
                        fails = last7Day.Fails,
                        pass = last7Day.Pass,
                        notRun = last7Day.NotRun,
                        labels = last7Day.Labels
                    }
                });
            return Json(new
            {
                status = 500,
                data = new
                {
                }
            });
        }
    }
}