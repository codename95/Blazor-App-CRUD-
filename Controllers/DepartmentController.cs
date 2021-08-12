using BlazorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT DepartmentID, DepartmentName FROM dbo.Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            try
            {
                string query = @"INSERT INTO dbo.Department (DepartmentName) VALUES (@DepartmentName)";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult("success");
            }
            catch (Exception err)
            {
                return new JsonResult("error " + err.Message);
            }

        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            try
            {
                string query = @"UPDATE dbo.Department SET DepartmentName = @DepartmentName WHERE DepartmentId  = @DepartmentId";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                        myCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult("success");
            }
            catch (Exception err)
            {
                return new JsonResult("error " + err.Message);
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM dbo.Department  WHERE DepartmentId  = @DepartmentId";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@DepartmentId", id);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult("success");
            }
            catch (Exception err)
            {
                return new JsonResult("error " + err.Message);
            }

        }
    }
}
