using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Rotativa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication6.Models;
using WebApplication6.ViewModels;

namespace WebApplication6.Controllers
{
    public class EventController : Controller
    {
        private dbSchoolsEntities _db = new dbSchoolsEntities();
        public List<Curs> cursuri = new List<Curs>();
        public List<SelectListItem> Courses { get; set; }
        EventViewModel ev = new EventViewModel();

        protected ApplicationDbContext ApplicationDbContext { get; set; }


        protected UserManager<ApplicationUser> UserManager { get; set; }


        public Events _event = new Events();
        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        public EventController(List<Curs> cursuri, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, Events @event)
        {
            this.cursuri = cursuri;
            ApplicationDbContext = applicationDbContext;
            UserManager = userManager;
            _event = @event;
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public EventController()
        {
        }

        Profesor profesor;
        Student student;
        public Action Populate()
        {
            List<Profesor> profesori = new List<Profesor>(_db.Profesori.ToList());
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            List<Student> students = new List<Student>(_db.Studenti.ToList());
            List<InscriereCursuri> inscriere = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());
            if (User.IsInRole("Student"))
            {
                foreach (Student student_ in students)
                {
                    if (user.Id == student_.GUID)
                        student = student_;
                }
                foreach (Curs curs_ in _cursuri)
                {
                    foreach (InscriereCursuri inscriereCursuri in inscriere)
                    {
                        if (student.Id == inscriereCursuri.StudId && inscriereCursuri.ClassId == curs_.CursId)
                            cursuri.Add(curs_);
                    }

                }
            }
            if (User.IsInRole("Profesor"))
            {
                foreach (Profesor prof in profesori)
                {
                    if (user.Id == prof.Guid)
                        profesor = prof;

                }

                foreach (Curs curs_ in _cursuri)
                {
                    if (curs_.ProfId == profesor.Id)
                        cursuri.Add(curs_);
                }

            }

            var items = from Curs curs in cursuri
                        select new SelectListItem { Value = curs.CursId.ToString(), Text = curs.Nume };

            ViewData["Classlist"] = items;

            return null;


        }
        [HttpPost]
        public  Action SetRoomsToStatus(String JSONString )
        {
            List<Sala> sali = new List<Sala>(_db.Sali.ToList());
            List<Events> events = new List<Events>(_db.Events.ToList());
            

            DateTimeOffset date = Convert.ToDateTime(JSONString);



            foreach (Events ev in events)
            {
                    if(ev.Start_time<date && ev.End_time > date)
                    {
                    _db.Sali.FirstOrDefault(x => x.Id == ev.SalaId).Libera = false;
                    _db.SaveChanges();
                    }
           
            };
            return null;


        }
        [HttpPost]
        public Action SetRoomsToStatusEnd( String JSONStringEnd)
        {
            List<Sala> sali = new List<Sala>(_db.Sali.ToList());
            List<Events> events = new List<Events>(_db.Events.ToList());


            DateTimeOffset endDate = Convert.ToDateTime(JSONStringEnd);



            foreach (Events ev in events)
            {
               if (ev.Start_time < endDate && ev.End_time > endDate)
                 {
                _db.Sali.FirstOrDefault(x => x.Id == ev.SalaId).Libera = false;
                _db.SaveChanges();
                }

            };
            return null;


        }
        public ActionResult GetSala(int id)
        {
            List<Sala> sali = new List<Sala>(_db.Sali.ToList());
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            Curs curs = _cursuri.Find(x => x.CursId == id);
            List<InscriereCursuri> inscrieri = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());

            List<SelectListItem> items = new List<SelectListItem>();
            var inscrieri_ = inscrieri.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .ToDictionary(x => x.Key, y => y.Count());
            if (inscrieri.Count() == 0)
            {
                foreach (Sala sala in sali)
                {
                    if (sala.Libera == true)
                        items.Add(new SelectListItem() { Text = sala.Nume, Value = sala.Id.ToString() });
                }
            }
            else {
                InscriereCursuri insc = inscrieri.Find(x => x.ClassId == curs.CursId);
                
                
                    inscrieri_.TryGetValue(insc, out int number);
                
                List<Sala> saliFiltrate = sali.FindAll(x => x.Capacitate >= number && x.Libera == true);


                foreach (Sala sala in saliFiltrate)
                {
                    items.Add(new SelectListItem() { Text = sala.Nume, Value = sala.Id.ToString() });

                } }


            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPCSala(int id)
        {
            List<Sala> sali = new List<Sala>(_db.Sali.ToList());
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            Curs curs = _cursuri.Find(x => x.CursId == id);
            List<InscriereCursuri> inscrieri = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());

            List<SelectListItem> items = new List<SelectListItem>();
            var inscrieri_ = inscrieri.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .ToDictionary(x => x.Key, y => y.Count());
            if (inscrieri.Count() == 0 )
            {
                var data = sali
               .Where(x => x.Libera == true && x.PC == true)
               .Select(l => new { Value = l.Id.ToString(), Text = l.Nume });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                InscriereCursuri insc = inscrieri.Find(x => x.ClassId == curs.CursId);
                inscrieri_.TryGetValue(insc, out int number);

                var data = sali.Where(x => x.Capacitate >= number && x.Libera == true && x.PC == true).Select(l => new { Value = l.Id.ToString(), Text = l.Nume });
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult GetVideoSala(int id)
        {
            List<Sala> sali = new List<Sala>(_db.Sali.ToList());
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            Curs curs = _cursuri.Find(x => x.CursId == id);
            List<InscriereCursuri> inscrieri = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());

            List<SelectListItem> items = new List<SelectListItem>();
            var inscrieri_ = inscrieri.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .ToDictionary(x => x.Key, y => y.Count());
            if (inscrieri.Count() == 0)
            {
                var data = sali
               .Where(x => x.Libera == true && x.Echipament_Video == true)
               .Select(l => new { Value = l.Id.ToString(), Text = l.Nume });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                InscriereCursuri insc = inscrieri.Find(x => x.ClassId == curs.CursId);
                inscrieri_.TryGetValue(insc, out int number);

                var data = sali.Where(x => x.Capacitate >= number && x.Libera == true && x.Echipament_Video == true).Select(l => new { Value = l.Id.ToString(), Text = l.Nume });
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult GetVideoPCSala(int id)
        {
            List<Sala> sali = new List<Sala>(_db.Sali.ToList());
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            Curs curs = _cursuri.Find(x => x.CursId == id);
            List<InscriereCursuri> inscrieri = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());

            List<SelectListItem> items = new List<SelectListItem>();
            var inscrieri_ = inscrieri.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .ToDictionary(x => x.Key, y => y.Count());
            if (inscrieri.Count() == 0)
            {
                var data = sali
               .Where(x => x.Libera == true && x.Echipament_Video == true && x.PC == true)
               .Select(l => new { Value = l.Id.ToString(), Text = l.Nume });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                InscriereCursuri insc = inscrieri.Find(x => x.ClassId == curs.CursId);
                inscrieri_.TryGetValue(insc, out int number);

                var data = sali.Where(x => x.Capacitate >= number && x.Libera == true && x.Echipament_Video == true && 
                x.PC == true).Select(l => new { Value = l.Id.ToString(), Text = l.Nume });
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }


        dbSchoolsDataSet ds = new dbSchoolsDataSet();
       
   

        List<Events> calendarEv = new List<Events>();
        List<Events> calendarReport = new List<Events>();

        // GET: Event
        public ActionResult Index()
        {
            GetEvents();
            return View(calendarReport);

            //List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            //List<Profesor> profesori = new List<Profesor>(_db.Profesori.ToList());

            //List<Events> calendar = new List<Events>();
            //List<InscriereCursuri> inscriere = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());


            //calendarEv = _db.Events.ToList();
            //if (User.IsInRole("Profesor"))
            //{
            //    foreach (Profesor prof in profesori)
            //    {
            //        if (user.Id == prof.Guid)
            //            profesor = prof;

            //    }
            //    foreach (Events events in calendarEv)
            //    {
            //        if (events.ProfId == profesor.Id)
            //            calendar.Add(events);

            //    }
            //}
            //else if (User.IsInRole("Student"))
            //{
            //    foreach (Events events in calendarEv)
            //    {
            //        foreach (InscriereCursuri inscriereCursuri in inscriere)
            //        {
            //            if (student.Id == inscriereCursuri.StudId)
            //            {
            //                foreach (Curs curs_ in _cursuri)
            //                {
            //                    if (inscriereCursuri.ClassId == curs_.CursId)
            //                        cursuri.Add(curs_);

            //                }
            //            }
            //        }
            //        foreach (Curs curs in cursuri)
            //        {
            //            if (events.CursId == curs.CursId)
            //                calendar.Add(events);
            //        }

            //    }
            //}

            //var ApptListForDate = calendar;
            //calendarReport.AddRange(calendar);
            //var eventList = from e in ApptListForDate
            //                select new
            //                {
            //                    id = e.Id,
            //                    title = e.Description,

            //                    start = e.Start_time.ToString("s"),
            //                    end = e.End_time.ToString("s"),
            //                    curs = e.CursId,

            //                    allDay = false
            //                };
            //var rows = eventList.ToArray();
            //return View(calendarReport);
           
        }
        public JsonResult GetEvents()
        {
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            List<Profesor> profesori = new List<Profesor>(_db.Profesori.ToList());
            List<Student> students = new List<Student>(_db.Studenti.ToList());


            List<Events> calendar = new List<Events>();
            List<InscriereCursuri> inscriere = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());


            calendarEv = _db.Events.ToList();
            if (User.IsInRole("Profesor"))
            {
                foreach (Profesor prof in profesori)
                {
                    if (user.Id == prof.Guid)
                        profesor = prof;

                }
                foreach (Events events in calendarEv)
                {
                    if (events.ProfId == profesor.Id)
                        calendar.Add(events);

                }
            }
            else if (User.IsInRole("Student"))
            {
                foreach (Student stud in students)
                {
                    if (user.Id == stud.GUID)
                        student = stud;

                }
                foreach (Events events in calendarEv)
                {
                    foreach (InscriereCursuri inscriereCursuri in inscriere)
                    {
                        if (student.Id == inscriereCursuri.StudId)
                        {
                            foreach (Curs curs_ in _cursuri)
                            {
                                if (inscriereCursuri.ClassId == curs_.CursId)
                                    cursuri.Add(curs_);

                            }
                        }
                    }
                    foreach (Curs curs in cursuri.Distinct())
                    {
                        if (events.CursId == curs.CursId)
                            calendar.Add(events);
                    }

                }
            }

            var ApptListForDate = calendar;
            calendarReport.AddRange(calendar);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.Id,
                                title = e.Description,

                                start = e.Start_time.ToString("s"),
                                end = e.End_time.ToString("s"),
                                curs = e.CursId,

                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("Index");
            return report;
        }



        public ActionResult PrintView()
        {
            GetEvents();
            var report = new ViewAsPdf("Index",calendarReport);
                return report;
           

        }
        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Event/Create
        [Authorize(Roles = "Profesor")]

        public ActionResult Create()
        {
            Populate();
            List<Sala> sali = _db.Sali.ToList();

            DateTime date = DateTime.UtcNow;
            List<Events> events = _db.Events.ToList();
    
            List<Profesor> profesori = new List<Profesor>(_db.Profesori.ToList());
            List<Curs> _cursuri = new List<Curs>(_db.Cursuri.ToList());
            List<Student> students = new List<Student>(_db.Studenti.ToList());
            List<InscriereCursuri> inscriere = new List<InscriereCursuri>(_db.InscriereCursuri.ToList());

            if (User.IsInRole("Student"))
            {
                foreach (Student student_ in students)
                {
                    if (user.Id == student_.GUID)
                        student = student_;
                }
                foreach (Curs curs_ in _cursuri)
                {
                    foreach (InscriereCursuri inscriereCursuri in inscriere)
                    {
                        if (student.Id == inscriereCursuri.StudId && inscriereCursuri.ClassId == curs_.CursId)
                            cursuri.Add(curs_);
                    }

                }
            }
            if (User.IsInRole("Profesor"))
            {
                foreach (Profesor prof in profesori)
                {
                    if (user.Id == prof.Guid)
                        profesor = prof;

                }

                foreach (Curs curs_ in _cursuri)
                {
                    if (curs_.ProfId == profesor.Id)
                        cursuri.Add(curs_);
                }

            }
            cursuri = cursuri.Distinct().ToList();
            var items = from Curs curs in cursuri
                        select new SelectListItem { Value = curs.CursId.ToString(), Text = curs.Nume };

            ViewData["Classlist"] = items;

            var objEvent = new EventViewModel();
            objEvent.cursuri = new SelectList(items);
            objEvent.sali = new[] { new SelectListItem { Value = "", Text = "" } };
            return View(objEvent);

        }
 
        // POST: Event/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")]Events eventToCreate)
        {
            Populate();

            List<Sala> sali = _db.Sali.ToList();


            try
            {
                ev.retrieve(ev);
               
                if (eventToCreate != null)
                {
                    sali = _db.Sali.ToList();
                    
                    Sala  salaSelected = sali.FirstOrDefault(x => x.Id == eventToCreate.SalaId);
                    salaSelected.Libera = false;
                    _db.SaveChanges();

                }
                eventToCreate.ProfId = ev.ProfId;
                _db.Events.Add(eventToCreate);
                _db.SaveChanges();
               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Event/Edit/5
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

        // GET: Event/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Event/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
