//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeliaMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Fakturaoppsett
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fakturaoppsett()
        {
            this.Nummers = new HashSet<Nummer>();
        }
    
        public string NavnPaKostnadssted { get; set; }
        public string Tileggsinfo_kostnadssted { get; set; }
        public string Fakturaformat { get; set; }
        public string Fakturaadresse { get; set; }
        public Nullable<int> Husnr { get; set; }
        public string Bokstav { get; set; }
        public Nullable<int> Postnummer { get; set; }
        public string Sted { get; set; }
        public string Epost { get; set; }
        public string Kostnadssted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Nummer> Nummers { get; set; }
    }
}
