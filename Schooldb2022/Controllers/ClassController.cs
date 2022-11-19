using Schooldb2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            IEnumerable<Classes> listclass=controller.Listclass(SearchKey);

            return View(listclass);
        }
        //GET : /Class/Show/{id}
        public ActionResult Show(int id)
        {
            ClassDataController controller=new ClassDataController();
            Classes newclass = controller.FindClass(id);
            return View(newclass);

        }
        
    }
}