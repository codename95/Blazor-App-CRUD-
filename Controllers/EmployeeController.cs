using BlazorApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT EmployeeID, EmployeeName, Department,
                            Convert(varchar(10), DateofJoining, 120) as DateofJoining,
                            PhotoFileName FROM dbo.Employee";
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
        public JsonResult Post(Employee employee)
        {
            try
            {
                string query = @"INSERT INTO dbo.Employee (EmployeeName, Department, DateOfJoining, PhotoFileName)
                            VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                        myCommand.Parameters.AddWithValue("@Department", employee.Department);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                        myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);

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
        public JsonResult Put(Employee employee)
        {
            try
            {
                string query = @"UPDATE dbo.Employee SET EmployeeName = @EmployeeName, Department = @Department,
                                                DateOfJoining = @DateOfJoining, PhotoFileName = @PhotoFileName
                                    WHERE EmployeeId  = @EmployeeId";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                       
                        myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                        myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                        myCommand.Parameters.AddWithValue("@Department", employee.Department);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                        myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);

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
                string query = @"DELETE FROM dbo.Employee  WHERE EmployeeId  = @EmployeeId";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeId", id);

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


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;

                var postedFile = httpRequest.Files[0];

                var fileName = postedFile.FileName;

                var physicalPath = _webHostEnvironment.ContentRootPath + "/Photos/" + fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}
