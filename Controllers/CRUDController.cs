using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRUDdbWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUDdbWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Students")]
    public class StudentsController : Controller
    {
        private string connectionString;
        public StudentsController()
        {
            connectionString = "Data Source=localhost;Initial Catalog=Student;Integrated Security=SSPI;";

        }
        [HttpGet]
        public ActionResult GetStudents()
        {
            List<StudentModel> Students = new List<StudentModel>();
            string query = "select * from StudentDetails";
            SqlCommand command = new SqlCommand(query);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                command.Connection = connection;
                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        StudentModel studentModelInstance = new StudentModel();
                        studentModelInstance.Id = dataReader["Id"].ToString();
                        studentModelInstance.Name = dataReader["Name"].ToString();
                        studentModelInstance.Class = dataReader["Class"].ToString();
                        studentModelInstance.RollNo = dataReader["RollNo"].ToString();
                        Students.Add(studentModelInstance);

                    }
                }
            }
            return Ok(Students);
        }

        [HttpPut]
        public ActionResult InsertStudent()
        {

            string query = "insert into StudentDetails Values(1, 'Srihari' , '12th' , '30')";
            SqlCommand command = new SqlCommand(query);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
            }

            return Ok("One Row Insterted");
        }

        [HttpGet("{Id}")]
        public ActionResult DeleteStudent(int Id)
        {
            string query = "delete from StudentDetails where Id="+Id;
            SqlCommand command = new SqlCommand(query);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
            }
            return Ok("Deleted");
        }

        [HttpPost]
        public ActionResult UpdateStudent([FromBody]StudentModel model)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("dbo.UpdateDetails", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Id", model.Id));
                command.Parameters.Add(new SqlParameter("@Name", model.Name));
                command.Parameters.Add(new SqlParameter("@Class", model.Class));
                command.ExecuteNonQuery();
            }
            return Ok("Updated");
        }
    }
}