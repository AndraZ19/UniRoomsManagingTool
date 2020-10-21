using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.ViewModels
{
    
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public System.DateTime Start_time { get; set; }
        public System.DateTime End_time { get; set; }
        public int CursId { get; set; }
        public int ProfId { get; set; }
        public int SalaId { get; set; }

        public virtual Curs Cursuri { get; set; }
        public virtual Profesor Profesori { get; set; }
        public virtual Sala Sali { get; set; }
        public IEnumerable<SelectListItem> cursuri { get; set; }
        public string SelectedCurs { get; set; }
        public IEnumerable<SelectListItem> sali { get; set; }
        public string SelectedSala { get; set; }
        public bool VideoReq { get; set; }
        public bool PcReq { get; set; }

        public EventViewModel retrieve(EventViewModel e)
        {
            dbSchoolsEntities db = new dbSchoolsEntities();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            e.ProfId = db.Profesori.ToList().Find(x => x.Guid == user.Id).Id;

            return e;
        }



    }
    
}