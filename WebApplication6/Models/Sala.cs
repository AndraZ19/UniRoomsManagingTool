//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication6.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sala
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sala()
        {
            this.Events = new HashSet<Events>();
        }
    
        public int Id { get; set; }
        public string Nume { get; set; }
        public int Etaj { get; set; }
        public string Cladire { get; set; }
        public bool Echipament_Video { get; set; }
        public bool PC { get; set; }
        public int Capacitate { get; set; }
        public bool Libera { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Events> Events { get; set; }
    }
}
