using LMS_GL.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_GL.Controllers
{
    public class AddToCartController : Controller
    {
        public ActionResult Add(Courses mo)
        {

            if (Session["cart"] == null)
            {
                List<Courses> li = new List<Courses>();

                li.Add(mo);
                Session["cart"] = li;
                ViewBag.cart = li.Count();


                Session["count"] = 1;


            }
            else
            {
                List<Courses> li = (List<Courses>)Session["cart"];
                li.Add(mo);
                Session["cart"] = li;
                ViewBag.cart = li.Count();
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;

            }
            return RedirectToAction("Index", "Home");
        }
    }
}
