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
    public class SubProjectController : Controller
    {
        private AutomationDashboardDbContext db = new AutomationDashboardDbContext();

        // GET: SubProject
        public async Task<ActionResult> Index()
        {
            var subProjects = db.SubProjects.Include(s => s.Project);
            return View(await subProjects.ToListAsync());
        }

        // GET: SubProject/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubProject subProject = await db.SubProjects.FindAsync(id);
            if (subProject == null)
            {
                return HttpNotFound();
            }
            return View(subProject);
        }

        // GET: SubProject/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "FullName");
            return View();
        }

        // POST: SubProject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FullName,DisplayName,Activate,ProjectId")] SubProject subProject)
        {
            if (ModelState.IsValid)
            {
                db.SubProjects.Add(subProject);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "FullName", subProject.ProjectId);
            return View(subProject);
        }

        // GET: SubProject/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubProject subProject = await db.SubProjects.FindAsync(id);
            if (subProject == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "FullName", subProject.ProjectId);
            return View(subProject);
        }

        // POST: SubProject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FullName,DisplayName,Activate,ProjectId")] SubProject subProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subProject).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "FullName", subProject.ProjectId);
            return View(subProject);
        }

        // GET: SubProject/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubProject subProject = await db.SubProjects.FindAsync(id);
            if (subProject == null)
            {
                return HttpNotFound();
            }
            return View(subProject);
        }

        // POST: SubProject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubProject subProject = await db.SubProjects.FindAsync(id);
            db.SubProjects.Remove(subProject);
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
