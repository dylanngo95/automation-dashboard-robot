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
    public class BuildController : Controller
    {
        private AutomationDashboardDbContext db = new AutomationDashboardDbContext();

        // GET: Build
        public async Task<ActionResult> Index()
        {
            var builds = db.Builds.Include(b => b.BuildDetails).Include(b => b.SubProject);
            return View(await builds.ToListAsync());
        }

        // GET: Build/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Build build = await db.Builds.FindAsync(id);
            if (build == null)
            {
                return HttpNotFound();
            }
            return View(build);
        }

        // GET: Build/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.BuildDetails, "Id", "Id");
            ViewBag.SubProjectId = new SelectList(db.SubProjects, "Id", "FullName");
            return View();
        }

        // POST: Build/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,BuildId,Number,Status,State,Pinned,WebUrl,Count,StartDate,FinishDate,Duration,SubProjectId")] Build build)
        {
            if (ModelState.IsValid)
            {
                db.Builds.Add(build);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.BuildDetails, "Id", "Id", build.Id);
            ViewBag.SubProjectId = new SelectList(db.SubProjects, "Id", "FullName", build.SubProjectId);
            return View(build);
        }

        // GET: Build/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Build build = await db.Builds.FindAsync(id);
            if (build == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.BuildDetails, "Id", "Id", build.Id);
            ViewBag.SubProjectId = new SelectList(db.SubProjects, "Id", "FullName", build.SubProjectId);
            return View(build);
        }

        // POST: Build/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BuildId,Number,Status,State,Pinned,WebUrl,Count,StartDate,FinishDate,Duration,SubProjectId")] Build build)
        {
            if (ModelState.IsValid)
            {
                db.Entry(build).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.BuildDetails, "Id", "Id", build.Id);
            ViewBag.SubProjectId = new SelectList(db.SubProjects, "Id", "FullName", build.SubProjectId);
            return View(build);
        }

        // GET: Build/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Build build = await db.Builds.FindAsync(id);
            if (build == null)
            {
                return HttpNotFound();
            }
            return View(build);
        }

        // POST: Build/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Build build = await db.Builds.FindAsync(id);
            db.Builds.Remove(build);
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
