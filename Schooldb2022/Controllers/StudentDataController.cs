using MySql.Data.MySqlClient;
using Schooldb2022.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Schooldb2022.Controllers
{
    public class StudentDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchooldbContext sdb = new SchooldbContext();

        /// <summary>
        /// Returns the list of students in the system 
        /// </summary>
        ///<param name="SearchKey">User search</param>
        /// <example>
        /// GET api/StudentData/listStudent  
        /// </example>
        /// <returns>The Names of Students</returns>
        [HttpGet]
        [Route("api/StudentData/ListStudent/{searchkey?}")]
        public IEnumerable<Students> ListStudent(string Searchkey = null)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select * from students where studentfname like @key or studentlname like @key or concat(studentfname,' ',studentlname) like @key";
            cmd.Parameters.AddWithValue("@key", "%" + Searchkey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a class object
            MySqlDataReader Resultset = cmd.ExecuteReader();
            List<Students> studentlist = new List<Students>();
            while (Resultset.Read())
            {
                int StudentId = Convert.ToInt32(Resultset["studentid"]);
                string StudentFname = Resultset["studentfname"].ToString();
                string StudentLname = Resultset["studentlname"].ToString();
                string StudentNumber = Resultset["studentnumber"].ToString();
                string Enroldate = Resultset["enroldate"].ToString();

                Students newstudent = new Students();
                newstudent.StudentId = StudentId;
                newstudent.StudentFname = StudentFname;
                newstudent.StudentLname = StudentLname;
                newstudent.StudentNumber = StudentNumber;
                newstudent.Enroldate = Enroldate;

                studentlist.Add(newstudent);

            }
            //close connection
            conn.Close();
            return studentlist;
        }


        /// <summary>
        /// Returns an individual student from the database by specifying the primary key studentid
        /// </summary>
        /// <param name="id">the student ID in the database</param>
        /// <returns>student object</returns>
        [HttpGet]
        public Students FindStudent(int id)
        {
            Students newstudent = new Students();

            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select  * from students where studentid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a class object
            MySqlDataReader Resultset = cmd.ExecuteReader();
            while (Resultset.Read())
            {
                newstudent.StudentId = Convert.ToInt32(Resultset["studentid"]);
                newstudent.StudentFname = Resultset["studentfname"].ToString();
                newstudent.StudentLname = Resultset["studentlname"].ToString();
                newstudent.StudentNumber = Resultset["studentnumber"].ToString();
                newstudent.Enroldate = Resultset["enroldate"].ToString();
            }

            //close connection
            conn.Close();


            //Create an instance of a connection
            MySqlConnection con = sdb.AccessDatabase();

            //Open the connection between the web server and database
            con.Open();

            //Establish a new command (query) for our database
            MySqlCommand cm = con.CreateCommand();
            cm.CommandText = "SELECT classes.classname FROM students LEFT OUTER join studentsxclasses on studentsxclasses.studentid=students.studentid join classes on classes.classid=studentsxclasses.classid where students.studentid=@studentid";
            cm.Parameters.AddWithValue("@studentid", id);
            cm.Prepare();
            List<string> list = new List<string>();

            //Gather Result Set of Query into a class object
            Resultset = cm.ExecuteReader();


            while (Resultset.Read())
            {

                //if their is courses data
                if (Resultset["classname"] != null)
                {
                    string courses = Resultset["classname"].ToString();
                    list.Add(courses);
                }
                //if their is no courses data
                else
                {
                    string msg = "No data";
                    list.Add(msg);
                }

            }

            newstudent.Courses = list;

            //close connection
            con.Close();

            return newstudent;
        }

        /// <summary>
        /// takes the student id and deleted that student from the database
        /// </summary>
        /// <param name="id">Student id</param>
        /// <example>POST : /api/StudentData/DeleteStudent/7</example>
        [HttpPost]
        public void DeleteStudent(int id)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Delete from students where studentid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            conn.Close();

        }

        /// <summary>
        /// Adds an Student to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewStudent">An object with fields that map to the columns of the Student's table. </param>
        /// <example>
        /// POST api/StudentData/AddStudent
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"StudentFname":"Christine",
        ///	"StudentLname":"Bittle",
        ///	"StudentNumber":"S565",
        ///	"entroldate":"2018-09-09"
        /// }
        /// </example>
        [HttpPost]
        public void AddStudent([FromBody] Students newstudent)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Insert into students (studentfname, studentlname, studentnumber, enroldate) values (@studentfname, @studentlname, @studentnumber, @enroldate)";
            cmd.Parameters.AddWithValue("@studentfname", newstudent.StudentFname);
            cmd.Parameters.AddWithValue("@studentlname", newstudent.StudentLname);
            cmd.Parameters.AddWithValue("@studentnumber", newstudent.StudentNumber);
            cmd.Parameters.AddWithValue("@enroldate", newstudent.Enroldate);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            conn.Close();

        }
    }


}

