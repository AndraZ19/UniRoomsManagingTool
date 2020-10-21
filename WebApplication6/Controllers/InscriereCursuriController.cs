using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;


namespace WebApplication6.Controllers
{
    public class InscriereCursuriController : Controller
    {
        private dbSchoolsEntities _db = new dbSchoolsEntities();
        // GET: InscriereCursuri
        public ActionResult Index()
        {
            return View();
        }

        // GET: InscriereCursuri/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        // GET: InscriereCursuri/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InscriereCursuri/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")]InscriereCursuri inscriere)
        {
            try
            {
                _db.InscriereCursuri.Add(inscriere);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InscriereCursuri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InscriereCursuri inscriere = _db.InscriereCursuri.Find(id);
            if (inscriere == null)
            {
                return HttpNotFound();
            }
            return View(inscriere);
        }

        // POST: InscriereCursuri/Edit/5
        [HttpPost]
        public ActionResult Edit(InscriereCursuri inscriere)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(inscriere).State = EntityState.Modified;
                    _db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(inscriere);
            }
        }

        // GET: InscriereCursuri/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InscriereCursuri inscriere = _db.InscriereCursuri.Find(id);
            if (inscriere == null)
            {
                return HttpNotFound();
            }
            return View(inscriere);
        }

        // POST: InscriereCursuri/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                InscriereCursuri inscriere = _db.InscriereCursuri.Find(id);
                _db.InscriereCursuri.Remove(inscriere);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
    }
}
