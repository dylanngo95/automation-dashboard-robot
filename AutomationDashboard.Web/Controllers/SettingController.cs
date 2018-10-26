using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutomationDashboard.Data;
using AutomationDashboard.Model.Models;

namespace AutomationDashboard.Web.Controllers
{
    public class SettingController : Controller
    {
        private AutomationDashboardDbContext db = new AutomationDashboardDbContext();

        // GET: Setting
        public async Task<ActionResult> Index()
        {
            return View(await db.Settings.ToListAsync());
        }

        // GET: Setting/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = await db.Settings.FindAsync(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // GET: Setting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Setting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Domain,UrlBuildStart,UrlBuildEnd,UrlReportStart,UrlReportEnd")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Settings.Add(setting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(setting);
        }

        // GET: Setting/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = await db.Settings.FindAsync(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Domain,UrlBuildStart,UrlBuildEnd,UrlReportStart,UrlReportEnd")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        // GET: Setting/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = await db.Settings.FindAsync(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Setting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Setting setting = await db.Settings.FindAsync(id);
            db.Settings.Remove(setting);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
