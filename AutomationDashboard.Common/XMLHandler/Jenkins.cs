using AutomationDashboard.Data;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutomationDashboard.Common.XMLHandler
{
    public class Jenkins : BuildDetailLog, IDowloadXml
    {
        public void SaveBuils(SubProject subproject, Setting setting, Project project, AutomationDashboardDbContext db)
        {
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
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] credentialBuffer = new UTF8Encoding().GetBytes(
                   project.UserName + ":" +
                   project.Password
                );
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);
                request.PreAuthenticate = true;


                using (WebResponse response = request.GetResponse())
                {
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(body);

                    XmlElement root = xmlDocument.DocumentElement;

                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        XmlNode build = root.ChildNodes[i];

                        var startTimeStamp = Convert.ToDouble(build.ChildNodes[4].InnerText.ToString());
                        var durationTimeStamp = Convert.ToDouble(build.ChildNodes[0].InnerText.ToString());

                        var id = Convert.ToInt32(build.ChildNodes[1].InnerText.ToString());
                        var number = Convert.ToInt32(build.ChildNodes[2].InnerText.ToString());
                        var status = build.ChildNodes[3].InnerText.ToString();
                        var startDate = DateTimeUtil.UnixTimeStampToDateTime(startTimeStamp);
                        var finishDate = DateTimeUtil.UnixTimeStampToDateTime(startTimeStamp + durationTimeStamp);
                        var duration = finishDate - startDate;

                        string state = "processing";
                        if (durationTimeStamp != 0)
                        {
                            state = "finished";
                        }

                        Build buildTemp = new Build()
                        {
                            BuildId = id,
                            SubProjectId = subproject.Id,
                            Number = number,
                            Status = status,
                            State = state,
                            StartDate = startDate,
                            FinishDate = finishDate,
                            Duration = duration.ToString()
                        };

                        db.Builds.Add(buildTemp);
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
            try
            {
                String urlApi = String.Format(
                    "{0}{1}{2}{3}{4}",
                    setting.Domain,
                    setting.UrlReportStart,
                    subProject.FullName + "/",
                    build.BuildId,
                    setting.UrlReportEnd
                    );
                Debug.WriteLine("Url Api: " + urlApi);

                WebRequest request = WebRequest.Create(urlApi);
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] credentialBuffer = new UTF8Encoding().GetBytes(
                   project.UserName + ":" +
                   project.Password
                );
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);
                request.PreAuthenticate = true;

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
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] credentialBuffer = new UTF8Encoding().GetBytes(
                   project.UserName + ":" +
                   project.Password
                );
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);
                request.PreAuthenticate = true;


                using (WebResponse response = request.GetResponse())
                {
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(body);

                    XmlElement root = xmlDocument.DocumentElement;

                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        XmlNode build = root.ChildNodes[i];

                        var startTimeStamp = Convert.ToDouble(build.ChildNodes[4].InnerText.ToString());
                        var durationTimeStamp = Convert.ToDouble(build.ChildNodes[0].InnerText.ToString());


                        var startDate = DateTimeUtil.UnixTimeStampToDateTime(startTimeStamp);
                        var finishDate = DateTimeUtil.UnixTimeStampToDateTime(startTimeStamp + durationTimeStamp);
                        var duration = finishDate - startDate;

                        string state = "processing";
                        if (durationTimeStamp != 0)
                        {
                            state = "finished";
                        }

                        if (lastBuild != null)
                        {
                            if (DateTime.Compare(finishDate, lastBuild.FinishDate) > 0)
                            {
                                Build buildTemp = new Build()
                                {
                                    BuildId = Convert.ToInt32(build.ChildNodes[1].InnerText.ToString()),
                                    SubProjectId = subproject.Id,
                                    Number = Convert.ToInt32(build.ChildNodes[2].InnerText.ToString()),
                                    Status = build.ChildNodes[3].InnerText.ToString(),
                                    State = state,
                                    StartDate = startDate,
                                    FinishDate = finishDate,
                                    Duration = duration.ToString()
                                };

                                db.Builds.Add(buildTemp);
                                db.SaveChanges();

                                // dowload build detail
                                SaveBuildDetail(buildTemp, setting, project, subproject, db);
                            }
                        }
                        else
                        {
                            Build buildTemp = new Build()
                            {
                                BuildId = Convert.ToInt32(build.ChildNodes[1].InnerText.ToString()),
                                SubProjectId = subproject.Id,
                                Number = Convert.ToInt32(build.ChildNodes[2].InnerText.ToString()),
                                Status = build.ChildNodes[3].InnerText.ToString(),
                                State = state,
                                StartDate = startDate,
                                FinishDate = finishDate,
                                Duration = duration.ToString()
                            };

                            db.Builds.Add(buildTemp);
                            db.SaveChanges();

                            // dowload build detail
                            SaveBuildDetail(buildTemp, setting, project, subproject, db);
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
