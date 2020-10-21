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
    public class PrezentaViewModel
    {
        public int Id { get; set; }
        public bool Prezenta { get; set; }
        public int StudentId { get; set; }
        public int EventId { get; set; }

        public virtual Events Events { get; set; }
        public virtual Student Studenti { get; set; }

        public IEnumerable<SelectListItem> cursuri { get; set; }
        public string SelectedCurs { get; set; }
        public PrezentaViewModel retrieve(PrezentaViewModel e)
        {
            dbSchoolsEntities db = new dbSchoolsEntities();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            e.StudentId = db.Studenti.ToList().Find(x => x.GUID == user.Id).Id;

            return e;
        }


    }
}