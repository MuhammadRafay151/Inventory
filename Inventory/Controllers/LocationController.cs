using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Inventory.Models;
using Newtonsoft.Json;
namespace Inventory.Controllers
{
    public class LocationController : Controller
    {
        // GET: Location
        public ActionResult Index()
        {
            ViewBag.data = Location.GetLocations();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Location l1)
        {
            if (ModelState.IsValid)
            {
                l1.create();
                return RedirectToAction("Index");
            }
            return View("Index", l1);
        }

        public JsonResult Edit(int id)
        {
            return Json(JsonConvert.SerializeObject(Location.GetLocation(id).Tables[0]), JsonRequestBehavior.AllowGet);
        }

        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit([Form]Location l1)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    l1.Update();
                   
                }
                return RedirectToAction("Index");

            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            Location l1 = new Location() { id = id };
            l1.Delete();
            return RedirectToAction("Index");
        }

        // POST: Location/Delete/5

    }
}
