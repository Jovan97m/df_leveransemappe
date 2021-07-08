using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TeliaMVC.Models
{
    [MetadataType(typeof(MetadataAdmin))]
    public partial class Admin
    {

    }

    [MetadataType(typeof(MetadataFakturaoppsett))]
    public partial class Fakturaoppsett
    {
    }

    [MetadataType(typeof(MetadataNummer))]
    public partial class Nummer
    {
    }
}