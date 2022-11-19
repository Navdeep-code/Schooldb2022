using Schooldb2022.Models;
using System;
using System.Collections.Generic;
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
    }
}