using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutomationDashboard.Data;
using AutomationDashboard.Model.Models;

namespace AutomationDashboard.Common.XMLHandler
{
    public class TeamCity : BuildDetailLog, IDowloadXml
    {

        public void SaveBuils(SubProject subproject, Setting setting, Project project, AutomationDashboardDbContext db)
        {
            Debug.WriteLine("==========================>");

            try
            {
                String urlApi = String.Format(
                    "{0}{1}{2}{3}",
                    setting.Domain,
                    setting.UrlBuildStart,
                    subproject.FullName,
                    setting.UrlBuildEnd
                    );
                Debug.WriteLine("Url Api: " + urlApi);

                WebRequest request = WebRequest.Create(urlApi);
                request.Credentials = new NetworkCredential(project.UserName, project.Password);

                using (WebResponse response = request.GetResponse())
                {
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(body);

                    XmlElement root = xmlDocument.DocumentElement;

                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        XmlNode item = root.ChildNodes[i];

                        var startDate = DateTime.ParseExact(item.ChildNodes[1].InnerText, "yyyyMMddTHHmmss+ffff", CultureInfo.InvariantCulture);
                        var finishDate = DateTime.ParseExact(item.ChildNodes[2].InnerText, "yyyyMMddTHHmmss+ffff", CultureInfo.InvariantCulture);
                        var duration = finishDate - startDate;

                        Build build = new Build()
                        {
                            BuildId = Int32.Parse(item.Attributes["id"].Value),
                            SubProjectId = subproject.Id,
                            Number = Int32.Parse(item.Attributes["number"].Value),
                            Status = item.Attributes["status"].Value,
                            State = item.Attributes["state"].Value,
                            Pinned = item.Attributes["pinned"].Value,
                            WebUrl = item.Attributes["webUrl"].Value,
                            Count = Int32.Parse(item.ChildNodes[0].Attributes["count"].Value),
                            StartDate = startDate,
                            FinishDate = finishDate,
                            Duration = duration.ToString()
                        };

                        db.Builds.Add(build);
                        db.SaveChanges();

                    }

                }
                Debug.WriteLine("clone data succes");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
            }
        }

        public void SaveBuildDetail(Build build, Setting setting, Project project, SubProject subProject, AutomationDashboardDbContext db)
        {
            //var subProject = dataManager.db.SubProjects.Single(p => p.Id == build.SubProjectId);
            //var project = dataManager.db.Projects.Single(p => p.Id == subProject.ProjectId);
            //var setting = dataManager.db.Settings.Single(p => p.Id == project.SettingId);

            try
            {
                WebRequest request = WebRequest.Create(
                    String.Format("{0}{1}{2}{3}",
                    setting.Domain,
                    setting.UrlReportStart,
                    build.BuildId.ToString(),
                    setting.UrlReportEnd
                    ));
                request.Credentials = new NetworkCredential(project.UserName, project.Password);

                // Add info to database
                AddRequestBuildDetail(build.BuildId, build.WebUrl, db);

                using (WebResponse response = request.GetResponse())
                {
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(body);
                    XmlElement root = xmlDocument.DocumentElement;
                    XmlNodeList statistics = xmlDocument.GetElementsByTagName("statistics");

                    int notRun = 0;
                    try
                    {
                        notRun = Int16.Parse(statistics[0].ChildNodes[1].ChildNodes[0].Attributes["fail"].Value);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error: " + ex);
                    }

                    BuildDetail buildDetail = new BuildDetail()
                    {
                        Id = build.Id,
                        Pass = Int32.Parse(statistics[0].FirstChild.LastChild.Attributes["pass"].Value),
                        Fail = Int32.Parse(statistics[0].FirstChild.LastChild.Attributes["fail"].Value),
                        NotRun = notRun,
                        DateTime = build.FinishDate
                        //Total = (Int32.Parse(statistics[0].FirstChild.LastChild.Attributes["pass"].Value) + Int32.Parse(statistics[0].FirstChild.LastChild.Attributes["fail"].Value)) + notRun,
                    };

                    db.BuildDetails.Add(buildDetail);
                    db.SaveChanges();

                    // When dowload buildDetail complete -> delete info in ReuestBuildDetailLog
                    RemoveRequestBuildDetail(build.BuildId, db);

                }

            }
            catch (Exception ex)
            {

                RemoveRequestBuildDetail(build.BuildId, db);
                Debug.WriteLine("Ex: " + ex);
            }
        }

        public void SaveBuildAndCheckTime(SubProject subproject, Setting setting, Project project, AutomationDashboardDbContext db)
        {
            Debug.WriteLine("==========================>");

            var lastBuild = db.Builds.Where(p => p.SubProjectId == subproject.Id).OrderByDescending(p => p.FinishDate).FirstOrDefault();

            try
            {
                String urlApi = String.Format(
                    "{0}{1}{2}{3}",
                    setting.Domain,
                    setting.UrlBuildStart,
                    subproject.FullName,
                    setting.UrlBuildEnd
                    );
                Debug.WriteLine("Url Api: " + urlApi);

                WebRequest request = WebRequest.Create(urlApi);
                request.Credentials = new NetworkCredential(project.UserName, project.Password);

                using (WebResponse response = request.GetResponse())
                {
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(body);

                    XmlElement root = xmlDocument.DocumentElement;

                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        XmlNode item = root.ChildNodes[i];

                        var startDate = DateTime.ParseExact(item.ChildNodes[1].InnerText, "yyyyMMddTHHmmss+ffff", CultureInfo.InvariantCulture);
                        var finishDate = DateTime.ParseExact(item.ChildNodes[2].InnerText, "yyyyMMddTHHmmss+ffff", CultureInfo.InvariantCulture);
                        var duration = finishDate - startDate;

                        // Check build is New
                        if (lastBuild != null)
                        {
                            if (DateTime.Compare(finishDate, lastBuild.FinishDate) > 0)
                            {

                                Build build = new Build()
                                {
                                    BuildId = Int32.Parse(item.Attributes["id"].Value),
                                    SubProjectId = subproject.Id,
                                    Number = Int32.Parse(item.Attributes["number"].Value),
                                    Status = item.Attributes["status"].Value,
                                    State = item.Attributes["state"].Value,
                                    Pinned = item.Attributes["pinned"].Value,
                                    WebUrl = item.Attributes["webUrl"].Value,
                                    Count = Int32.Parse(item.ChildNodes[0].Attributes["count"].Value),
                                    StartDate = startDate,
                                    FinishDate = finishDate,
                                    Duration = duration.ToString()
                                };

                                db.Builds.Add(build);
                                db.SaveChanges();

                                // Dowload and save buildDetail
                                SaveBuildDetail(build, setting, project, subproject, db);
                            }
                        }
                        else
                        {
                            Build build = new Build()
                            {
                                BuildId = Int32.Parse(item.Attributes["id"].Value),
                                SubProjectId = subproject.Id,
                                Number = Int32.Parse(item.Attributes["number"].Value),
                                Status = item.Attributes["status"].Value,
                                State = item.Attributes["state"].Value,
                                Pinned = item.Attributes["pinned"].Value,
                                WebUrl = item.Attributes["webUrl"].Value,
                                Count = Int32.Parse(item.ChildNodes[0].Attributes["count"].Value),
                                StartDate = startDate,
                                FinishDate = finishDate,
                                Duration = duration.ToString()
                            };

                            db.Builds.Add(build);
                            db.SaveChanges();

                            // Dowload and save buildDetail
                            SaveBuildDetail(build, setting, project, subproject, db);
                        }


                    }

                }

                Debug.WriteLine("clone data succes");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
            }
        }

    }
}
