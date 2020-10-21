using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.ViewModels;

namespace WebApplication6.Controllers
{
    public class PrezenteController : Controller
    {
        private dbSchoolsEntities _db = new dbSchoolsEntities();
        List<Student> students;
        List<Profesor> profesori;
        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        PrezentaViewModel ev = new PrezentaViewModel();
        List<Prezente> prezente;
        List<Prezente> prez = new List<Prezente>();


        // GET: Prezente
        public ActionResult Index()
        {
            prezente =  _db.Prezente.ToList();
            profesori = _db.Profesori.ToList();
            students = _db.Studenti.ToList();
            if (User.IsInRole("Student"))
            {
                foreach (Student student_ in students)
                {
                    if (user.Id == student_.GUID)
                        prez.AddRange(prezente.FindAll(x => x.StudentId == student_.Id));
                }
         
            }
            if (User.IsInRole("Profesor"))
            {
                foreach (Profesor prof in profesori)
                {
                    if(user.Id == prof.Guid)
                        prez.AddRange(prezente.FindAll(x => x.Events.ProfId == prof.Id));

                }

          

            }

            return View(prez);
        }

        public Action GetPrez()
        {
            prezente = _db.Prezente.ToList();
            profesori = _db.Profesori.ToList();
            students = _db.Studenti.ToList();
            if (User.IsInRole("Student"))
            {
                foreach (Student student_ in students)
                {
                    if (user.Id == student_.GUID)
                        prez.AddRange(prezente.FindAll(x => x.StudentId == student_.Id));
                }

            }
            if (User.IsInRole("Profesor"))
            {
                foreach (Profesor prof in profesori)
                {
                    if (user.Id == prof.Guid)
                        prez.AddRange(prezente.FindAll(x => x.Events.ProfId == prof.Id));

                }



            }
            return null;
        }

        public ActionResult PrintView()
        {
            GetPrez();
            var report = new ViewAsPdf("Index", prez);
            return report;


        }

        // GET: Prezente/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public Action Populate()
        {
            List<Student> students = new List<Student>(_db.Studenti.ToList());
            List<InscriereCursuri> inscriere = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            List<Events> events = new List<Events>(_db.Events.ToList());
            List<Events> events_ = new List<Events>();




            Student student = students.First(x => x.GUID == user.Id);
            foreach (Curs curs_ in _cursuri)
            {
                foreach (InscriereCursuri inscriereCursuri in inscriere)
                {
                    if (student.Id == inscriereCursuri.StudId)
                    {
                        if (inscriereCursuri.ClassId == curs_.CursId)
                        {
                            events_.Add(events.FirstOrDefault(x => x.CursId == curs_.CursId));
                        }
                    }

                }
            }

            var items = from Events ev in events_
                        select new SelectListItem { Value = ev.Id.ToString(), Text = ev.Description };
            ViewData["Eventslist"] = items;
            return null;
        }
        // GET: Prezente/Create
        [Authorize(Roles = "Student")]
        public ActionResult Create()
        {
            List<Student> students = new List<Student>(_db.Studenti.ToList());
            List<InscriereCursuri> inscriere = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            List<Events> events = new List<Events>(_db.Events.ToList());
            List<Events> events_ = new List<Events>();




            Student student = students.First(x => x.GUID == user.Id);
            foreach (Curs curs_ in _cursuri)
                {
                    foreach (InscriereCursuri inscriereCursuri in inscriere)
                    {
                    if (student.Id == inscriereCursuri.StudId) {
                        if (inscriereCursuri.ClassId == curs_.CursId)
                        {
                            events_.Add(events.FirstOrDefault(x => x.CursId == curs_.CursId));
                        }
                    }

                }
            }
     
            var items = from Events ev in events_
                        select new SelectListItem { Value = ev.Id.ToString(), Text = ev.Description };
            ViewData["Eventslist"] = items;
            var pvm = new PrezentaViewModel();
            pvm.cursuri = new SelectList(items);
            pvm.Prezenta = false;
            return View(pvm);


            
        }

        // POST: Prezente/Create
        [Authorize(Roles = "Student")]
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] Prezente prezentaToCreate)
        {
            Populate();
            try
            {
                List<Student> students = new List<Student>(_db.Studenti.ToList());
                Student student = students.First(x => x.GUID == user.Id);
                prezentaToCreate.StudentId = student.Id;
                //ev.retrieve(ev);

                _db.Prezente.Add(prezentaToCreate);
                _db.SaveChanges();
            }
            catch
            {

                return RedirectToAction("Index");

            }

            return RedirectToAction("Index");
        }

        // GET: Prezente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Prezente/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Prezente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prezente prezenta = _db.Prezente.Find(id);
            if (prezenta == null)
            {
                return HttpNotFound();
            }
            return View(prezenta);
        }
        // POST: Prezente/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Prezente prezenta = _db.Prezente.Find(id);
                _db.Prezente.Remove(prezenta);
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
