using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace Schooldb2022.Models
{
    public class SchooldbContext
    {
      private static string User { get { return "root";} }

      private static string Password { get { return "root"; } }

      private static string Database { get { return "school"; } }

      private static string Server { get { return "localhost"; } }

      private static string Port { get { return "3306"; } }

        //ConnectionString is a series of credentials used to connect to the database.
        protected static string ConnectionString { get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
       } }
        /// <summary>
        /// Returns a connection to the school database.
        /// </summary>
        /// <example>
        /// private SchooldbContext sdb = new SchooldbContext();
        /// MySqlConnection Conn = sdb.AccessDatabase();
        /// </example>
        /// <returns>A MySqlConnection Object</returns>
        public MySqlConnection AccessDatabase()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
   
}