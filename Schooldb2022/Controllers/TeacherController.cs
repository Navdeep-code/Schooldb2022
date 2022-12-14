using Schooldb2022.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

namespace Schooldb2022.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.listTeacher(SearchKey);
            return View(teachers);
        }

        //GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);

            return View(NewTeacher);
        }

        //GET : /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);

            return View(NewTeacher);
        }

        //POST : /Teacher/Delete/{id

        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET :/Teacher/New

        public ActionResult New()
        {
            return View();
        }

        //POST :/Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, string Hiredate, decimal Salary)
        {

            Debug.WriteLine("I have access");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(EmployeeNumber);
            Debug.WriteLine(Hiredate);
            Debug.WriteLine(Salary);

            Teacher newteacher = new Teacher();

            newteacher.TeacherFname = TeacherFname;
            newteacher.TeacherLname = TeacherLname;
            newteacher.EmployeeNumber = EmployeeNumber;
            newteacher.Hiredate = Convert.ToDateTime(Hiredate);
            newteacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(newteacher);

            return RedirectToAction("List");
        }

        //GET: /Teacher/Edit/{id}
        [HttpGet]
        public ActionResult Update(int id)
        {
            //i need to get the information about the teacher
            TeacherDataController controller = new TeacherDataController();
            Teacher selectedteacher = controller.FindTeacher(id);
            return View(selectedteacher);
        }

        public ActionResult Ajax_Update(int id)
        {
            //i need to get the information about the teacher
            TeacherDataController controller = new TeacherDataController();
            Teacher selectedteacher = controller.FindTeacher(id);
            return View(selectedteacher);
        }

        //POST: /Teacher/Update/{id}

        [HttpPost]
        public ActionResult Update(int Id, string TeacherFname, string TeacherLname, string EmployeeNumber, string Hiredate, decimal Salary)
        {
            Debug.WriteLine(TeacherLname);
           // Debug.WriteLine(EmployeeNumber);
           // Debug.WriteLine(Hiredate);
            //Debug.WriteLine(Salary);
           // Debug.WriteLine(TeacherFname);

            //Server-Side rendering validation
            if (TeacherFname=="" || TeacherLname=="" || EmployeeNumber=="" || Hiredate==""|| Salary.Equals(null))
            {
                if (TeacherFname=="")
                {
                    ViewData["ErrorFname"] = "*Please fill in the FirstName";
                }
                if (TeacherLname=="")
                {
                    ViewData["ErrorLname"] = "*Please fill in the LastName";
             
                }
                if (EmployeeNumber=="")
                {
                    ViewData["ErrorEnum"] = "*Please fill in the Employee Number";
                }
                if (Hiredate=="")
                {
                    ViewData["ErrorHdate"] = "*Please fill in the Hire Date";
                }
                if (Salary.Equals(null))
                {
                    ViewData["ErrorSal"] = "*Please fill in the Salary";
                }
                TeacherDataController controller = new TeacherDataController();
                Teacher selectedteacher = controller.FindTeacher(Id);
                return View(selectedteacher);
            }
            else
            {
                //Else Update the teacher data

                Teacher UpdatedTeacher = new Teacher();
                UpdatedTeacher.TeacherFname = TeacherFname;
                UpdatedTeacher.TeacherLname = TeacherLname;
                UpdatedTeacher.EmployeeNumber = EmployeeNumber;
                UpdatedTeacher.Salary = Salary;
                UpdatedTeacher.Hiredate = Convert.ToDateTime(Hiredate);
                TeacherDataController controller = new TeacherDataController();
                controller.UpdateTeacher(Id, UpdatedTeacher);
                return RedirectToAction("Show/" + Id);
            }


        }
    }
}