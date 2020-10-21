using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    public class StudentiController : Controller
    {
        private dbSchoolsEntities _db = new dbSchoolsEntities();
        List<Profesor> profesori;
        List<Student> students;
        List<Student> students_;



        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());


        // GET: Studenti
        [Authorize(Roles = "Administrator, Profesor")]


        public ActionResult Index()
        {
   
            profesori = _db.Profesori.ToList();
            students = _db.Studenti.ToList();
            students_ = _db.Studenti.ToList();

            if (User.IsInRole("Profesor"))
            {
                students_.Clear();
                List<Curs> cursuri = _db.Cursuri.ToList();
                List<InscriereCursuri> inCursuri = _db.InscriereCursuri.ToList();
                List<InscriereCursuri> inCursuri_ = new List<InscriereCursuri>();



                foreach (Profesor prof in profesori)
                {

                    if (user.Id == prof.Guid)
                    {
                        cursuri = cursuri.FindAll(x => x.ProfId == prof.Id);
                        foreach (Curs curs_ in cursuri) {
                            inCursuri_.AddRange( inCursuri.FindAll(x => x.ClassId == curs_.CursId));
                                }
                        foreach(InscriereCursuri inc in inCursuri)
                        {
                            students_.Add(students.FirstOrDefault(x => x.Id == inc.StudId));
                        }

                    }
                }



            }

            return View(students);
        }

        // GET: Studenti/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Studenti/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Studenti/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create([Bind(Exclude ="Id")] Student studentToCreate)
        {
            try
            {
                _db.Studenti.Add(studentToCreate);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Studenti/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _db.Studenti.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Studenti/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Nume, An, Medie")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(student).State = EntityState.Modified;
                    _db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Studenti/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _db.Studenti.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

            // POST: Studenti/Delete/5
            [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Student student = _db.Studenti.Find(id);
                _db.Studenti.Remove(student);
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
