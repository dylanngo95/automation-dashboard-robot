using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutomationDashboard.Common.XMLHandler;
using AutomationDashboard.Data;
using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Data.Repositories;
using AutomationDashboard.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomationDashboard.Web.App_Start
{
    public class Startup
    {
        public static void ConfigAutofac() {

            var builder = new ContainerBuilder();
            // Register Controller
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register Api
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<AutomationDashboardDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<BuildDetailLog>().AsSelf().InstancePerRequest();
            builder.RegisterType<TeamCity>().As<IDowloadXml>().InstancePerRequest();
            builder.RegisterType<Jenkins>().As<IDowloadXml>().InstancePerRequest();


            // Repositories
            builder.RegisterAssemblyTypes(typeof(ProjectRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(SubProjectRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(BuildRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(CronJobSettingRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(ProjectService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(SubProjectService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(CommonService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(BuildService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(CronJobSettingService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}