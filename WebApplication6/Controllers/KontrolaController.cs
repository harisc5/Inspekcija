using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class KontrolaController : Controller
    {
        
        [HttpGet]
        // GET: Kontrola
        public ActionResult Index()
        {
            DataTable dtblKontrola = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT KontrolaID,Datum,ImeTijela,ImeProizvoda,Rezultat,Sigurnost,Proizvodjac FROM Kontrola where Deleted = 0", sqlCon); //getting all data from database
                sqlDa.Fill(dtblKontrola); //fill the table with data
            }



            return View(dtblKontrola); //returning back with the filled table
        }

       [HttpGet]

        // GET: Kontrola/Create
        public ActionResult Create()
        {
            return View(new KontrolaModel());
        }

        // POST: Kontrola/Create
        [HttpPost]
        public ActionResult Create(KontrolaModel kontrolaModel)
        {
            
          
            using (SqlConnection sqlCon = new SqlConnection(
                  ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();
                //checking input date
                if (kontrolaModel.Datum > DateTime.Now)
                {
                    ModelState.AddModelError("Datum", "Unesite datum do današnjeg");
                    return View();
                }
                if (kontrolaModel.Datum < DateTime.MinValue)
                {
                    ModelState.AddModelError("Datum", "Unesite datum veći od minimalnog");
                    return View();
                }
                if (kontrolaModel.ImeProizvoda == null)
                {
                    ModelState.AddModelError("ImeProizvoda", "Unesite proizvod");
                    return View();
                }
                if(kontrolaModel.ImeTijela == null)
                {
                    ModelState.AddModelError("ImeTijela", "Unesite inspekcijsko tijelo");
                    return View();
                }
                if(kontrolaModel.Rezultat == null)
                {
                    ModelState.AddModelError("Rezultat", "Unesite rezultat");
                    return View();
                }
                //checking if the value of ImeProizvoda exists in the database
                string query2 = "SELECT count(*) FROM Proizvod where ImeProizvoda = @ImeProizvoda ";
                SqlCommand cmd2 = new SqlCommand(query2, sqlCon);
                cmd2.Parameters.AddWithValue("@ImeProizvoda", kontrolaModel.ImeProizvoda);
                int count = 0;
                count = (int)cmd2.ExecuteScalar();
                if(count == 0)
                {
                    ModelState.AddModelError("ImeProizvoda", "Unesite postojeći proizvod");
                    return View();
                }
               // checking if the value of ImeTijela exists in the database
                string query3 = "SELECT COUNT(*) FROM Tijela where ImeTijela = @ImeTijela";
                SqlCommand cmd3 = new SqlCommand(query3, sqlCon);
                cmd3.Parameters.AddWithValue("@ImeTijela", kontrolaModel.ImeTijela);
                int count2 = 0;
                count2 = (int)cmd3.ExecuteScalar();
                if(count2 == 0)
                {
                    ModelState.AddModelError("ImeTijela", "Unesite postojeće inspekcijsko tijelo");
                    return View();
                }

                string query4 = "SELECT COUNT(*) FROM Proizvod where ImeProizvoda=@ImeProizvoda and Proizvodjac = @Proizvodjac";
                SqlCommand cmd4 = new SqlCommand(query4, sqlCon);
                cmd4.Parameters.AddWithValue("@ImeProizvoda", kontrolaModel.ImeProizvoda);
                cmd4.Parameters.AddWithValue("@Proizvodjac", kontrolaModel.Proizvodjac);
                int count3 = 0;
                count3 = (int)cmd4.ExecuteScalar();
                if(count3 == 0)
                {
                    ModelState.AddModelError("ImeProizvoda", "Proizvod mora odgovarati proizvodjacu");
                    ModelState.AddModelError("Proizvodjac", "Proizvodjac mora odgovarati proizvodu");
                    return View();
                }

                //inserting data in the database
                string query = "INSERT INTO Kontrola(Datum,ImeTijela,ImeProizvoda,Rezultat,Sigurnost,Proizvodjac)" +
                    " VALUES(@Datum,@ImeTijela,@ImeProizvoda,@Rezultat,@Sigurnost,@Proizvodjac)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Datum", kontrolaModel.Datum);
                sqlCmd.Parameters.AddWithValue("@ImeTijela", kontrolaModel.ImeTijela);
                sqlCmd.Parameters.AddWithValue("@ImeProizvoda", kontrolaModel.ImeProizvoda);
                sqlCmd.Parameters.AddWithValue("@Rezultat", kontrolaModel.Rezultat);
                sqlCmd.Parameters.AddWithValue("@Sigurnost", kontrolaModel.Sigurnost);
                sqlCmd.Parameters.AddWithValue("@Proizvodjac", kontrolaModel.Proizvodjac);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Kontrola/Edit/5
        public ActionResult Edit(int id)
        {
            KontrolaModel kontrolaModel = new KontrolaModel();
            DataTable dtblKontrola = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Kontrola WHERE KontrolaID = @KontrolaID"; //editing the data based on KontrolaID
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@KontrolaID", id);
                sqlDa.Fill(dtblKontrola);

            }
            
            if (dtblKontrola.Rows.Count == 1)
            {
                kontrolaModel.KontrolaID = Convert.ToInt32(dtblKontrola.Rows[0][0].ToString());
                kontrolaModel.Datum =Convert.ToDateTime(dtblKontrola.Rows[0][1].ToString());
                kontrolaModel.ImeTijela = dtblKontrola.Rows[0][2].ToString();
                kontrolaModel.ImeProizvoda = dtblKontrola.Rows[0][3].ToString();
                kontrolaModel.Rezultat = dtblKontrola.Rows[0][4].ToString();
                kontrolaModel.Sigurnost = dtblKontrola.Rows[0][5].ToString();
                kontrolaModel.Proizvodjac = dtblKontrola.Rows[0][7].ToString();

                return View(kontrolaModel);

            }
            else
                return RedirectToAction("Index");
        }

        // POST: Kontrola/Edit/5
        [HttpPost]
        public ActionResult Edit(KontrolaModel kontrolaModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();
                if (kontrolaModel.Datum > DateTime.Now)
                {
                    ModelState.AddModelError("Datum", "Unesite datum do današnjeg");
                    return View();
                }
                if (kontrolaModel.Datum > DateTime.Now)
                {
                    ModelState.AddModelError("Datum", "Unesite datum do današnjeg");
                    return View();
                }
                if (kontrolaModel.ImeProizvoda == null)
                {
                    ModelState.AddModelError("ImeProizvoda", "Unesite proizvod");
                    return View();
                }
                if (kontrolaModel.ImeTijela == null)
                {
                    ModelState.AddModelError("ImeTijela", "Unesite inspekcijsko tijelo");
                    return View();
                }
                if (kontrolaModel.Rezultat == null)
                {
                    ModelState.AddModelError("Rezultat", "Unesite rezultat");
                    return View();
                }
                //checking if the value of ImeProizvoda exists in the database
                string query2 = "SELECT count(*) FROM Proizvod where ImeProizvoda = @ImeProizvoda ";
                SqlCommand cmd2 = new SqlCommand(query2, sqlCon);
                cmd2.Parameters.AddWithValue("@ImeProizvoda", kontrolaModel.ImeProizvoda);
                int count = 0;
                count = (int)cmd2.ExecuteScalar();
                if (count == 0)
                {
                    ModelState.AddModelError("ImeProizvoda", "Unesite postojeći proizvod");
                    return View();
                }
                // checking if the value of ImeTijela exists in the database
                string query3 = "SELECT COUNT(*) FROM Tijela where ImeTijela = @ImeTijela";
                SqlCommand cmd3 = new SqlCommand(query3, sqlCon);
                cmd3.Parameters.AddWithValue("@ImeTijela", kontrolaModel.ImeTijela);
                int count2 = 0;
                count2 = (int)cmd3.ExecuteScalar();
                if (count2 == 0)
                {
                    ModelState.AddModelError("ImeTijela", "Unesite postojeće inspekcijsko tijelo");
                    return View();
                }

                string query4 = "SELECT COUNT(*) FROM Proizvod where ImeProizvoda = @ImeProizvoda and Proizvodjac = @Proizvodjac and Deleted = 0";
                SqlCommand cmd4 = new SqlCommand(query4, sqlCon);
                cmd4.Parameters.AddWithValue("@ImeProizvoda", kontrolaModel.ImeProizvoda);
                cmd4.Parameters.AddWithValue("@Proizvodjac", kontrolaModel.Proizvodjac);
                int count3 = 0;
                count3 = (int)cmd4.ExecuteScalar();
                if (count3 == 0)
                {
                    ModelState.AddModelError("ImeProizvoda", "Proizvod mora odgovarati proizvodjacu");
                    ModelState.AddModelError("Proizvodjac", "Proizvodjac mora odgovarati proizvodu");
                    return View();
                }
                //filling the new data into the database
                string query = "UPDATE Kontrola SET Datum = @Datum, ImeTijela = @ImeTijela, ImeProizvoda = @ImeProizvoda, Rezultat = @Rezultat, Sigurnost = @Sigurnost, Proizvodjac = @Proizvodjac WHERE KontrolaID = @KontrolaID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KontrolaID", kontrolaModel.KontrolaID);
                sqlCmd.Parameters.AddWithValue("@Datum", kontrolaModel.Datum);
                sqlCmd.Parameters.AddWithValue("@ImeTijela", kontrolaModel.ImeTijela);
                sqlCmd.Parameters.AddWithValue("@ImeProizvoda", kontrolaModel.ImeProizvoda);
                sqlCmd.Parameters.AddWithValue("@Rezultat", kontrolaModel.Rezultat);
                sqlCmd.Parameters.AddWithValue("@Sigurnost", kontrolaModel.Sigurnost);
                sqlCmd.Parameters.AddWithValue("@Proizvodjac", kontrolaModel.Proizvodjac);
                sqlCmd.ExecuteNonQuery();

            }

            return RedirectToAction("Index");
        }


        // GET: Kontrola/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(
               ConfigurationManager.ConnectionStrings["TestCS"].ConnectionString))
            {
                sqlCon.Open();
                string query = " UPDATE Kontrola SET Deleted = 1 where KontrolaID = @KontrolaID"; //delete data based on KontrolaID
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KontrolaID", id);
                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }
   

       
    }
}

