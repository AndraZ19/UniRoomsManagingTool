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
    
    public partial class Curs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Curs()
        {
            this.InscriereCursuri = new HashSet<InscriereCursuri>();
            this.Events = new HashSet<Events>();
        }
    
        public string Nume { get; set; }
        public Nullable<int> Capacitate { get; set; }
        public Nullable<bool> PCReq { get; set; }
        public Nullable<bool> VideoReq { get; set; }
        public int CursId { get; set; }
        public Nullable<int> ProfId { get; set; }
    
        public virtual Profesor Profesori { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InscriereCursuri> InscriereCursuri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Events> Events { get; set; }
    }
}
