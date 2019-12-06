using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebApplication6.Models;
using System.Globalization;

namespace WebApplication6.Controllers
{
    public class ProizvodController : Controller
    {

        [HttpGet]


        // GET: Proizvod
        public ActionResult Index()
        {
            DataTable dtblProizvod = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                //showing all data from Proizvod table
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Proizvod WHERE Deleted = 0", sqlCon);
                sqlDa.Fill(dtblProizvod);
            }
            
            return View(dtblProizvod);
        }


        [HttpGet]
        // GET: Proizvod/Create
        public ActionResult Create()
        {
            ViewBag.Country = populateCountries();
            return View(new ProizvodModel());
        }

        // POST: Proizvod/Create
        [HttpPost]
        public ActionResult Create(ProizvodModel proizvodModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();
                //check if there is any value in SerijskiBroj
                if (proizvodModel.SerijskiBroj == null)
                {
                    proizvodModel.SerijskiBroj = 0;
                }
                //check if there is any value in Opis
                if (proizvodModel.Opis == null)
                {
                    proizvodModel.Opis = "null";
                }
                if(proizvodModel.ImeProizvoda == null)
                {
                    ModelState.AddModelError("ImeProizvoda", "Unesite proizvod");
                    return View();
                }
                if (proizvodModel.Proizvodjac == null)
                {
                    ModelState.AddModelError("Proizvodjac", "Unesite ime proizvodjaca");
                    return View();
                }
                if (proizvodModel.ZemljaPorijekla == null)
                {
                    ModelState.AddModelError("ZemljaPorijekla", "Unesite zemlju porijekla");
                    return View();
                }
                //insert data into database
                string query = "INSERT INTO Proizvod(ImeProizvoda,Proizvodjac,SerijskiBroj,ZemljaPorijekla,Opis) VALUES(@ImeProizvoda,@Proizvodjac,@SerijskiBroj,@ZemljaPorijekla,@Opis)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ImeProizvoda", proizvodModel.ImeProizvoda);
                sqlCmd.Parameters.AddWithValue("@Proizvodjac", proizvodModel.Proizvodjac);
                sqlCmd.Parameters.AddWithValue("@SerijskiBroj", proizvodModel.SerijskiBroj);
                sqlCmd.Parameters.AddWithValue("@ZemljaPorijekla", proizvodModel.ZemljaPorijekla);
                sqlCmd.Parameters.AddWithValue("@Opis", proizvodModel.Opis);
                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }

        // GET: Proizvod/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Country = populateCountries();
            ProizvodModel proizvodModel = new ProizvodModel();
            DataTable dtblProizvod = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Proizvod WHERE ProizvodID = @ProizvodID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProizvodID", id);
                sqlDa.Fill(dtblProizvod);

            }
            if (dtblProizvod.Rows.Count == 1)
            {
                proizvodModel.ProizvodID = Convert.ToInt32(dtblProizvod.Rows[0][0].ToString());
                proizvodModel.ImeProizvoda = dtblProizvod.Rows[0][1].ToString();
                proizvodModel.Proizvodjac = dtblProizvod.Rows[0][2].ToString();
                proizvodModel.SerijskiBroj = Convert.ToInt32(dtblProizvod.Rows[0][3].ToString());
                proizvodModel.ZemljaPorijekla = dtblProizvod.Rows[0][4].ToString();
                proizvodModel.Opis = dtblProizvod.Rows[0][5].ToString();
                return View(proizvodModel);


            }
            else
                return RedirectToAction("Index");

        }

        // POST: Proizvod/Edit/5
        [HttpPost]
        public ActionResult Edit(ProizvodModel proizvodModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                //check if there is any value in SerijskiBroj
                if (proizvodModel.SerijskiBroj == null)
                {
                    proizvodModel.SerijskiBroj = 0;
                }
                //check if there is any value in Opis
                if (proizvodModel.Opis == null)
                {
                    proizvodModel.Opis = "Empty";
                }
                if (proizvodModel.ImeProizvoda == null)
                {
                    ModelState.AddModelError("ImeProizvoda", "Unesite proizvod");
                    return View();
                }
                if (proizvodModel.Proizvodjac == null)
                {
                    ModelState.AddModelError("Proizvodjac", "Unesite ime proizvodjaca");
                    return View();
                }
                if (proizvodModel.ZemljaPorijekla == null)
                {
                    ModelState.AddModelError("ZemljaPorijekla", "Unesite zemlju porijekla");
                    return View();
                }
                //updating new data into database
                sqlCon.Open();
                string query = "UPDATE Proizvod SET ImeProizvoda = @ImeProizvoda, Proizvodjac = @Proizvodjac, SerijskiBroj = @SerijskiBroj, ZemljaPorijekla = @ZemljaPorijekla, Opis = @Opis WHERE ProizvodID = @ProizvodID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProizvodID", proizvodModel.ProizvodID);
                sqlCmd.Parameters.AddWithValue("@ImeProizvoda", proizvodModel.ImeProizvoda);
                sqlCmd.Parameters.AddWithValue("@Proizvodjac", proizvodModel.Proizvodjac);
                sqlCmd.Parameters.AddWithValue("@SerijskiBroj", proizvodModel.SerijskiBroj);
                sqlCmd.Parameters.AddWithValue("@ZemljaPorijekla", proizvodModel.ZemljaPorijekla);
                sqlCmd.Parameters.AddWithValue("@Opis", proizvodModel.Opis);
                sqlCmd.ExecuteNonQuery();

            }

            return RedirectToAction("Index");
        }

        // GET: Proizvod/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                //delete from table based on ProizvodID
                sqlCon.Open();
                string query = "UPDATE Proizvod SET Deleted = 1 where ProizvodID = @ProizvodID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProizvodID", id);
                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }
        private static List<CountryModel> populateCountries()
        {
            List<CountryModel> countries = new List<CountryModel>();
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {

                string query = " SELECT * FROM Countries";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlCon;
                    sqlCon.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            countries.Add(new CountryModel
                            {
                                country = sdr["Country"].ToString(),
                                id = Convert.ToInt32(sdr["ID"])
                            });
                        }
                    }
                    sqlCon.Close();
                }
            }

            return countries;
        }
    }
    
}

