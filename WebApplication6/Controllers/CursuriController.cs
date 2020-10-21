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
    public class CursuriController : Controller
    {
        private dbSchoolsEntities _db = new dbSchoolsEntities();
        // GET: Cursuri
        public ActionResult Index()
        {
            return View(_db.Cursuri.ToList());
        }

        // GET: Cursuri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curs curs = _db.Cursuri.Find(id);
            if (curs == null)
            {
                return HttpNotFound();
            }
            return View(curs);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Cursuri/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cursuri/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] Curs cursToAdd)
        {
            try
            {
                _db.Cursuri.Add(cursToAdd);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cursuri/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curs curs = _db.Cursuri.Find(id);
            if (curs == null)
            {
                return HttpNotFound();
            }
            return View(curs);
        }
        [Authorize(Roles = "Administrator")]

        // POST: Cursuri/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "CursId, Nume, Capacitate, PCReq, VideoReq, ProfId")] Curs curs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(curs).State = EntityState.Modified;
                    _db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(curs);
            }
        }
        [Authorize(Roles = "Administrator")]

        // GET: Cursuri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curs curs = _db.Cursuri.Find(id);
            if (curs == null)
            {
                return HttpNotFound();
            }
            return View(curs);
        }

        // POST: Cursuri/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Curs curs = _db.Cursuri.Find(id);
                _db.Cursuri.Remove(curs);
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
