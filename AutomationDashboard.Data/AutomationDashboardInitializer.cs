using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data
{
    public class AutomationDashboardInitializer : CreateDatabaseIfNotExists<AutomationDashboardDbContext>
    {
        protected override void Seed(AutomationDashboardDbContext context)
        {
            IList<Setting> defaultSettings = new List<Setting>();
            defaultSettings.Add(new Setting()
            {
                Name = "TeamCity_DN_Production",
                Domain = "http://teamcity.dn.se",
                UrlBuildStart = "/httpAuth/app/rest/buildTypes/id:",
                UrlBuildEnd = "/builds?fields=build(id,buildTypeId,number,status,tags,state,startDate,finishDate,webUrl,pinned)",
                UrlReportStart = "/httpAuth/app/rest/builds/id:",
                UrlReportEnd = "/artifacts/content/Report/ChromeOutput.xml"

            });
            defaultSettings.Add(new Setting()
            {
                Name = "TeamCity_Local",
                Domain = "http://10.11.7.81:8080",
                UrlBuildStart = "/httpAuth/app/rest/buildTypes/id:",
                UrlBuildEnd = "/builds?fields=build(id,buildTypeId,number,status,tags,state,startDate,finishDate,webUrl,pinned)",
                UrlReportStart = "/httpAuth/app/rest/builds/id:",
                UrlReportEnd = "/artifacts/content/Report/ChromeOutput.xml"

            });
            defaultSettings.Add(new Setting()
            {
                Name = "Jenkins_FH",
                Domain = "http://fh-jenkins.niteco.se:8080",
                UrlBuildStart = "/job/",
                UrlBuildEnd = "/api/xml?tree=builds[number,status,timestamp,id,result,duration]",
                UrlReportStart = "/job/",
                UrlReportEnd = "/robot/report/output.xml"

            });

            context.Settings.AddRange(defaultSettings);

            base.Seed(context);
        }
    }
}
