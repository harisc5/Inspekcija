using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class InspekcijaModel
    {
        public int TijelaID { get; set; }

        [Display(Name ="Ime Tijela")]
        public string ImeTijela { get; set; }

        
        public string Inspektorat { get; set; }

        
        public string Nadleznost { get; set; }

        public string Kontakt { get; set; }
    }
   
    public enum Inspekt
    {
        FBIH,
        RS,
        BrckoDistrikt
    }

    public enum Nadlez
    {
      Trznisna_Inspekcija,
      Zdravstveno_Sanitarna
        
    }

}
