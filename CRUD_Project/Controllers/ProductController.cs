using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using CRUD_Project.Models;

namespace CRUD_Project.Controllers
{
    public class ProductController : Controller
    {
        //1. Declare Connection String
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\geetg\OneDrive\Documents\MvcCrudDB.mdf;Integrated Security=True;Connect Timeout=30";



        // GET: Product
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon= new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Products",sqlCon);
                sqlDa.Fill(dtblProduct);    
            }
            return View(dtblProduct);
        }

       
        // GET: Product/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductModel());
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel productModel)
        {
        using(SqlConnection sqlCon= new SqlConnection(connectionString))
            {
                //For inserting into database....................
                sqlCon.Open();
                string query = "INSERT INTO Products VALUES( @ProductName , @Price , @Count)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
              
                sqlCmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCmd.Parameters.AddWithValue("@Count",productModel.Count);
                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }





        [HttpGet]
        // GET: Product/Edit/Pen
        public ActionResult Edit(int id)
        {
            ProductModel productModel = new ProductModel();
            DataTable dtblProduct = new DataTable();
          
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "SELECT * From Products Where Id = @Id";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                    sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                    sqlDa.Fill(dtblProduct);
                }
         
            if (dtblProduct.Rows.Count == 1)
            {
                productModel.Id = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productModel.ProductName = dtblProduct.Rows[0][1].ToString();
                productModel.Price = Convert.ToDecimal(dtblProduct.Rows[0][2].ToString());
                productModel.Count = Convert.ToInt32(dtblProduct.Rows[0][3].ToString());
            
                return View(productModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //.....................................Create and Edit are over Now update the changes made

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,ProductModel productModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                //For inserting into database....................
                sqlCon.Open();
                string query = "UPDATE Products SET ProductName= @ProductName, Price= @Price, Count= @Count Where Id = @Id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                //  sqlCmd.Parameters.AddWithValue("@Id", productModel.Id);
                sqlCmd.Parameters.AddWithValue("@Id", id);
                sqlCmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCmd.Parameters.AddWithValue("@Count", productModel.Count);
                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");


        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                //For inserting into database....................
                sqlCon.Open();
                string query = "DELETE From Products Where Id = @Id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id",id);
                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
           
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
