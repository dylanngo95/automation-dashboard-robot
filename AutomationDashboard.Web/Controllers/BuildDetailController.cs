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
    public class BuildDetailController : Controller
    {
        private AutomationDashboardDbContext db = new AutomationDashboardDbContext();

        // GET: BuildDetail
        public async Task<ActionResult> Index()
        {
            var buildDetails = db.BuildDetails.Include(b => b.Build);
            return View(await buildDetails.ToListAsync());
        }

        // GET: BuildDetail/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildDetail buildDetail = await db.BuildDetails.FindAsync(id);
            if (buildDetail == null)
            {
                return HttpNotFound();
            }
            return View(buildDetail);
        }

        // GET: BuildDetail/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Builds, "Id", "Status");
            return View();
        }

        // POST: BuildDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Pass,Fail,NotRun,DateTime")] BuildDetail buildDetail)
        {
            if (ModelState.IsValid)
            {
                db.BuildDetails.Add(buildDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Builds, "Id", "Status", buildDetail.Id);
            return View(buildDetail);
        }

        // GET: BuildDetail/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildDetail buildDetail = await db.BuildDetails.FindAsync(id);
            if (buildDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Builds, "Id", "Status", buildDetail.Id);
            return View(buildDetail);
        }

        // POST: BuildDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Pass,Fail,NotRun,DateTime")] BuildDetail buildDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buildDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Builds, "Id", "Status", buildDetail.Id);
            return View(buildDetail);
        }

        // GET: BuildDetail/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildDetail buildDetail = await db.BuildDetails.FindAsync(id);
            if (buildDetail == null)
            {
                return HttpNotFound();
            }
            return View(buildDetail);
        }

        // POST: BuildDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BuildDetail buildDetail = await db.BuildDetails.FindAsync(id);
            db.BuildDetails.Remove(buildDetail);
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
