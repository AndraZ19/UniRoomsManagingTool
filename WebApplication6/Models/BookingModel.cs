using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication6.Models;

namespace WebApplication6.Models
{
    public class BookingModel
    {
        Profesor p = new Profesor();
        public int Id { get; set; }
        public string[] Cursuri => new string[] { p.Discipline };
    }

   /* public BookingModel CreateBooking(ApplicationUser a )
    {


    } */
}