using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class ProizvodModel
    {
       
        public int ProizvodID { get; set; }
       
        [DisplayName("Ime Proizvoda")]
        public string ImeProizvoda { get; set; }
        
        public string Proizvodjac { get; set; }

        [DisplayName("Serijski broj")]
        public int? SerijskiBroj { get; set; }

        [DisplayName("Zemlja Porijekla")]
        public string ZemljaPorijekla { get; set; }
        

        public string Opis { get; set; }
    }
}