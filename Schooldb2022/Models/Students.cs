using System.Collections.Generic;

namespace Schooldb2022.Models
{
    public class Students
    {

        //The following fields define a Student
        public int StudentId;
        public string StudentFname;
        public string StudentLname;
        public string StudentNumber;
        public string Enroldate;
        public List<string> Courses;

        public Students() { }
    }
}