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

    public interface ISettingService {

        Setting Add(Setting setting);

        void Update(Setting setting);

        Setting Delete(int id);

        IEnumerable<Setting> GetAll();

        Setting GetById(int id);

        void Save();
    }

    public class SettingService : ISettingService
    {
        private ISettingRepository settingRepository;

        private IUnitOfWork unitOfWork;

        public SettingService(ISettingRepository settingRepository, IUnitOfWork unitOfWork)
        {
            this.settingRepository = settingRepository;
            this.unitOfWork = unitOfWork;
        }

        public Setting Add(Setting setting)
        {
            var settingResult = this.settingRepository.Add(setting);
            unitOfWork.Commit();
            return settingResult;
        }

        public Setting Delete(int id)
        {
            return this.settingRepository.Delete(id);
        }

        public IEnumerable<Setting> GetAll()
        {
            return this.settingRepository.GetAll();
        }

        public Setting GetById(int id)
        {
            return this.settingRepository.GetSingleById(id);
        }


        public void Update(Setting setting)
        {
            this.settingRepository.Update(setting);
        }

        public void Save()
        {
            this.unitOfWork.Commit();
        }

    }
}
