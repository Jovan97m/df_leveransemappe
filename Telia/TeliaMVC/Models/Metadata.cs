using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TeliaMVC.Models
{
    public class MetadataAdmin
    {
        public int Id { get; set; }
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "This field is Required")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is Required")]
        public string Password { get; set; }
        public string LoginErrorMsg { get; set; }
    }
    public class MetadataClient
    {
        public int Id { get; set; }
        [Display(Name = "OrgNummer")]
        [Required(ErrorMessage = "This field is Required")]
        public string Orgnummer { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is Required")]
        public string Password { get; set; }
    }

    public class MetadataFakturaoppsett
    {
        [StringLength(60)]
        public string NavnPaKostnadssted { get; set; }
        [StringLength(200)]
        public string Tileggsinfo_kostnadssted { get; set; }
        [Required(ErrorMessage = "Fakturaformat is needed")]
        [StringLength(30)]
        public string Fakturaformat { get; set; } // mora da postoji
        [Required(ErrorMessage = "Fakturaadresse is needed")]
        [StringLength(30)]
        public string Fakturaadresse { get; set; }
        public Nullable<int> Husnr { get; set; }
        public string Bokstav { get; set; }
        public Nullable<int> Postnummer { get; set; }
        public string Sted { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Epost { get; set; }
        //Primary Key
        [Required(ErrorMessage = "Kostnadssted is needed")]
        [StringLength(40)]
        public string Kostnadssted { get; set; }
    }
    public class MetadataNummer
    {
        [Required]
        [TelefonnummerCheck]
        public string Telefonnummer { get; set; }
        [Required]
        public string Abonnementstype { get; set; }
        [Required]
        [StringLength(25)]
        public string Fornavn { get; set; }
        [Required]
        [StringLength(35)]
        public string Etternavn { get; set; }
        [StringLength(35)]
        public string Bedrift_som_skal_faktureres { get; set; }
        public string c_o_adresse_for_SIM_levering { get; set; }
        public string Gateadresse_SIM_Skal_sendes_til { get; set; }
        public Nullable<int> Hus_nummer { get; set; }
        [StringLength(20)]
        public string Hus_bokstav { get; set; }
        public Nullable<int> post_nr_ { get; set; }
        public string Post_sted { get; set; }
        [Display(Name = "Epost - Sporings_informasjon")]
        [Required(ErrorMessage = "This email address is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Epost_for_sporings_informasjon { get; set; }
        [Display(Name = "Epost")]
        [Required(ErrorMessage = "This email address is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Epost { get; set; }
        [Required]
        [StringLength(30)]
        public string Kostnadsted { get; set; }
        public Nullable<int> Tilleggsinfo_ansatt_ID { get; set; }
        [DataSIM2]
        public Nullable<int> Ekstra_talesim_ { get; set; }
        [DataSIM5]
        public Nullable<int> Ekstra_datasim { get; set; }
        public int ID { get; set; }

    }
}