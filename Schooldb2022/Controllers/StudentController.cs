using Schooldb2022.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schooldb2022.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        //GET : /Student/List
        public ActionResult List(string SearchKey = null)
        {
            StudentDataController controller=new StudentDataController();
            IEnumerable<Students> students = controller.ListStudent(SearchKey);
            return View(students);  
        }

        //GET : /Student/Show/{id}
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Students newstudent = controller.FindStudent(id);

            return View(newstudent);
        }

        //GET : /Student/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            StudentDataController controller = new StudentDataController();
            Students newstudent = controller.FindStudent(id);

            return View(newstudent);
        }

        //POST : /Student/Delete/{id

        public ActionResult Delete(int id)
        {
            StudentDataController controller = new StudentDataController();
            controller.DeleteStudent(id);
            return RedirectToAction("List");
        }

        public ActionResult New()
        {
            return View();
        }

        //POST :/Student/Create
        [HttpPost]
        public ActionResult Create(string StudentFname, string StudentLname, string StudentNumber, string Enroldate)
        {


            Students newstudent = new Students();

            newstudent.StudentFname = StudentFname;
            newstudent.StudentLname = StudentLname;
            newstudent.StudentNumber = StudentNumber;
            newstudent.Enroldate = Enroldate;

            StudentDataController controller = new StudentDataController();
            controller.AddStudent(newstudent);

            return RedirectToAction("List");
        }
    }
}