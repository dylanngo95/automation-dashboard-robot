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
    public interface ICronJobSettingService {
        CronJobSetting Add(CronJobSetting cronJobSetting);

        CronJobSetting Update(CronJobSetting cronJobSetting);

        CronJobSetting Delete(int id);

        IEnumerable<CronJobSetting> GetAll();

        CronJobSetting GetById(int id);

        CronJobSetting GetCronJobSettingFirst();

        CronJobSetting AddOrUpdate(CronJobSetting cronJobSetting);

        void Save();
    }

    public class CronJobSettingService : ICronJobSettingService
    {

        private ICronJobSettingRepository cronJobSettingRepository;

        private IUnitOfWork unitOfWork;

        public CronJobSettingService(ICronJobSettingRepository cronJobSettingRepository, IUnitOfWork unitOfWork)
        {
            this.cronJobSettingRepository = cronJobSettingRepository;
            this.unitOfWork = unitOfWork;
        }

        public CronJobSetting Add(CronJobSetting cronJobSetting)
        {
            var result = this.cronJobSettingRepository.Add(cronJobSetting);
            unitOfWork.Commit();
            return result;
        }

        public CronJobSetting AddOrUpdate(CronJobSetting cronJobSetting)
        {
            var cronJob = this.cronJobSettingRepository.GetCronJobSettingFirst();
            if (cronJob != null)
               return this.cronJobSettingRepository.UpdateResult(cronJobSetting);
            return this.cronJobSettingRepository.Add(cronJobSetting);
        }

        public CronJobSetting Delete(int id)
        {
            return this.cronJobSettingRepository.Delete(id);
        }

        public IEnumerable<CronJobSetting> GetAll()
        {
            return this.cronJobSettingRepository.GetAll().AsEnumerable();
        }

        public CronJobSetting GetById(int id)
        {
            return this.cronJobSettingRepository.GetSingleById(id);
        }

        public CronJobSetting GetCronJobSettingFirst()
        {
            return this.cronJobSettingRepository.GetCronJobSettingFirst();
        }

        public void Save()
        {
            this.unitOfWork.Commit();
        }

        public CronJobSetting Update(CronJobSetting cronJobSetting)
        {
            return this.cronJobSettingRepository.UpdateResult(cronJobSetting);
        }
    }
}
