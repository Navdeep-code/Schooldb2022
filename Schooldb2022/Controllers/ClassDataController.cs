using MySql.Data.MySqlClient;
using Schooldb2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            MySqlDataReader resultset =cmd.ExecuteReader();
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
            MySqlConnection conn =sdb.AccessDatabase();

            //Open the connection between the web server and database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd =conn.CreateCommand();
            cmd.CommandText = "SELECT classes.* , concat(teachers.teacherfname,\" \",teachers.teacherlname) as teachername FROM classes join teachers on teachers.teacherid=classes.teacherid where classid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a class object
            MySqlDataReader Resultset =cmd.ExecuteReader();
            Classes newclass = new Classes();

        
            while (Resultset.Read())
            {
                
                newclass.ClassName = Resultset["classname"].ToString() ;
                newclass.ClassCode = Resultset["classcode"].ToString();
                newclass.StartDate = Resultset["startdate"].ToString();
                newclass.FinishDate = Resultset["finishdate"].ToString();
                newclass.Teachername = Resultset["teachername"].ToString();
                newclass.TeacherId = Convert.ToInt32(Resultset["teacherid"]);

              
            }
            //close Connection
            conn.Close();
            return newclass;


        }
    }
}
