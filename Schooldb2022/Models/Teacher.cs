using System;

namespace Schooldb2022.Models
{
    public class Teacher
    {
        //The following fields define a Teacher
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string EmployeeNumber;
        public decimal Salary;
        public DateTime Hiredate;
        public string Course;

        public Teacher() { }
    }
}