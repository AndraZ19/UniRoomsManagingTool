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
    public class ProfesoriController : Controller
    {
        private dbSchoolsEntities _db = new dbSchoolsEntities();

        // GET: Profesori
        public ActionResult Index()
        {
            return View(_db.Profesori.ToList());
            
        }

        // GET: Profesori/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Profesori/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profesori/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create([Bind(Exclude="Id")]Profesor profesorToCreate)
        {
            try
            {
                _db.Profesori.Add(profesorToCreate);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Profesori/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = _db.Profesori.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // POST: Profesori/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Nume, Discipline")] Profesor profesor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(profesor).State = EntityState.Modified;
                    _db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(profesor);
            }
        }

        // GET: Profesori/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = _db.Profesori.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // POST: Profesori/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {

               Profesor profesor = _db.Profesori.Find(id);
                _db.Profesori.Remove(profesor);
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
