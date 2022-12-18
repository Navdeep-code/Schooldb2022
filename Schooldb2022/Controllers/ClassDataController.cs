using MySql.Data.MySqlClient;
using Schooldb2022.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Schooldb2022.Controllers
{
    public class ClassDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchooldbContext sdb = new SchooldbContext();

        /// <summary>
        /// Returns the list of classes in the system 
        /// </summary>
        /// <param name="SearchKey">User search</param>
        /// <example>
        /// GET api/ClassData/listclass  
        /// </example>
        /// <returns>The Names of classes and their classcode</returns>
        [HttpGet]
        [Route("api/ClassData/Listclass/{searchkey?}")]

        public IEnumerable<Classes> Listclass(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from classes where classcode like @key or classname like @key";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            List<Classes> classlist = new List<Classes>();

            //Gather Result Set of Query into a class object
            MySqlDataReader resultset = cmd.ExecuteReader();
            while (resultset.Read())
            {
                Classes newclass = new Classes();
                newclass.ClassId = Convert.ToInt32(resultset["classid"]);
                newclass.ClassName = resultset["classname"].ToString();
                newclass.ClassCode = resultset["classcode"].ToString();

                classlist.Add(newclass);

            }

            //close connection
            conn.Close();

            return classlist;
        }

        /// <summary>
        /// Returns a class from the database by specifying the primary key classid
        /// </summary>
        /// <param name="id">the class's ID in the database</param>
        /// <returns>class object</returns>
        [HttpGet]
        public Classes FindClass(int id)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * from classes where classid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a class object
            MySqlDataReader Resultset = cmd.ExecuteReader();
            Classes newclass = new Classes();


            while (Resultset.Read())
            {

                newclass.ClassName = Resultset["classname"].ToString();
                newclass.ClassCode = Resultset["classcode"].ToString();
                newclass.StartDate = Resultset["startdate"].ToString();
                newclass.FinishDate = Resultset["finishdate"].ToString();



            }
            //close Connection
            conn.Close();
            return newclass;


        }
        /// <summary>
        /// takes the class id and deleted that class from the database
        /// </summary>
        /// <param name="id">Class id</param>
        /// <example>POST : /api/ClassData/DeleteClass/7</example>
        [HttpPost]
        public void DeleteClass(int id)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Delete from classes where classid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            conn.Close();

        }

        /// <summary>
        /// Adds an Class to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewClass">An object with fields that map to the columns of the Class's table. </param>
        /// <example>
        /// POST api/ClassData/AddClass
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"Classname":"http5001",
        ///	"ClassCode":"Web development",
        ///	"StartDate":"2022-08-28",
        ///	"Finishdate":"2022-11-30"
        /// }
        /// </example>
        [HttpPost]
        public void AddClass([FromBody] Classes newclass)
        {
            //Create an instance of a connection
            MySqlConnection conn = sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Insert into classes (classcode, classname, startdate, finishdate) values (@classcode, @classname, @startdate, @finishdate)";
            cmd.Parameters.AddWithValue("@classcode", newclass.ClassCode);
            cmd.Parameters.AddWithValue("@classname", newclass.ClassName);
            cmd.Parameters.AddWithValue("@startdate", newclass.StartDate);
            cmd.Parameters.AddWithValue("@finishdate", newclass.FinishDate);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            conn.Close();

        }
    }
}
