using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.Controllers;
using System.Data;

namespace WebApplication6.Controllers
{
    public class PregledPoDatumuController : Controller
    {
        
       [HttpGet]
        // GET: PregledPoDatumu
        public ActionResult Index()
        {
            DataTable dtblRezultat = new DataTable();
            PregledPoDatumu pregledPoDatumu = new PregledPoDatumu();
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                //filling the table with all the data from database
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Kontrola where Deleted = 0 ORDER BY Datum", sqlCon);
                sqlDa.Fill(dtblRezultat);
               
            }

            return View(dtblRezultat);
        }

      
        [HttpGet]

        // GET: PregledPoDatumu/Create
        public ActionResult Create()
        {
            return View(new PregledPoDatumu());
        }

        // POST: PregledPoDatumu/Create
        [HttpPost]
        public ActionResult Create(PregledPoDatumu pregledPoDatumu)
        {
            
            DataTable dtblRezultat = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(
                ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                //check if startdate is bigger than enddate
                sqlCon.Open();
                if (pregledPoDatumu.Datum1 > pregledPoDatumu.Datum2)
                {
                    ModelState.AddModelError("Datum1", "start date must be before end date");
                    return View();
                }
                //check if first date is bigger than today's
                if(pregledPoDatumu.Datum1 > DateTime.Now)
                {
                    ModelState.AddModelError("Datum1", "Unesite datum prije današnjeg");
                    return View();
                }
                //check if enddate is bigger than today's
                if(pregledPoDatumu.Datum2 > DateTime.Now)
                {
                    ModelState.AddModelError("Datum2", "Unesite datum do današnjeg");
                    return View();
                }
                //check if there is any date entered
                if(pregledPoDatumu.Datum1 == DateTime.MinValue){
                    ModelState.AddModelError("Datum1", "Unesite datum ");
                    return View();
                }
                //check if any date is entered
                if (pregledPoDatumu.Datum2 == DateTime.MinValue)
                {
                    ModelState.AddModelError("Datum2", "Unesite datum ");
                    return View();
                }
                //create new query with startdate and enddate             
                string query = "SELECT * FROM Kontrola where Datum > @Datum1 and Datum < @Datum2";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Datum1", pregledPoDatumu.Datum1);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Datum2", pregledPoDatumu.Datum2);
                sqlDa.Fill(dtblRezultat);
                
                
            }
            return View("Index", dtblRezultat); //returning to index page with result
        }
    }
}
