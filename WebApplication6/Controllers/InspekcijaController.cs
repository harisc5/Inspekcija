using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class InspekcijaController : Controller
    {
        [HttpGet]


        // GET: Proizvod
        public ActionResult Index()
        {

            DataTable dtblInspekcija = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Tijela", sqlCon); //printing all data from database
                sqlDa.Fill(dtblInspekcija); //fill the table with data
            }



            return View(dtblInspekcija);
        }


        [HttpGet]
        // GET: Proizvod/Create
        public ActionResult Create()
        {
            return View(new InspekcijaModel());

        }

        // POST: Proizvod/Create
        [HttpPost]
        public ActionResult Create(InspekcijaModel inspekcijaModel)
        {
            //create connection with database
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();

                string query3 = "SELECT COUNT(*) FROM Tijela where ImeTijela = @ImeTijela";
                SqlCommand cmd3 = new SqlCommand(query3, sqlCon);
                cmd3.Parameters.AddWithValue("@ImeTijela", inspekcijaModel.ImeTijela);
                int count2 = 0;
                count2 = (int)cmd3.ExecuteScalar();

                if(count2 != 0)
                {
                    ModelState.AddModelError("ImeTijela", "Inspekcijsko tijelo već postoji");
                    return View();
                }
                if (inspekcijaModel.ImeTijela == null)
                {
                    ModelState.AddModelError("ImeTijela", "Unesite inspekcijsko tijelo");
                    return View();
                }
                if(inspekcijaModel.Kontakt == null)
                {
                    ModelState.AddModelError("Kontakt", "Unesite kontakt osobu");
                    return View();
                }


                //creating query and adding parameters
                string query = "INSERT INTO Tijela(ImeTijela,Inspektorat,Nadleznost,Kontakt)" +
                    " VALUES(@ImeTijela,@Inspektorat,@Nadleznost,@Kontakt)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ImeTijela", inspekcijaModel.ImeTijela);
                sqlCmd.Parameters.AddWithValue("@Inspektorat", inspekcijaModel.Inspektorat);
                sqlCmd.Parameters.AddWithValue("@Nadleznost", inspekcijaModel.Nadleznost);
                sqlCmd.Parameters.AddWithValue("@Kontakt", inspekcijaModel.Kontakt);
                sqlCmd.ExecuteNonQuery(); //execution of query
            }
            return RedirectToAction("Index"); //redirecting back to index page
        }
    }
}
