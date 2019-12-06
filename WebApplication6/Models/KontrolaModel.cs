using CustomValidationAttributeDemo.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApplication6.Models
{
    public class KontrolaModel
    {
        public int KontrolaID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Display(Name = "Ime Tijela")]
        public string ImeTijela { get; set; }

        [Display(Name = "Ime Proizvoda")]

        public string ImeProizvoda { get; set; }

        public string Rezultat { get; set; }

     
        public string Sigurnost { get; set; }
        public string Proizvodjac { get; set; }

    }

    public enum Sigur
    {
        True,
        False
    }
    
}