using Schooldb2022.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Schooldb2022.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Class/List
        public ActionResult List(string SearchKey = null)
        {
            ClassDataController controller = new ClassDataController();
            IEnumerable<Classes> listclass = controller.Listclass(SearchKey);

            return View(listclass);
        }
        //GET : /Class/Show/{id}
        public ActionResult Show(int id)
        {
            ClassDataController controller = new ClassDataController();
            Classes newclass = controller.FindClass(id);
            return View(newclass);

        }
        //GET : /Class/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            ClassDataController controller = new ClassDataController();
            Classes newclass = controller.FindClass(id);
            return View(newclass);
        }

        //POST : /Class/Delete/{id

        public ActionResult Delete(int id)
        {
            ClassDataController controller = new ClassDataController();
            controller.DeleteClass(id);
            return RedirectToAction("List");
        }

        public ActionResult New()
        {
            return View();
        }

        //POST :/Class/Create
        [HttpPost]
        public ActionResult Create(string ClassCode, string ClassName, string StartDate, string FinishDate)
        {


            Classes newclass = new Classes();

            newclass.ClassCode = ClassCode;
            newclass.ClassName = ClassName;
            newclass.StartDate = StartDate;
            newclass.FinishDate = FinishDate;

            ClassDataController controller = new ClassDataController();
            controller.AddClass(newclass);

            return RedirectToAction("List");
        }

    }
}