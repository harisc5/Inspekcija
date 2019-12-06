using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication6.Controllers
{
    public class PregledKontrolaController : Controller
    {
        [HttpGet]
        // GET: PregledKontrola
        public ActionResult Index()
        {
            DataTable dtblRezultat = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                //showing results with the common column ImeProizvoda
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT Proizvod.ImeProizvoda,SerijskiBroj,ZemljaPorijekla,Datum,Rezultat FROM Proizvod,Kontrola where Proizvod.ImeProizvoda = Kontrola.ImeProizvoda and Proizvod.Deleted = 0  and Kontrola.Deleted = 0 and  Proizvod.Proizvodjac = Kontrola.Proizvodjac", sqlCon);
                sqlDa.Fill(dtblRezultat);
            }

            return View(dtblRezultat); //returning result
        }
    }
}
