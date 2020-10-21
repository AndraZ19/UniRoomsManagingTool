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
    public class SaliController : Controller
    {
        private dbSchoolsEntities _db = new dbSchoolsEntities();
        // GET: Sali
        public ActionResult Index()
        {
            return View(_db.Sali.ToList());
        }

        // GET: Sali/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sali/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sali/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")]Sala salaToAdd)
        {
            try
            {
                _db.Sali.Add(salaToAdd);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sali/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sala sala = _db.Sali.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            return View(sala);
        }

        // POST: Sali/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = " Nume, Etaj, Cladire, Echipament_video, PC, Capacitate, Libera")] Sala sala)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(sala).State = EntityState.Modified;
                    _db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(sala);
            }
        }

        // GET: Sali/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sala sala = _db.Sali.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            return View(sala);
        }

        // POST: Sali/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {

                Sala sala = _db.Sali.Find(id);
                _db.Sali.Remove(sala);
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
