using MySql.Data.MySqlClient;
using Schooldb2022.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Schooldb2022.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchooldbContext sdb = new SchooldbContext();


        /// <summary>
        /// Returns the list of teachers in the system 
        /// </summary>
        /// <param name="SearchKey">User search</param>
        /// <example>
        /// GET api/TeacherData/listTeacher 
        /// </example>
        /// <returns>The Names of Teacher</returns>
        [HttpGet]
        [Route("api/TeacherData/listTeacher/{SearchKey?}")]

         
        public IEnumerable<Teacher> listTeacher(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd =conn.CreateCommand(); 
            string query = "Select * from teachers where teacherfname like @key or teacherlname like @key or concat(teacherfname,'',teacherlname) like @key";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@key", "%"+SearchKey+"%");
            cmd.Prepare();

            //Gather Result Set of Query into a class object
            MySqlDataReader Resultset = cmd.ExecuteReader();

            List<Teacher> teacherlist = new List<Teacher> { };

            while (Resultset.Read())
            {
                int TeacherId = Convert.ToInt32(Resultset["teacherid"]);
                string TeacherFname = Resultset["teacherfname"].ToString();
                string TeacherLname = Resultset["teacherlname"].ToString();
                string EmployeeNumber = Resultset["employeenumber"].ToString();
                string Hiredate = Resultset["hiredate"].ToString();
                decimal Salary =Convert.ToDecimal( Resultset["salary"]);

                Teacher newteacher = new Teacher();
                newteacher.TeacherId = TeacherId;   
                newteacher.TeacherFname = TeacherFname; 
                newteacher.TeacherLname = TeacherLname; 
                newteacher.EmployeeNumber = EmployeeNumber; 
                newteacher.Hiredate = Hiredate;
                newteacher.Salary = Salary;
                    


                teacherlist.Add(newteacher);
            }

            //close connection
            conn.Close();
            return teacherlist;


        }


        /// <summary>
        /// Returns an individual teacher from the database by specifying the primary key teacherid
        /// </summary>
        /// <param name="id">the teacher's ID in the database</param>
        /// <returns>teacher object</returns>
       
        [HttpGet]
     
        public  Teacher FindTeacher(int id)
        {
            Teacher newteacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT teachers.* , classes.classname FROM teachers LEFT OUTER join classes on classes.teacherid=teachers.teacherid where teachers.teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a class object
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {

                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                string Hiredate = ResultSet["hiredate"].ToString();
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);
                string Course = ResultSet["classname"].ToString();

                newteacher.TeacherId = TeacherId;
                newteacher.TeacherFname = TeacherFname;
                newteacher.TeacherLname = TeacherLname;
                newteacher.EmployeeNumber = EmployeeNumber;
                newteacher.Hiredate = Hiredate;
                newteacher.Salary = Salary;
                newteacher.Course= Course;  

            }
           
            return newteacher;

        }
        /// <summary>
        /// takes the teacher id and deleted that teacher from the database
        /// </summary>
        /// <param name="id">Teacher id</param>
        /// <example>POST : /api/TeacherData/DeleteTeacher/7</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            
            cmd.ExecuteNonQuery();
            conn.Close();

        }
        /// <summary>
        /// Adds an Teachers to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teacher's table. </param>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"EmpoloyeeNumber":"T565",
        ///	"Hiredate":"2018-09-09"
        ///	 "Salary":"50.6"
        /// }
        /// </example>
        [HttpPost]
        public  void AddTeacher([FromBody]Teacher newteacher)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";
            cmd.Parameters.AddWithValue("@teacherfname", newteacher.TeacherFname);
            cmd.Parameters.AddWithValue("@teacherlname", newteacher.TeacherLname);
            cmd.Parameters.AddWithValue("@employeenumber", newteacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@hiredate", newteacher.Hiredate); 
            cmd.Parameters.AddWithValue("@salary", newteacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            conn.Close();

        }

        /// <summary>
        /// Updates a=a Teacher information in the system
        /// <param name="TeacherId">The id of the teacher in the system</param>
        /// <param name="UpdatedTeacher">post content, teacher information</param>
        /// </summary>
        /// <example>
        /// api/teacherdate/updateteacher/6
        /// POST: POST CONTENT/ FORM BODY/ REQUEST BODY
        /// {"TeacherFname":"Julia","TeacherLname":"Rogers"}
        /// </example>

        [HttpPost]
        public void UpdateTeacher([FromBody]int TeacherId, Teacher UpdatedTeacher)
        {
            Debug.WriteLine("updating teacher" + TeacherId);
            Debug.WriteLine("POST CONTENT");
            Debug.WriteLine(UpdatedTeacher.TeacherFname);

            string query = "update teachers set teacherfname=@fname, teacherlname=@lname, employeenumber=@number, hiredate=@date, salary=@salary where TeacherId=@id";
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", TeacherId);
            cmd.Parameters.AddWithValue("@fname", UpdatedTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@lname", UpdatedTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@number", UpdatedTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@date", UpdatedTeacher.Hiredate);
            cmd.Parameters.AddWithValue("@salary", UpdatedTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            conn.Close();


        }
    }
}
