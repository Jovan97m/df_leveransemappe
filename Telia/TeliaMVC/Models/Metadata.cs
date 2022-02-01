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
        [Required(ErrorMessage = "Dette feltet er obligatorisk!")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Dette feltet er obligatorisk!")]
        public string Password { get; set; }
        public string LoginErrorMsg { get; set; }
    }
    public class MetadataClient
    {
        public int Id { get; set; }
        [Display(Name = "Orgnummer")]
        [Required(ErrorMessage = "Dette feltet er obligatorisk!")]
        [Orgnummer]
        public string Orgnummer { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Dette feltet er obligatorisk!")]
        public string Password { get; set; }
        public string FirmaNavn { get; set; }
        public string GateNavn { get; set; }
        public Nullable<int> HusNummer { get; set; }
        public string HusBokStav { get; set; }
        public Nullable<int> PostNummer { get; set; }
        public string Sted { get; set; }
        [EmailAddress(ErrorMessage = "Ugyldig e-postadresse")]
        public string Epost { get; set; }
        public string KontaktNavn { get; set; }
        public string KontaktEpost { get; set; }
        public string KontaktTlfnr { get; set; }
        public string TekniskKontaktNavn { get; set; }
        public string TekniskKontaktEpost { get; set; }
        public string TekniskKontaktTlfnr { get; set; }

        [Display(Name = "Mobile Abonementype")]
        public int Id_abonementype { get; set; }

        [Display(Name = "Fixed Abonementype")]
        public int Id_abonemetypeF { get; set; }
        [Display(Name = "Internet Abonementype")]
        public int Id_abonementypeI { get; set; }
    }

    public class MetadataFakturaoppsett
    {
        [StringLength(60)]
        public string NavnPaKostnadssted { get; set; }
        [StringLength(200)]
        public string Tileggsinfo_kostnadssted { get; set; }
        [Required(ErrorMessage = "Dette feltet er obligatorisk!")]
        [StringLength(30)]
        public string Fakturaformat { get; set; } // mora da postoji
        [Required(ErrorMessage = "Dette feltet er obligatorisk!")]
        [StringLength(30)]
        public string Fakturaadresse { get; set; }
        public Nullable<int> Husnr { get; set; }
        public string Bokstav { get; set; }
        public Nullable<int> Postnummer { get; set; }
        public string Sted { get; set; }

        [Display(Name = "Epost")]
        [Required(ErrorMessage = " E-postadresse er nødvendig")]
        [EmailAddress(ErrorMessage = "Ugyldig e-postadresse")]
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
        [Display(Name = "Avtale")]
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
        [Range(1, 9999)]
        public Nullable<int> post_nr_ { get; set; }
        [Display(Name = "Post-sted")]
        public string Post_sted { get; set; }
        [Display(Name = "Epost - Sporings_informasjon")]
        [EmailAddress(ErrorMessage = "Ugyldig e-postadresse")]
        public string Epost_for_sporings_informasjon { get; set; }
        [Display(Name = "Epost")]
        [EmailAddress(ErrorMessage = "Ugyldig e-postadresse")]
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
        public string Orgnummer { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Pending { get; set; }
        public string Katalogoppforing { get; set; }
        public Nullable<System.DateTime> Porteringsdatoog_tid { get; set; }
        public string Binding { get; set; }
        public Nullable<int> Postnummer { get; set; }
        public Nullable<int> Antall_TrillingSIM { get; set; }
        public Nullable<int> allDataSIM { get; set; }
        public string Manuell_Top_up { get; set; }
        public string Sperre_Top_up { get; set; }
        public string Norden { get; set; }
        public Nullable<bool> Tale_og_SMS_til_EU { get; set; }
        public string TBN { get; set; }
        [Required]
        public Nullable<int> HovedSIM { get; set; }
        public Nullable<int> TrillingSIM1 { get; set; }
        public Nullable<int> TrillingSIM2 { get; set; }
        public Nullable<int> DataSIM1 { get; set; }
        public Nullable<int> DataSIM2 { get; set; }
        public Nullable<int> DataSIM3 { get; set; }
        public Nullable<int> DataSIM4 { get; set; }
        public Nullable<int> DataSIM5 { get; set; }
        public string DeliveryMethodCode { get; set; }
        public string DeliveryStreetName { get; set; }
        public string DeliveryStreetNumber { get; set; }
        public string DeliveryStreetSuffix { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryZIP { get; set; }
        public string DeliveryCountryCode { get; set; }

        [EmailAddress(ErrorMessage = "Ugyldig e-postadresse")]
        public string DeliveryContractEmail { get; set; }
        public string DeliveryContractCountryCode { get; set; }
        public string DeliveryContractLocalNumber { get; set; }
        [StringLength(20)]
        public string DeliveryIndividualFirstName { get; set; }
        [StringLength(20)]
        public string DeliveryIndividualLastName { get; set; }

    }
}